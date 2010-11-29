using System;
using System.ComponentModel;
using System.Reflection;

namespace ClashEngine.NET.Data
{
	using Interfaces.Data;

	/// <summary>
	/// Jednopoziomowa właściwość - obiekt główny + właściwość/pole.
	/// </summary>
	public class OneLevelPropertyPath
		: IPropertyPath
	{
		#region Private fields
		private object _Root = null;
		private MemberInfo Member = null;
		private Type _RootType = null;
		private string MemberName = string.Empty;
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

				if (this.Initialized && this._Root is INotifyPropertyChanged)
				{
					(this.Root as INotifyPropertyChanged).PropertyChanged -= this.OnValueChanged;
				}
				this._Root = value;
				if (this.Initialized && this._Root is INotifyPropertyChanged)
				{
					(this.Root as INotifyPropertyChanged).PropertyChanged += this.OnValueChanged;
				}
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
		/// Typ wartości.
		/// </summary>
		public Type ValueType
		{
			get
			{
				return (this.Member is PropertyInfo ? (this.Member as PropertyInfo).PropertyType : (this.Member as FieldInfo).FieldType);
			}
		}

		/// <summary>
		/// Konwerter typów dla wartości.
		/// </summary>
		public TypeConverter ValueConverter { get; private set; }

		/// <summary>
		/// Aktualna wartość.
		/// </summary>
		public object Value
		{
			get
			{
				if (!this.Initialized)
				{
					throw new InvalidOperationException("Initialize first");
				}
				return (this.Member is PropertyInfo ? (this.Member as PropertyInfo).GetValue(this.Root, null) : (this.Member as FieldInfo).GetValue(this.Root));
			}
			set
			{
				if (!this.Initialized)
				{
					throw new InvalidOperationException("Initialize first");
				}
				if(this.Member is PropertyInfo)
				{
					(this.Member as PropertyInfo).SetValue(this.Root, value, null);
				}
				else
				{
					(this.Member as FieldInfo).SetValue(this.Root, value);
				}
			}
		}

		/// <summary>
		/// Czy obiekt został zainicjalizowany.
		/// </summary>
		public bool Initialized { get; private set; }
		#endregion

		#region INotifyPropertyChanged Members
		/// <summary>
		/// Wywoływane gdy zmieni się Value.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region ISupportInitialize Members
		/// <summary>
		/// Nieużywane.
		/// </summary>
		public void BeginInit()
		{ }

		/// <summary>
		/// Kończy inicjalizację obiektu.
		/// </summary>
		/// <remarks>
		/// Pobiera, jeśli trzeba, Member.
		/// Rejestruje, jeśli może, zdarzenie PropertyChanged.
		/// </remarks>
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
			if (this.Member == null)
			{
				this.Member = this.RootType.GetProperty(this.MemberName);
				if (this.Member == null)
				{
					this.Member = this.RootType.GetField(this.MemberName);
					if (this.Member == null)
					{
						throw new ArgumentException(string.Format("Cannot find property nor field named {0} in type {1}", this.MemberName, this.RootType.Name));
					}
				}
			}

			this.ValueConverter = Converters.Utilities.GetTypeConverter(this.Member);

			if (this.Root is INotifyPropertyChanged)
			{
				(this.Root as INotifyPropertyChanged).PropertyChanged += this.OnValueChanged;
			}

			this.Initialized = true;
		}
		#endregion

		#region Constructors
		#region One parameter
		/// <summary>
		/// Inicjalizuje ścieżkę.
		/// </summary>
		/// <param name="name">Nazwa pola właściwości.</param>
		/// <exception cref="ArgumentNullException">name jest puste bądź jest nullem.</exception>
		public OneLevelPropertyPath(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name");
			}
			this.MemberName = name;
		}

		/// <summary>
		/// Inicjalizuje ścieżkę.
		/// </summary>
		/// <param name="property">Właściwość.</param>
		/// <exception cref="ArgumentNullException">property jest nullem.</exception>
		public OneLevelPropertyPath(PropertyInfo property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			this.Member = property;
		}

		/// <summary>
		/// Inicjalizuje ścieżkę.
		/// </summary>
		/// <param name="field">Pole.</param>
		/// <exception cref="ArgumentNullException">field jest nullem.</exception>
		public OneLevelPropertyPath(FieldInfo field)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			this.Member = field;
		}
		#endregion

		#region With object
		/// <summary>
		/// Inicjalizuje ścieżkę.
		/// </summary>
		/// <param name="name">Nazwa pola właściwości.</param>
		/// <param name="root">Obiekt główny.</param>
		/// <exception cref="ArgumentNullException">name jest puste bądź jest nullem.</exception>
		public OneLevelPropertyPath(string name, object root)
			: this(name)
		{
			this.Root = root;
		}

		/// <summary>
		/// Inicjalizuje ścieżkę.
		/// </summary>
		/// <param name="property">Właściwość.</param>
		/// <param name="root">Obiekt główny.</param>
		/// <exception cref="ArgumentNullException">property jest nullem.</exception>
		public OneLevelPropertyPath(PropertyInfo property, object root)
			: this(property)
		{
			this.Root = root;
		}

		/// <summary>
		/// Inicjalizuje ścieżkę.
		/// </summary>
		/// <param name="field">Pole.</param>
		/// <param name="root">Obiekt główny.</param>
		/// <exception cref="ArgumentNullException">field jest nullem.</exception>
		public OneLevelPropertyPath(FieldInfo field, object root)
			: this(field)
		{
			this.Root = root;
		}
		#endregion

		#region With type
		/// <summary>
		/// Inicjalizuje ścieżkę.
		/// </summary>
		/// <param name="name">Nazwa pola właściwości.</param>
		/// <param name="rootType">Typ obiektu głównego.</param>
		/// <exception cref="ArgumentNullException">name jest puste bądź jest nullem.</exception>
		public OneLevelPropertyPath(string name, Type rootType)
			: this(name)
		{
			this.RootType = rootType;
		}

		/// <summary>
		/// Inicjalizuje ścieżkę.
		/// </summary>
		/// <param name="property">Właściwość.</param>
		/// <param name="rootType">Typ obiektu głównego.</param>
		/// <exception cref="ArgumentNullException">property jest nullem.</exception>
		public OneLevelPropertyPath(PropertyInfo property, Type rootType)
			: this(property)
		{
			this.RootType = rootType;
		}

		/// <summary>
		/// Inicjalizuje ścieżkę.
		/// </summary>
		/// <param name="field">Pole.</param>
		/// <param name="rootType">Typ obiektu głównego.</param>
		/// <exception cref="ArgumentNullException">field jest nullem.</exception>
		public OneLevelPropertyPath(FieldInfo field, Type rootType)
			: this(field)
		{
			this.RootType = rootType;
		}
		#endregion
		#endregion

		#region Private members
		private void OnValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == this.Member.Name && this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(this.Member.Name));
			}
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			if (this.Root is INotifyPropertyChanged)
			{
				(this._Root as INotifyPropertyChanged).PropertyChanged -= this.OnValueChanged;
			}
			this._Root = null;
			this.Member = null;
			this._RootType = null;
		}
		#endregion
	}
}
