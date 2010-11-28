using System;
using System.Collections.Generic;
using System.Linq;

namespace ClashEngine.NET.Data
{
	using Interfaces.Data;

	/// <summary>
	/// Ścieżka do właściwości lub pola.
	/// </summary>
	/// <remarks>
	/// Po utworzeniu(po ustawieniu na wartości poprawne Path i Root) nie można zmieniać typu(i tylko jego) wartości głównej.
	/// </remarks>
	public class PropertyPath
		: IPropertyPath
	{
		#region Private fields
		private object _Root = null;
		private Type _RootType = null;

		private List<Internals.IPropertyLevel> Levels = new List<Internals.IPropertyLevel>();
		private int PathLevels = 0;

		private bool Initialized = false;
		#endregion

		#region IPropertyPath Members
		/// <summary>
		/// Ścieżka jako tekst.
		/// </summary>
		public string Path { get; private set; }

		/// <summary>
		/// Obiekt główny.
		/// </summary>
		public object Root
		{
			get { return this._Root; }
			set
			{
				if (this.RootType != null && !this.RootType.IsInstanceOfType(value))
				{
					throw new ArgumentException(string.Format("Value must be of type {0}", this.RootType.Name), "value");
				}
				this._Root = value;
				this.Evaluate();
			}
		}

		/// <summary>
		/// Typ obiektu głównego.
		/// </summary>
		public Type RootType
		{
			get { return this._RootType; }
			set
			{
				if (this.Initialized)
				{
					throw new NotSupportedException("Already initialized");
				}
				this._RootType = value;
			}
		}

		/// <summary>
		/// Aktualna wartość.
		/// </summary>
		public object Value
		{
			get { return this.Levels[this.Levels.Count - 1].Value; }
			set
			{
				this.Levels[this.Levels.Count - 1].SetValue(
					this.Levels.Count == 1 ? this.Root : this.Levels[this.Levels.Count - 2].Value
				, value);
			}
		}
		#endregion

		#region INotifyPropertyChanged Members
		/// <summary>
		/// Wywoływane gdy zmieni się docelowa wartość lub którakolwiek wartość przed.
		/// </summary>
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region ISupportInitialize Members
		public void BeginInit()
		{ }

		public void EndInit()
		{
			if (this.RootType == null && this.Root != null)
			{
				this.RootType = this.Root.GetType();
			}
			else if (this.RootType == null)
			{
				throw new InvalidOperationException("RootType must be set.");
			}
			this.PathLevels = this.Path.Count(c => c == '.' || c == '[') + 1;

			//Parsujemy ścieżkę
			var parts = this.Path.Split('.');
			var lastType = this.RootType;
			for (int i = 0, j = 0; i < parts.Length; ++i, ++j)
			{
				parts[i] = parts[i].Trim();
				int openBracket = parts[i].IndexOf('[');
				if (openBracket > -1)
				{
					int closeBracket = parts[i].IndexOf(']');
					if (closeBracket < openBracket)
					{
						throw new ArgumentException("Invalid format", "Path");
					}
					Internals.IPropertyLevel lvl = new Internals.PropertyLevel(lastType, parts[0].Substring(0, openBracket), j, this.ValueChanged);
					this.Levels.Add(lvl);
					lastType = this.HandleType(lvl);
					++j;

					lvl = new Internals.IndexerLevel(lastType, CreateIndecies(parts[0].Substring(openBracket + 1, closeBracket - openBracket - 1)), j, this.ValueChanged);
					this.Levels.Add(lvl);
					lastType = this.HandleType(lvl);
				}
				else
				{
					var lvl = new Internals.PropertyLevel(lastType, parts[i], j, this.ValueChanged);
					this.Levels.Add(lvl);
					lastType = this.HandleType(lvl);
				}
			}
			this.Evaluate();

			//Dodajemy zdarzenia PropertyChanged tam gdzie się da
			object lastObj = this.Root;
			foreach (var lvl in this.Levels)
			{
				lvl.RegisterPropertyChanged(lastObj);
				lastObj = lvl.Value;
			}
			this.Initialized = true;
		}
		#endregion

		#region Constructors
		public PropertyPath(string path)
		{
			this.Path = path;
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Zdarzenie zmiany którejś wartości w ścieżce.
		/// </summary>
		/// <param name="idx"></param>
		private void ValueChanged(int idx)
		{
			//Usuwamy zdarzenia PropertyChanged z obiektów.
			object prevObject = (idx == 0 ? this.Root : this.Levels[idx].Value);
			for (int i = idx; i < this.Levels.Count; i++)
			{
				this.Levels[i].UnregisterPropertyChanged(prevObject);
				prevObject = this.Levels[i].Value;
			}
			this.Evaluate(idx);
			//Dodajemy je do nowych
			prevObject = (idx == 0 ? this.Root : this.Levels[idx].Value);
			for (int i = idx; i < this.Levels.Count; i++)
			{
				this.Levels[i].RegisterPropertyChanged(prevObject);
				prevObject = this.Levels[i].Value;
			}

			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("Value"));
			}
		}

		/// <summary>
		/// Oblicza wszystkie wartości w ścieżce.
		/// </summary>
		/// <param name="startAt">Element początkowy.</param>
		private void Evaluate(int startAt = 0)
		{
			object lastObj = (startAt == 0 ? this.Root : this.Levels[startAt - 1].Value);
			for (int i = startAt; i < this.Levels.Count; i++)
			{
				if (lastObj == null)
				{
					break;
				}
				this.Levels[i].UpdateValue(lastObj);
				lastObj = this.Levels[i].Value;
			}
		}

		/// <summary>
		/// Obsługuje pobranie typu z poziomu.
		/// </summary>
		/// <param name="lvl"></param>
		/// <returns></returns>
		private Type HandleType(Internals.IPropertyLevel lvl)
		{
			if (lvl.Type == typeof(object))
			{
				this.Evaluate();
				if (this.Levels[this.Levels.Count - 1].Value == null)
				{
					throw new InvalidOperationException("Cannot evaluate full path");
				}
				return this.Levels[this.Levels.Count - 1].Value.GetType();
			}
			return lvl.Type;
		}

		/// <summary>
		/// Tworzy indeksy na podstawie ciągu znaków.
		/// Aktualnie wspierane:
		/// int
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private static object[] CreateIndecies(string str)
		{
			return new object[] { int.Parse(str) };
		}
		#endregion
	}
}
