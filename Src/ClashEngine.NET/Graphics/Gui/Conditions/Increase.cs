using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace ClashEngine.NET.Graphics.Gui.Conditions
{
	using Interfaces.Data;
	using Interfaces.Graphics.Gui.Conditions;
	using OpenTK;

	/// <summary>
	/// Zwiększa wartość.
	/// </summary>
	/// <remarks>
	/// Obsługiwane typu: byte, sbyte, short, ushort, int, uint, long, ulong, decimal, float, double, Vector2/3/4, char.
	/// </remarks>
	public class Increase
		: IIncrease, ISupportInitialize
	{
		#region Private fields
		private object ConvertedAmount = null;
		private Action SetMethod = null;
		#endregion

		#region IIncrease Members
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
		/// Wartość o jaką zostanie zwiększony.
		/// </summary>
		public object Amount { get; set; }

		/// <summary>
		/// Konwerter dla wartości.
		/// </summary>
		[DefaultValue(null)]
		[TypeConverter(typeof(Converters.SystemTypeConverter))]
		public Type CustomConverter { get; set; }
		#endregion

		#region ITrigger Members
		public void Trig()
		{
			if (this.SetMethod == null)
			{
				throw new InvalidOperationException("Initialize first");
			}
			this.SetMethod();
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

			this.DeduceMethod();

			this.ConvertedAmount = this.Amount;
			if (this.CustomConverter != null)
			{
				var converter = Activator.CreateInstance(this.CustomConverter) as TypeConverter;
				this.ConvertedAmount = converter.ConvertTo(this.Amount, this.Path.ValueType);
			}
			else
			{
				try
				{
					this.ConvertedAmount = Convert.ChangeType(this.Amount, this.Path.ValueType);
				}
				catch (InvalidCastException)
				{ }
			}

			if (!this.Path.ValueType.IsInstanceOfType(this.ConvertedAmount))
			{
				var targetConverter = TypeDescriptor.GetConverter(this.Path.ValueType);
				if (targetConverter != null && targetConverter.CanConvertFrom(this.ConvertedAmount.GetType()))
				{
					this.ConvertedAmount = targetConverter.ConvertFrom(this.ConvertedAmount);
				}
			}
		}
		#endregion

		#region Private methods
		private void DeduceMethod()
		{
			//byte, sbyte, short, ushort, int, uint, long, ulong, decimal, float, double, Vector2/3/4, char
			if (this.Path.ValueType == typeof(byte))
			{
				this.SetMethod = () => this.Path.Value = (byte)this.Path.Value + (byte)this.ConvertedAmount;
			}
			else if (this.Path.ValueType == typeof(sbyte))
			{
				this.SetMethod = () => this.Path.Value = (sbyte)this.Path.Value + (sbyte)this.ConvertedAmount;
			}
			else if (this.Path.ValueType == typeof(short))
			{
				this.SetMethod = () => this.Path.Value = (short)this.Path.Value + (short)this.ConvertedAmount;
			}
			else if (this.Path.ValueType == typeof(ushort))
			{
				this.SetMethod = () => this.Path.Value = (ushort)this.Path.Value + (ushort)this.ConvertedAmount;
			}
			else if (this.Path.ValueType == typeof(int))
			{
				this.SetMethod = () => this.Path.Value = (int)this.Path.Value + (int)this.ConvertedAmount;
			}
			else if (this.Path.ValueType == typeof(uint))
			{
				this.SetMethod = () => this.Path.Value = (uint)this.Path.Value + (uint)this.ConvertedAmount;
			}
			else if (this.Path.ValueType == typeof(long))
			{
				this.SetMethod = () => this.Path.Value = (long)this.Path.Value + (long)this.ConvertedAmount;
			}
			else if (this.Path.ValueType == typeof(ulong))
			{
				this.SetMethod = () => this.Path.Value = (ulong)this.Path.Value + (ulong)this.ConvertedAmount;
			}
			else if (this.Path.ValueType == typeof(decimal))
			{
				this.SetMethod = () => this.Path.Value = (decimal)this.Path.Value + (decimal)this.ConvertedAmount;
			}
			else if (this.Path.ValueType == typeof(float))
			{
				this.SetMethod = () => this.Path.Value = (float)this.Path.Value + (float)this.ConvertedAmount;
			}
			else if (this.Path.ValueType == typeof(double))
			{
				this.SetMethod = () => this.Path.Value = (double)this.Path.Value + (double)this.ConvertedAmount;
			}
			else if (this.Path.ValueType == typeof(Vector2))
			{
				this.SetMethod = () => this.Path.Value = (Vector2)this.Path.Value + (Vector2)this.ConvertedAmount;
			}
			else if (this.Path.ValueType == typeof(Vector3))
			{
				this.SetMethod = () => this.Path.Value = (Vector3)this.Path.Value + (Vector3)this.ConvertedAmount;
			}
			else if (this.Path.ValueType == typeof(Vector4))
			{
				this.SetMethod = () => this.Path.Value = (Vector4)this.Path.Value + (Vector4)this.ConvertedAmount;
			}
		}
		#endregion
	}
}
