using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;

namespace ClashEngine.NET.Graphics.Gui.Conditions
{
	using Interfaces.Graphics.Gui.Conditions;

	/// <summary>
	/// Wyzwalacz wywołujący metodę.
	/// </summary>
	[ContentProperty("Parameters")]
	public class Call
		: ICall, ISupportInitialize
	{
		#region Private fields
		private MethodInfo _Method;
		private object[] _Parameters;
		#endregion

		#region ICall Members
		/// <summary>
		/// Obiekt na którym będziemy wywoływać metodę.
		/// </summary>
		[TypeConverter(typeof(NameReferenceConverter))]
		public object Object { get; set; }

		/// <summary>
		/// Nazwa metody.
		/// </summary>
		public string Method { get; set; }

		/// <summary>
		/// Parametry wywołania metody.
		/// </summary>
		public IParametersCollection Parameters { get; private set; }
		#endregion

		#region ITrigger Members
		/// <summary>
		/// Wywołuje metodę.
		/// </summary>
		public void Trig()
		{
			this._Method.Invoke(this.Object, this._Parameters);
		}
		#endregion

		#region Constructors
		public Call()
		{
			this.Parameters = new ParametersCollection();
		}
		#endregion

		#region ISupportInitialize Members
		public void BeginInit()
		{ }

		public void EndInit()
		{
			if (this.Object == null)
			{
				throw new InvalidOperationException("Object cannot be null");
			}
			if (string.IsNullOrWhiteSpace(this.Method))
			{
				throw new InvalidOperationException("Method cannot be null or empty");
			}

			#region Deduct method
			var methods = this.Object.GetType().GetMethods();
			int leastParametersCnt = int.MaxValue;
			int leastParametersNo = -1;
			for (int i = 0; i < methods.Length; i++)
			{
				if (methods[i].Name == this.Method)
				{
					var parametersCnt = methods[i].GetParameters().Length;
					if (parametersCnt - this.Parameters.Count >= 0 && parametersCnt - this.Parameters.Count < leastParametersCnt)
					{
						leastParametersCnt = parametersCnt - this.Parameters.Count;
						leastParametersNo = i;
					}
				}
			}
			if (leastParametersNo == -1)
			{
				throw new InvalidOperationException("Cannot match parameters to method");
			}
			this._Method = methods[leastParametersNo];
			#endregion

			#region Parameters conversion
			var parameters = this._Method.GetParameters();
			this._Parameters = new object[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
			{
				if (i < this.Parameters.Count)
				{
					var converter = TypeDescriptor.GetConverter(parameters[i].ParameterType);
					if (this.Parameters[i].Value == null || parameters[i].ParameterType.IsInstanceOfType(this.Parameters[i].Value))
					{
						this._Parameters[i] = this.Parameters[i].Value;
					}
					else if (this.Parameters[i].Value != null && converter.CanConvertFrom(this.Parameters[i].Value.GetType()))
					{
						this._Parameters[i] = converter.ConvertFrom(this.Parameters[i].Value);
					}
					else
					{
						throw new InvalidOperationException("Cannot match parameters to method");
					}

					int newI = i;
					this.Parameters[i].PropertyChanged += (s, e) =>
					{
						this.ParameterChanged(newI, (s as IParameter).Value, parameters[newI].ParameterType);
					};
				}
				else if (parameters[i].DefaultValue != DBNull.Value)
				{
					this._Parameters[i] = parameters[i].DefaultValue;
				}
				else
				{
					throw new InvalidOperationException("Cannot match parameters to method");
				}
			}
			#endregion
		}
		#endregion

		#region Private methods
		private void ParameterChanged(int idx, object newValue, Type reqType)
		{
			var converter = TypeDescriptor.GetConverter(reqType);
			if (newValue == null || reqType.IsInstanceOfType(newValue))
			{
				this._Parameters[idx] = newValue;
			}
			else if (newValue != null && converter.CanConvertFrom(newValue.GetType()))
			{
				this._Parameters[idx] = converter.ConvertFrom(newValue);
			}
			else
			{
				try
				{
					this._Parameters[idx] = Convert.ChangeType(newValue, reqType);
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException("Cannot convert parameter", ex);
				}
			}
		}
		#endregion
	}
}
