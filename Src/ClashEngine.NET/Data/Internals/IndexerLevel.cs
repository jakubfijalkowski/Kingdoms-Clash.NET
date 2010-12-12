using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Text;

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
		private object[] Indecies = null;
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
			this.Value = this.Indexer.GetValue(root, this.Indecies);
		}

		/// <summary>
		/// Zmienia wartość dla danego obiektu.
		/// </summary>
		/// <param name="to">Obiekt.</param>
		/// <param name="value">Wartość.</param>
		public void SetValue(object to, object value)
		{
			this.Indexer.SetValue(to, value, this.Indecies);
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

		/// <summary>
		/// Pobiera konwerter typów.
		/// </summary>
		/// <returns></returns>
		public TypeConverter GetTypeConverter()
		{
			return Converters.Utilities.GetTypeConverter(this.Indexer);
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje nowy poziom.
		/// </summary>
		/// <param name="rootType">Typ - rodzic dla danego indeksera.</param>
		/// <param name="indecies">Indeksy(do sparsowania) dla indeksera.</param>
		/// <param name="level">Poziom - wewnętrzna reprezentacja.</param>
		/// <param name="valueChanged">Wywoływane gdy zmieni się wartość w danym poziomie.</param>
		public IndexerLevel(Type rootType, string indecies, int level, Action<int> valueChanged)
		{
			this.Level = level;
			this.ValueChanged = valueChanged;
			this.Indexer = rootType.GetProperty("Item");
			if (this.Indexer == null)
			{
				throw new ArgumentException(string.Format("Cannot find indexer in type {0}", rootType.Name));
			}

			this.Name = "Item";
			this.ParseIndecies(indecies);
			foreach (var idx in this.Indecies)
			{
				this.Name += "." + idx.ToString();
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

		private void ParseIndecies(string indecies)
		{
			var parameters = this.Indexer.GetIndexParameters();
			this.Indecies = new object[parameters.Length];

			//Musimy sparsować nasze parametry uwzględniając ' i "
			#region Parsing
			StringCollection s = new StringCollection();
			StringBuilder quoteBuilder = new StringBuilder();
			char isQuote = '\0';

			for (int i = 0; i < indecies.Length; ++i)
			{
				if (indecies[i] == '"' || indecies[i] == '\'') //' lub " - musimy pominąć teraz przecinki
				{
					if (indecies[i] == isQuote) //Kończymy zakres
					{
						quoteBuilder.Append(indecies.Substring(0, i).Trim());
						indecies = indecies.Remove(0, i + 1); //Usuwamy razem z ' lub "
						i = -1;
						isQuote = '\0';
					}
					else if (isQuote == '\0') //Nowy zakres!
					{
						isQuote = indecies[i];
						quoteBuilder.Append(indecies.Substring(0, i)); //Dodajemy to, co już było
						indecies = indecies.Remove(0, i + 1); //Usuwamy razem z " lub '
						i = 0;
					}
				}
				else if (isQuote == '\0' && indecies[i] == ',')
				{
					s.Add(quoteBuilder.ToString() + indecies.Substring(0, i).Trim());
					indecies = indecies.Remove(0, i + 1);
					quoteBuilder.Clear();
					i = -1;
				}
			}
			s.Add(quoteBuilder.ToString() + indecies);
			#endregion

			for (int i = 0; i < parameters.Length; i++)
			{
				if (s.Count > i)
				{
					var converter = TypeDescriptor.GetConverter(parameters[i]);
					if (converter.CanConvertFrom(typeof(string)))
					{
						this.Indecies[i] = converter.ConvertFrom(s[i]);
					}
					else
					{
						this.Indecies[i] = Convert.ChangeType(s[i], parameters[i].ParameterType);
					}
				}
				else if (parameters[i].DefaultValue != DBNull.Value)
				{
					this.Indecies[i] = parameters[i].DefaultValue;
				}
				else
				{
					throw new ArgumentException("Insufficient parameters");
				}
			}
		}
		#endregion
	}
}
