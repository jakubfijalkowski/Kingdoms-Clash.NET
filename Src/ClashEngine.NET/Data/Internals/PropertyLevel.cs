using System;
using System.ComponentModel;
using System.Reflection;

namespace ClashEngine.NET.Data.Internals
{
	/// <summary>
	/// Pojedynczy poziom w PropertyPath.
	/// </summary>
	internal sealed class PropertyLevel
		: IPropertyLevel
	{
		#region Private fields
		private MemberInfo Member = null;
		#endregion

		#region IPropertyLevel Members
		/// <summary>
		/// Numer poziomu.
		/// </summary>
		public int Level { get; private set; }

		/// <summary>
		/// Nazwa właściwości danego poziomu.
		/// </summary>
		public string Name { get { return this.Member.Name; } }

		/// <summary>
		/// Typ poziomu po pobraniu wartości.
		/// </summary>
		public Type Type { get { return (this.Member is PropertyInfo ? (this.Member as PropertyInfo).PropertyType : (this.Member as FieldInfo).FieldType); } }

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
			if (this.Member is PropertyInfo)
			{
				this.Value = (this.Member as PropertyInfo).GetValue(root, null);
			}
			else
			{
				this.Value = (this.Member as FieldInfo).GetValue(root);
			}
		}

		/// <summary>
		/// Zmienia wartość dla danego obiektu.
		/// </summary>
		/// <param name="to">Obiekt.</param>
		/// <param name="value">Wartość.</param>
		public void SetValue(object to, object value)
		{
			if (this.Member is PropertyInfo)
			{
				(this.Member as PropertyInfo).SetValue(to, value, null);
			}
			else
			{
				(this.Member as FieldInfo).SetValue(to, value);
			}
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
			return Converters.Utilities.GetTypeConverter(this.Member);
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje nowy poziom.
		/// </summary>
		/// <param name="rootType">Typ - rodzic dla danej właściwości.</param>
		/// <param name="name">Nazwa.</param>
		/// <param name="level">Poziom - wewnętrzna reprezentacja.</param>
		/// <param name="valueChanged">Wywoływane gdy zmieni się wartość w danym poziomie.</param>
		public PropertyLevel(Type rootType, string name, int level, Action<int> valueChanged)
		{
			this.Level = level;
			this.ValueChanged = valueChanged;

			this.Member = rootType.GetProperty(name);
			if (this.Member == null)
			{
				this.Member = rootType.GetField(name);
				if (this.Member == null)
				{
					throw new ArgumentException(string.Format("Cannot find property nor field names {0} in type {1}", name, rootType.Name));
				}
			}
		}
		#endregion

		#region Private methods
		private void OnValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == this.Name)
			{
				this.ValueChanged(this.Level);
			}
		}
		#endregion
	}
}
