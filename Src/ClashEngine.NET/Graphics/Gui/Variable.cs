using System;
using System.ComponentModel;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Zmienna XAML.
	/// </summary>
	[System.Diagnostics.DebuggerDisplay("Variable {Id,nq} of type {Value.GetType().Name,nq}")]
	public class Variable
		: IVariable
	{
		#region Private fields
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private object _Value = null;
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private Type _RequiredType = typeof(string);
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private TypeConverter Converter = new StringConverter();
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private bool WasCustomSet = false;
		#endregion

		#region IVariable Members
		/// <summary>
		/// Nazwa zmiennej.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Wartość zmiennej.
		/// </summary>
		public object Value
		{
			get { return this._Value; }
			set
			{
				if (this.RequiredType.IsInstanceOfType(value))
				{
					this._Value = value;
				}
				else
				{
					this._Value = this.Convert(value);
				}
				if (this.PropertyChanged != null)
				{
					this.PropertyChanged(this, new PropertyChangedEventArgs("Value"));
				}
			}
		}

		/// <summary>
		/// Wymagany typ.
		/// </summary>
		public Type RequiredType
		{
			get { return this._RequiredType; }
			set
			{
				this._RequiredType = value;
				if (!this.WasCustomSet)
				{
					var convs = this._RequiredType.GetCustomAttributes(typeof(TypeConverterAttribute), false);
					if (convs.Length == 1)
					{
						this.Converter = Activator.CreateInstance(Type.GetType((convs[0] as TypeConverterAttribute).ConverterTypeName)) as TypeConverter;
					}
					else
					{
						this.Converter = new TypeConverter();
					}
				}
				this._Value = this.Convert(this._Value);
			}
		}

		/// <summary>
		/// Konwerter wskazany przez użytkownika.
		/// </summary>
		public Type CustomConverter
		{
			get { return this.Converter.GetType(); }
			set
			{
				this.Converter = Activator.CreateInstance(value) as TypeConverter;
				this.WasCustomSet = true;
				this._Value = this.Convert(this._Value);
			}
		}
		#endregion

		#region INotifyPropertyChanged Members
		/// <summary>
		/// Wywoływane przy zmianie wartości.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Private members
		/// <summary>
		/// Konwertuje, jeśli może, wartość.
		/// </summary>
		/// <param name="from"></param>
		/// <returns></returns>
		private object Convert(object from)
		{
			if (from != null)
			{
				if (this.Converter.CanConvertFrom(from.GetType()))
				{
					return this.Converter.ConvertFrom(from);
				}
				else
				{
					try
					{
						return System.Convert.ChangeType(from, this.RequiredType, System.Globalization.CultureInfo.InvariantCulture);
					}
					catch
					{ }
				}
			}
			return from;
		}
		#endregion
	}
}
