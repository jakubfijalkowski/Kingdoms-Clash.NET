using System;
using System.ComponentModel;
using System.Reflection;

namespace ClashEngine.NET.Data.Internals
{
	/// <summary>
	/// Pojedynczy poziom(indekser) w PropertyPath.
	/// </summary>
	internal sealed class IndexerLevel
		: IPropertyLevel
	{		
		#region Private fields
		private PropertyInfo Indexer = null;
		private object[] Indexes = null;
		#endregion

		#region IPropertyLevel Members
		/// <summary>
		/// Numer poziomu.
		/// </summary>
		public int Level { get; private set; }

		/// <summary>
		/// Nazwa właściwości danego poziomu.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Typ poziomu po pobraniu wartości.
		/// </summary>
		public Type Type { get { return this.Indexer.PropertyType; } }

		/// <summary>
		/// Wartość.
		/// </summary>
		public object Value { get; private set; }
		
		/// <summary>
		/// Metoda, która będzie wywoływana przy zmianie Value.
		/// Parametr: Level.
		/// </summary>
		public Action<int> ValueChanged { get; private set; }

		/// <summary>
		/// Uaktualnia wartość dla danego poziomu.
		/// </summary>
		/// <param name="root">Obiekt przed.</param>
		public void UpdateValue(object root)
		{
			this.Value = this.Indexer.GetValue(root, this.Indexes);
		}

		/// <summary>
		/// Zmienia wartość dla danego obiektu.
		/// </summary>
		/// <param name="to">Obiekt.</param>
		/// <param name="value">Wartość.</param>
		public void SetValue(object to, object value)
		{
			this.Indexer.SetValue(to, value, this.Indexes);
			this.Value = value;
		}

		/// <summary>
		/// Rejestruje, jeśli może, zdarzenie Value.PropertyChanged.
		/// </summary>
		/// <param name="root">Obiekt, do którego przynależy dana właściwość.</param>
		public void RegisterPropertyChanged(object root)
		{
			if (root is INotifyPropertyChanged)
			{
				(root as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(OnValueChanged);
			}
		}

		/// <summary>
		/// Usuwa, jeśli może, zdarzenie Value.PropertyChanged.
		/// </summary>
		/// <param name="root">Obiekt, do którego przynależy dana właściwość.</param>
		public void UnregisterPropertyChanged(object root)
		{
			if (root is INotifyPropertyChanged)
			{
				(root as INotifyPropertyChanged).PropertyChanged -= new PropertyChangedEventHandler(OnValueChanged);
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje nowy poziom.
		/// </summary>
		/// <param name="rootType">Typ - rodzic dla danego indeksera.</param>
		/// <param name="indexes">Indeksy dla indeksera.</param>
		/// <param name="level">Poziom - wewnętrzna reprezentacja.</param>
		/// <param name="valueChanged">Wywoływane gdy zmieni się wartość w danym poziomie.</param>
		public IndexerLevel(Type rootType, object[] indexes, int level, Action<int> valueChanged)
		{
			this.Level = level;
			this.ValueChanged = valueChanged;
			this.Indexes = indexes;
			this.Indexer = rootType.GetProperty("Item");
			this.Name = "Item";
			foreach (var idx in indexes)
			{
				this.Name += "." + idx.ToString();
			}
			if (this.Indexer == null)
			{
				throw new ArgumentException(string.Format("Cannot find indexer in type {0}", rootType.Name));
			}
		}
		#endregion

		#region Private methods
		private void OnValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == this.Name || e.PropertyName == "Item")
			{
				this.ValueChanged(this.Level);
			}
		}
		#endregion
	}
}
