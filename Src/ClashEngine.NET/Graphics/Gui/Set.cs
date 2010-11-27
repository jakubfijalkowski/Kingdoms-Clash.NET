using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Prosty wyzwalacz - przypisanie.
	/// </summary>
	public class Set
		: ISet, ISupportInitialize
	{
		#region Private fields
		private object ConvertedValue = null;
		private PropertyInfo Property = null;
		#endregion

		#region ISet Members
		/// <summary>
		/// Obiekt docelowy.
		/// </summary>
		[TypeConverter(typeof(NameReferenceConverter))]
		public object Object { get; set; }

		/// <summary>
		/// Właściwość, której zostanie przypisana dana wartość.
		/// </summary>
		public string PropertyName { get; set; }

		/// <summary>
		/// Wartość do przypisania.
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		/// Konwerter dla wartości.
		/// </summary>
		[DefaultValue(null)]
		[TypeConverter(typeof(Converters.SystemTypeConverter))]
		public Type CustomConverter { get; set; }
		#endregion

		#region ITrigger Members
		/// <summary>
		/// Ustawia wartość wskazanej właściwości na podaną.
		/// </summary>
		public void Trig()
		{
			if (this.Property == null)
			{
				throw new InvalidOperationException("Initialize first");
			}
			this.Property.SetValue(this.Object, this.ConvertedValue, null);
		}
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
	}
}
