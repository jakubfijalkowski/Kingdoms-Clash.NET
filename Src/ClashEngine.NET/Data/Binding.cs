using System;
using System.ComponentModel;
using System.Reflection;

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
		private Type TargetType = null;
		private Type SourceType = null;
		private bool ControlFlow = false;
		private string PropertyName = string.Empty;
		#endregion

		#region IBinding Members
		/// <summary>
		/// Tryb bindowania.
		/// </summary>
		public BindingMode Mode { get; private set; }

		/// <summary>
		/// Obiekt źródłowy.
		/// </summary>
		public object Source { get; private set; }

		/// <summary>
		/// Źródłowa właściwość.
		/// </summary>
		public MemberInfo SourceProperty { get; private set; }

		/// <summary>
		/// Obiekt docelowy.
		/// </summary>
		public object Target { get; private set; }

		/// <summary>
		/// Docelowa właściwość.
		/// </summary>
		public MemberInfo TargetProperty { get; private set; }

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
		/// <param name="sourceProperty">Źródłowe pole/właściwość.</param>
		/// <param name="target">Obiekt docelowy.</param>
		/// <param name="targetProperty">Docelowe pole/właściwość.</param>
		/// <param name="mode">Tryb wiązania.</param>
		/// <exception cref="ArgumentNullException">Któryś z argumentów jest nullem.</exception>
		/// <exception cref="ArgumentException">sourceProperty albo targetProperty nie jest PropertyInfo albo FieldInfo.</exception>
		public Binding(object source, MemberInfo sourceProperty, object target, MemberInfo targetProperty, BindingMode mode = BindingMode.OneWay)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (sourceProperty == null)
			{
				throw new ArgumentNullException("sourceProperty");
			}
			if (!(sourceProperty is PropertyInfo) && !(sourceProperty is FieldInfo))
			{
				throw new ArgumentException("Must be PropertyInfo or FieldInfo", "sourceProperty");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (targetProperty == null)
			{
				throw new ArgumentNullException("targetProperty");
			}
			if (!(targetProperty is PropertyInfo) && !(targetProperty is FieldInfo))
			{
				throw new ArgumentException("Must be PropertyInfo or FieldInfo", "targetProperty");
			}
			this.Source = source;
			this.SourceProperty = sourceProperty;
			this.Target = target;
			this.TargetProperty = targetProperty;
			this.Mode = mode;
			this.SetBindings();
		}

		/// <summary>
		/// Inicjalizuje wiązanie.
		/// </summary>
		/// <param name="source">Obiekt źródłowy.</param>
		/// <param name="sourceProperty">Źródłowa właściwość.</param>
		/// <param name="target">Obiekt docelowy.</param>
		/// <param name="targetProperty">Docelowa właściwość.</param>
		/// <param name="mode">Tryb wiązania.</param>
		/// <exception cref="ArgumentNullException">Któryś z argumentów jest nullem.</exception>
		public Binding(object source, PropertyInfo sourceProperty, object target, PropertyInfo targetProperty, BindingMode mode = BindingMode.OneWay)
			: this(source, (MemberInfo)sourceProperty, target, (MemberInfo)targetProperty, mode)
		{ }

		/// <summary>
		/// Inicjalizuje wiązanie.
		/// </summary>
		/// <param name="source">Obiekt źródłowy.</param>
		/// <param name="sourceField">Źródłowe pole.</param>
		/// <param name="target">Obiekt docelowy.</param>
		/// <param name="targetProperty">Docelowa właściwość.</param>
		/// <param name="mode">Tryb wiązania.</param>
		/// <exception cref="ArgumentNullException">Któryś z argumentów jest nullem.</exception>
		public Binding(object source, FieldInfo sourceField, object target, PropertyInfo targetProperty, BindingMode mode = BindingMode.OneWay)
			: this(source, (MemberInfo)sourceField, target, (MemberInfo)targetProperty, mode)
		{ }

		/// <summary>
		/// Inicjalizuje wiązanie.
		/// </summary>
		/// <param name="source">Obiekt źródłowy.</param>
		/// <param name="sourceProperty">Źródłowa właściwość.</param>
		/// <param name="target">Obiekt docelowy.</param>
		/// <param name="targetField">Docelowe pole.</param>
		/// <param name="mode">Tryb wiązania.</param>
		/// <exception cref="ArgumentNullException">Któryś z argumentów jest nullem.</exception>
		public Binding(object source, PropertyInfo sourceProperty, object target, FieldInfo targetField, BindingMode mode = BindingMode.OneWay)
			: this(source, (MemberInfo)sourceProperty, target, (MemberInfo)targetField, mode)
		{ }

		/// <summary>
		/// Inicjalizuje wiązanie.
		/// </summary>
		/// <param name="source">Obiekt źródłowy.</param>
		/// <param name="sourceField">Źródłowe pole.</param>
		/// <param name="target">Obiekt docelowy.</param>
		/// <param name="targetField">Docelowe pole.</param>
		/// <param name="mode">Tryb wiązania.</param>
		/// <exception cref="ArgumentNullException">Któryś z argumentów jest nullem.</exception>
		public Binding(object source, FieldInfo sourceField, object target, FieldInfo targetField, BindingMode mode = BindingMode.OneWay)
			: this(source, (MemberInfo)sourceField, target, (MemberInfo)targetField, mode)
		{ }

		/// <summary>
		/// Inicjalizuje wiązanie.
		/// </summary>
		/// <param name="source">Obiekt źródłowy.</param>
		/// <param name="sourcePropertyName">Nazwa źródłowego pola/właściwości.</param>
		/// <param name="target">Obiekt docelowy.</param>
		/// <param name="targetPropertyName">Nazwa docelowego pola/właściwości.</param>
		/// <param name="mode">Tryb wiązania.</param>
		/// <exception cref="ArgumentNullException">source lub target jest nullem lub sourcePropertyName lub targetPropertyName jest puste.</exception>
		/// <exception cref="InvalidOperationException">Nie można znaleźć odpowiednich pól/właściwości w obiektach.</exception>
		public Binding(object source, string sourcePropertyName, object target, string targetPropertyName, BindingMode mode = BindingMode.OneWay)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (string.IsNullOrWhiteSpace(sourcePropertyName))
			{
				throw new ArgumentNullException("sourceProperty");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (string.IsNullOrWhiteSpace(targetPropertyName))
			{
				throw new ArgumentNullException("targetProperty");
			}
			MemberInfo targetProperty = target.GetType().GetProperty(targetPropertyName);
			if (targetProperty == null)
			{
				targetProperty = target.GetType().GetField(targetPropertyName);
				if (targetProperty == null)
				{
					throw new InvalidOperationException(string.Format("Cannot find property {0} in target object", targetPropertyName));
				}
			}
			MemberInfo sourceProperty = source.GetType().GetProperty(sourcePropertyName);
			if (sourceProperty == null)
			{
				sourceProperty = source.GetType().GetField(sourcePropertyName);
				if (sourceProperty == null)
				{
					throw new InvalidOperationException(string.Format("Cannot find property {0} in source object", sourcePropertyName));
				}
			}
			this.Source = source;
			this.SourceProperty = sourceProperty;
			this.Target = target;
			this.TargetProperty = targetProperty;
			this.Mode = mode;
			this.SetBindings();
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
			if (e.PropertyName == this.PropertyName)
			{
				if (this.ControlFlow)
				{
					this.ControlFlow = false;
					return;
				}

				if (this.TargetProperty is PropertyInfo)
				{
					(this.TargetProperty as PropertyInfo).SetValue(this.Target,
						this.GetConvertedSourceToTarget()
						, null);
				}
				else
				{
					(this.TargetProperty as FieldInfo).SetValue(this.Target,
						this.GetConvertedSourceToTarget());
				}
			}
		}

		/// <summary>
		/// Cel do źródła.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TargetToSource(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == this.PropertyName)
			{
				this.ControlFlow = true;

				if (this.SourceProperty is PropertyInfo)
				{
					(this.SourceProperty as PropertyInfo).SetValue(this.Source,
						this.GetConvertedTargetToSource(), null);
				}
				else
				{
					(this.SourceProperty as FieldInfo).SetValue(this.Source,
						this.GetConvertedTargetToSource());
				}
			}
		}

		/// <summary>
		/// Pobiera obiekt ze źródła i, jeśli może, konwertuje go do typu docelowego.
		/// </summary>
		/// <returns></returns>
		private object GetConvertedSourceToTarget()
		{
			object obj = null;
			if (this.SourceProperty is PropertyInfo)
			{
				obj = (this.SourceProperty as PropertyInfo).GetValue(this.Source, null);
			}
			else
			{
				obj = (this.SourceProperty as FieldInfo).GetValue(this.Source);
			}

			if (obj != null && this.SourceConverter.CanConvertTo(this.TargetType))
			{
				obj = this.SourceConverter.ConvertTo(obj, this.TargetType);
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
		private object GetConvertedTargetToSource()
		{
			object obj = null;
			if (this.TargetProperty is PropertyInfo)
			{
				obj = (this.TargetProperty as PropertyInfo).GetValue(this.Source, null);
			}
			else
			{
				obj = (this.TargetProperty as FieldInfo).GetValue(this.Source);
			}

			if (obj != null && this.TargetConverter.CanConvertTo(this.SourceType))
			{
				obj = this.TargetConverter.ConvertTo(obj, this.SourceType);
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
			this.PropertyName = this.SourceProperty.Name;

			#region Converters
			if (this.ConverterType != null)
			{
				this.SourceConverter = this.TargetConverter = Activator.CreateInstance(this.ConverterType) as TypeConverter;
			}
			else
			{
				this.SourceConverter = Converters.Utilities.GetTypeConverter(this.SourceProperty);
				this.TargetConverter = Converters.Utilities.GetTypeConverter(this.TargetProperty);
			}
			#endregion

			#region Bindings
			//Metoda lokalna, by nie dublować kodu ;)
			Action bindSource = () =>
			{
				if (!(this.Source is INotifyPropertyChanged))
				{
					throw new InvalidOperationException("Source must implement INotifyPropertyChanged for this mode");
				}
				(this.Source as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(SourceToTarget);
			};

			//Zależnie od trybu wiązania dodajemy odpowiednie zdarzenia.
			switch (this.Mode)
			{
			case BindingMode.OneWay:
				bindSource();
				break;
			case BindingMode.TwoWay:
				bindSource();
				if (!(this.Target is INotifyPropertyChanged))
				{
					throw new InvalidOperationException("Target must implement INotifyPropertyChanged for this mode");
				}
				(this.Target as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(TargetToSource);
				break;
			}
			#endregion
		}
		#endregion
	}
}
