﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Markup;

namespace ClashEngine.NET.Graphics.Gui.Conditions
{
	using Extensions;
	using Interfaces.Data;
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
		private bool _Value = false;
		private OperatorType _Operator = OperatorType.Equals;
		private Func<bool> Compare;
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
				this._Object = value;
			}
		}

		/// <summary>
		/// Ścieżka, do której przypisana zostanie wartość.
		/// </summary>
		[TypeConverter(typeof(Converters.PropertyPathConverter))]
		public IPropertyPath Path { get; set; }

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

		/// <summary>
		/// Operator porównania.
		/// </summary>
		public OperatorType Operator
		{
			get { return this._Operator; }
			set
			{
				this._Operator = value;
				this.DeduceOperatorMethod();
			}
		}

		/// <summary>
		/// Wyzwalacze wywoływane gdy warunek nie jest(ale był!) spełniony.
		/// </summary>
		public ITriggersCollection Else { get; private set; }
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
		{
			this.DeduceOperatorMethod();
		}

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

			this.Path.PropertyChanged += this.OnPathValueChanged;

			Type reqType = this.Path.ValueType;
			if (this.Operator == OperatorType.And)
			{
				this.CustomConverter = typeof(System.ComponentModel.Int64Converter);
				reqType = typeof(Int64);
			}

			this.ConvertedValue = this.Value;
			if (this.CustomConverter != null)
			{
				var converter = Activator.CreateInstance(this.CustomConverter) as TypeConverter;
				this.ConvertedValue = converter.ConvertTo(this.Value, reqType);
			}
			else
			{
				try
				{
					this.ConvertedValue = Convert.ChangeType(this.Value, reqType);
				}
				catch (InvalidCastException)
				{ }
			}

			if (this.ConvertedValue != null && !this.Path.ValueType.IsInstanceOfType(this.ConvertedValue))
			{
				var targetConverter = TypeDescriptor.GetConverter(reqType);
				if (targetConverter != null && targetConverter.CanConvertFrom(this.ConvertedValue.GetType()))
				{
					this.ConvertedValue = targetConverter.ConvertFrom(this.ConvertedValue);
				}
			}

			this.DeduceOperatorMethod();
			this._Value = this.Compare();
		}
		#endregion

		#region Constructors
		public If()
		{
			this.Triggers = new TriggersCollection();
			this.Else = new TriggersCollection();
		}
		#endregion

		#region Private methods
		void OnPathValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.Compare())
			{
				this.Triggers.TrigAll();
				if (!this._Value)
				{
					this._Value = true;
					this.PropertyChanged.Raise(this, () => Value);
				}
			}
			else if (this._Value)
			{
				this.Else.TrigAll();
				this._Value = false;
				this.PropertyChanged.Raise(this, () => Value);
			}
		}

		void DeduceOperatorMethod()
		{
			switch (this.Operator)
			{
			case OperatorType.Equals:
				this.Compare = () => this.Path.Value == this.ConvertedValue || this.Path.Value.Equals(this.ConvertedValue)
				|| ((this.ConvertedValue is IComparable) && (this.ConvertedValue as IComparable).CompareTo(this.Path.Value) == 0)
				|| ((this.Path.Value is IComparable) && (this.Path.Value as IComparable).CompareTo(this.ConvertedValue) == 0);
				break;

			case OperatorType.NotEquals:
				this.Compare = () => this.Path.Value != this.ConvertedValue || !this.Path.Value.Equals(this.ConvertedValue)
				|| ((this.ConvertedValue is IComparable) && (this.ConvertedValue as IComparable).CompareTo(this.Path.Value) != 0)
				|| ((this.Path.Value is IComparable) && (this.Path.Value as IComparable).CompareTo(this.ConvertedValue) != 0);
				break;

			case OperatorType.And:
				this.Compare = () =>
					{
						long val = (long)Convert.ChangeType(this.Path.Value, TypeCode.Int64);
						return (val & (long)this.ConvertedValue) != 0;
					};
				break;
			}
		}
		#endregion
	}
}
