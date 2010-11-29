using System;
using System.ComponentModel;

namespace ClashEngine.NET.Data
{
	using Interfaces.Data;

	/// <summary>
	/// Wiązanie.
	/// </summary>
	public class Binding
		: IBinding
	{
		#region Private fields
		private TypeConverter SourceConverter = null;
		private TypeConverter TargetConverter = null;
		private bool ControlFlowSource = false;
		private bool ControlFlowTarget = false;
		#endregion

		#region IBinding Members
		/// <summary>
		/// Tryb bindowania.
		/// </summary>
		public BindingMode Mode { get; private set; }

		/// <summary>
		/// Obiekt źródłowy.
		/// </summary>
		public object Source { get; internal set; }

		/// <summary>
		/// Źródłowa właściwość.
		/// </summary>
		public IPropertyPath SourcePath { get; private set; }

		/// <summary>
		/// Obiekt docelowy.
		/// </summary>
		public object Target { get; private set; }

		/// <summary>
		/// Docelowa właściwość.
		/// </summary>
		public IPropertyPath TargetPath { get; private set; }

		/// <summary>
		/// Konwerter.
		/// Używany tylko, jeśli typ TargetProperty != typu SourceProperty lub ustawiono go ręcznie.
		/// </summary>
		public Type ConverterType { get; set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje wiązanie.
		/// </summary>
		/// <param name="source">Obiekt źródłowy.</param>
		/// <param name="sourceProperty">Ścieżka do źródła.</param>
		/// <param name="target">Obiekt docelowy.</param>
		/// <param name="targetProperty">Ścieżka do celu.</param>
		/// <param name="mode">Tryb wiązania.</param>
		/// <param name="converterType">Wymuszony typ konwertera.</param>
		/// <exception cref="ArgumentNullException">Któryś z argumentów jest nullem.</exception>
		internal Binding(object source, IPropertyPath sourceProperty, object target, IPropertyPath targetProperty, bool setTarget,
			BindingMode mode = BindingMode.OneWay, Type converterType = null)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (sourceProperty == null)
			{
				throw new ArgumentNullException("sourceProperty");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (targetProperty == null)
			{
				throw new ArgumentNullException("targetProperty");
			}
			this.Source = source;
			this.SourcePath = sourceProperty;
			if (!this.SourcePath.Initialized)
			{
				this.SourcePath.BeginInit();
				this.SourcePath.Root = this.Source;
				this.SourcePath.EndInit();
			}
			else
			{
				this.SourcePath.Root = this.Source;
			}

			this.Target = target;
			this.TargetPath = targetProperty;
			if (!this.TargetPath.Initialized)
			{
				this.TargetPath.BeginInit();
				this.TargetPath.Root = this.Target;
				this.TargetPath.EndInit();
			}
			else
			{
				this.TargetPath.Root = this.Target;
			}

			this.Mode = mode;
			this.ConverterType = converterType;
			this.SetBindings();

			if (setTarget)
			{
				this.TargetPath.Value = this.GetConvertedSource();
			}
		}

		/// <summary>
		/// Inicjalizuje wiązanie.
		/// </summary>
		/// <param name="source">Obiekt źródłowy.</param>
		/// <param name="sourceProperty">Ścieżka do źródła.</param>
		/// <param name="target">Obiekt docelowy.</param>
		/// <param name="targetProperty">Ścieżka do celu.</param>
		/// <param name="mode">Tryb wiązania.</param>
		/// <param name="converterType">Wymuszony typ konwertera.</param>
		/// <exception cref="ArgumentNullException">Któryś z argumentów jest nullem.</exception>
		public Binding(object source, IPropertyPath sourceProperty, object target, IPropertyPath targetProperty,
			BindingMode mode = BindingMode.OneWay, Type converterType = null)
			: this(source, sourceProperty, target, targetProperty, true, mode, converterType)
		{ }

		~Binding()
		{
			this.Dispose();
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Źródło do celu.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SourceToTarget(object sender, PropertyChangedEventArgs e)
		{
			if (this.ControlFlowSource)
			{
				this.ControlFlowSource = false;
				return;
			}
			this.ControlFlowTarget = true;

			this.TargetPath.Value = this.GetConvertedSource();
		}

		/// <summary>
		/// Cel do źródła.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TargetToSource(object sender, PropertyChangedEventArgs e)
		{
			if (this.ControlFlowTarget)
			{
				this.ControlFlowTarget = false;
				return;
			}
			this.ControlFlowSource = true;

			this.SourcePath.Value = this.GetConvertedTarget();
		}

		/// <summary>
		/// Pobiera obiekt ze źródła i, jeśli może, konwertuje go do typu docelowego.
		/// </summary>
		/// <returns></returns>
		private object GetConvertedSource()
		{
			object obj = this.SourcePath.Value;
			if (obj != null && this.SourceConverter.CanConvertTo(this.TargetPath.ValueType))
			{
				obj = this.SourceConverter.ConvertTo(obj, this.TargetPath.ValueType);
			}
			else if (obj != null && this.TargetConverter.CanConvertFrom(obj.GetType()))
			{
				obj = this.TargetConverter.ConvertFrom(obj);
			}
			return obj;
		}

		/// <summary>
		/// Pobiera obiekt z celu i, jeśli może, konwertuje go do typu źródła.
		/// </summary>
		/// <returns></returns>
		private object GetConvertedTarget()
		{
			object obj = this.TargetPath.Value;
			if (obj != null && this.TargetConverter.CanConvertTo(this.SourcePath.ValueType))
			{
				obj = this.TargetConverter.ConvertTo(obj, this.SourcePath.ValueType);
			}
			else if (obj != null && this.SourceConverter.CanConvertFrom(obj.GetType()))
			{
				obj = this.SourceConverter.ConvertFrom(obj);
			}
			return obj;
		}

		/// <summary>
		/// Ustawia bindowania.
		/// </summary>
		private void SetBindings()
		{
			#region Converters
			if (this.ConverterType != null)
			{
				this.SourceConverter = this.TargetConverter = Activator.CreateInstance(this.ConverterType) as TypeConverter;
			}
			else
			{
				this.SourceConverter = this.SourcePath.ValueConverter;
				this.TargetConverter = this.TargetPath.ValueConverter;
			}
			#endregion

			#region Bindings
			//Zależnie od trybu wiązania dodajemy odpowiednie zdarzenia.
			switch (this.Mode)
			{
			case BindingMode.OneWay:
				this.SourcePath.PropertyChanged += new PropertyChangedEventHandler(this.SourceToTarget);
				break;

			case BindingMode.TwoWay:
				this.SourcePath.PropertyChanged += new PropertyChangedEventHandler(this.SourceToTarget);
				this.TargetPath.PropertyChanged += new PropertyChangedEventHandler(this.TargetToSource);
				break;
			}
			#endregion
		}
		#endregion

		#region Internals
		/// <summary>
		/// Wymusza uaktualnienie celu(jeśli Mode != OneTime).
		/// Używane przez BindingExtension.
		/// </summary>
		internal void ForceUpdateTarget()
		{
			this.TargetPath.Value = this.SourcePath.Value;
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			this.SourcePath.Dispose();
			this.TargetPath.Dispose();
		}
		#endregion
	}
}
