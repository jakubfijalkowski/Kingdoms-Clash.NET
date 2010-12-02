using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace ClashEngine.NET.Graphics.Gui.Conditions
{
	using Interfaces.Data;
	using Interfaces.Graphics.Gui.Conditions;

	/// <summary>
	/// Prosty wyzwalacz - przypisanie.
	/// </summary>
	public class Set
		: ISet, ISupportInitialize
	{
		#region Private fields
		private object ConvertedValue = null;
		//private PropertyInfo Property = null;
		#endregion

		#region ISet Members
		/// <summary>
		/// Obiekt docelowy.
		/// </summary>
		[TypeConverter(typeof(NameReferenceConverter))]
		public object Object { get; set; }

		/// <summary>
		/// Ścieżka, do której przypisana zostanie wartość.
		/// </summary>
		[TypeConverter(typeof(Converters.PropertyPathConverter))]
		public IPropertyPath Path { get; set; }

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
			if (this.Path == null || !this.Path.Initialized)
			{
				throw new InvalidOperationException("Initialize first");
			}
			this.Path.Value = this.ConvertedValue;
		}
		#endregion

		#region ISupportInitialize Members
		public void BeginInit()
		{ }

		public void EndInit()
		{
			if (this.Object == null || this.Path == null)
			{
				throw new InvalidOperationException("Properties Object and Path must be set");
			}

			if (!this.Path.Initialized)
			{
				this.Path.BeginInit();
				this.Path.Root = this.Object;
				this.Path.EndInit();
			}
			else
			{
				this.Path.Root = this.Object;
			}

			this.ConvertedValue = this.Value;
			if (this.CustomConverter != null)
			{
				var converter = Activator.CreateInstance(this.CustomConverter) as TypeConverter;
				this.ConvertedValue = converter.ConvertTo(this.Value, this.Path.ValueType);
			}
			else
			{
				try
				{
					this.ConvertedValue = Convert.ChangeType(this.Value, this.Path.ValueType);
				}
				catch (InvalidCastException)
				{ }
			}

			if (!this.Path.ValueType.IsInstanceOfType(this.ConvertedValue))
			{
				var targetConverter = TypeDescriptor.GetConverter(this.Path.ValueType);
				if (targetConverter != null && targetConverter.CanConvertFrom(this.ConvertedValue.GetType()))
				{
					this.ConvertedValue = targetConverter.ConvertFrom(this.ConvertedValue);
				}
			}
		}
		#endregion
	}
}
