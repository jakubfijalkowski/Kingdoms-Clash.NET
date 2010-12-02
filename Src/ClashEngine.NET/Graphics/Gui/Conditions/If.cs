using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Markup;

namespace ClashEngine.NET.Graphics.Gui.Conditions
{
	using Interfaces.Graphics.Gui.Conditions;

	/// <summary>
	/// Prosty warunek - jeśli coś jest równe czemuś ustaw coś na daną wartość.
	/// </summary>
	[DebuggerDisplay("If {Object.GetType().Name,nq}.{PropertyName,nq} = {ConvertedValue,nq}")]
	[ContentProperty("Triggers")]
	public class If
		: IIf, ISupportInitialize, IMultiIfCondition
	{
		#region Private fields
		private object _Object = null;
		private object ConvertedValue = null;
		private PropertyInfo Property = null;
		private bool _Value = false;
		#endregion

		#region ICondition Members
		/// <summary>
		/// Wyzwalacze wywoływane przy spełnieniu warunku.
		/// </summary>
		public ITriggersCollection Triggers { get; private set; }
		#endregion

		#region IIf Members
		/// <summary>
		/// Obiekt, z którego pobrana będzie wartość.
		/// </summary>
		[TypeConverter(typeof(NameReferenceConverter))]
		public object Object
		{
			get { return this._Object; }
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!(value is INotifyPropertyChanged))
				{
					throw new ArgumentException("Must implement INotifyPropertyChanged interface", "value");
				}
				this._Object = value;
			}
		}

		/// <summary>
		/// Właściwość.
		/// </summary>
		public string PropertyName { get; set; }

		/// <summary>
		/// Wartość do porównywania.
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		/// Konwerter dla wartości.
		/// </summary>
		[DefaultValue(null)]
		[TypeConverter(typeof(Converters.SystemTypeConverter))]
		public Type CustomConverter { get; set; }
		#endregion

		#region IMultiIfCondition Members
		/// <summary>
		/// Aktualna wartość warunku.
		/// </summary>
		bool IMultiIfCondition.Value
		{
			get { return this._Value; }
		}
		#endregion

		#region INotifyPropertyChanged Members
		/// <summary>
		/// Wywoływane przy zmianie IMultiIfCondition.Value.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region ISupportInitialize Members
		public void BeginInit()
		{ }

		public void EndInit()
		{
			if (this.Object == null || string.IsNullOrWhiteSpace(this.PropertyName))
			{
				throw new InvalidOperationException("Properties Object and Property must be set");
			}
			this.Property = this.Object.GetType().GetProperty(this.PropertyName.Trim());
			if (this.Property == null)
			{
				throw new InvalidOperationException(string.Format("Cannot find property {0} in object", this.PropertyName.Trim()));
			}
			(this.Object as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(OnObjectPropertyChanged);

			this.ConvertedValue = this.Value;
			if (this.CustomConverter != null)
			{
				var converter = Activator.CreateInstance(this.CustomConverter) as TypeConverter;
				this.ConvertedValue = converter.ConvertTo(this.Value, this.Property.PropertyType);
			}
			else
			{
				try
				{
					this.ConvertedValue = Convert.ChangeType(this.Value, this.Property.PropertyType);
				}
				catch (InvalidCastException)
				{ }
			}

			if (!this.Property.PropertyType.IsInstanceOfType(this.ConvertedValue))
			{
				var targetConverter = Converters.Utilities.GetTypeConverter(this.Property);
				if (targetConverter != null && targetConverter.CanConvertFrom(this.ConvertedValue.GetType()))
				{
					this.ConvertedValue = targetConverter.ConvertFrom(this.ConvertedValue);
				}
			}
		}
		#endregion

		#region Constructors
		public If()
		{
			this.Triggers = new TriggersCollection();
		}
		#endregion

		#region Private methods
		void OnObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == this.PropertyName)
			{
				object newValue = this.Property.GetValue(this.Object, null);
				if (newValue == this.ConvertedValue || newValue.Equals(this.ConvertedValue)
					|| ((this.ConvertedValue is IComparable) && (this.ConvertedValue as IComparable).CompareTo(newValue) == 0)
					|| ((newValue is IComparable) && (newValue as IComparable).CompareTo(this.ConvertedValue) == 0))
				{
					this.Triggers.TrigAll();
					if (!this._Value)
					{
						this._Value = true;
						if (this.PropertyChanged != null)
						{
							this.PropertyChanged(this, new PropertyChangedEventArgs("Value"));
						}
					}
				}
				else if (this._Value)
				{
					this._Value = false;
					if (this.PropertyChanged != null)
					{
						this.PropertyChanged(this, new PropertyChangedEventArgs("Value"));
					}
				}
			}
		}
		#endregion
	}
}
