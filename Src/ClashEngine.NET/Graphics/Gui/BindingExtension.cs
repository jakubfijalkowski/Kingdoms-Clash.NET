using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;

namespace ClashEngine.NET.Graphics.Gui
{
	using System.Xaml;
	using Interfaces.Data;
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Binduje wartość wewnątrz kontenera Xaml.
	/// </summary>
	[MarkupExtensionReturnType(typeof(object))]
	public class BindingExtension
		: MarkupExtension, IBindingExtension
	{
		#region Private fields
		private TypeConverter SourceConverter = null;
		private TypeConverter TargetConverter = null;
		private Type TargetType = null;
		private Type SourceType = null;
		private bool ControlFlow = false;
		#endregion

		#region IBindingExtension Members
		/// <summary>
		/// Tryb bindowania.
		/// Po utworzeniu bindingu zmiana wartości nie odniesie skutku.
		/// </summary>
		public BindingMode Mode { get; set; }

		/// <summary>
		/// Ścieżka do elementu docelowego.
		/// Obsługiwany format: idKontrolki.właściwość/pole.
		/// Po utworzeniu bindingu zmiana wartości nie odniesie skutku.
		/// </summary>
		public string Path { get; set; }
		#endregion

		#region IBinding Members
		/// <summary>
		/// Obiekt źródłowy.
		/// </summary>
		public object Source { get; set; }

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

		#region MarkupExtension Members
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			#region Providers
			//var rootProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
			//if (rootProvider == null)
			//{
			//    throw new InvalidOperationException("IRootObjectProvider");
			//}
			//var rootObject = rootProvider.RootObject as IXamlGuiContainer;
			//if (rootObject == null)
			//{
			//    throw new InvalidOperationException("RootObject");
			//}

			//Pobieramy właściwość, do której mamy przypisywać wartości.
			var targetProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
			if (targetProvider == null)
			{
				throw new InvalidOperationException("IProvideValueTarget");
			}

			var nameResolver = serviceProvider.GetService(typeof(IXamlNameResolver)) as IXamlNameResolver;
			if (nameResolver == null)
			{
				throw new InvalidOperationException("IXamlNameResolver");
			}
			#endregion

			#region Source
			string[] parts = this.Path.Split('.');
			string propName= null;
			if (parts.Length == 2)
			{
				string sourceName = parts[0].Trim();
				this.Source = nameResolver.Resolve(sourceName);
				if (this.Source == null)
				{
					if (nameResolver.IsFixupTokenAvailable)
					{
						return nameResolver.GetFixupToken(new string[] { sourceName });
					}
					else
					{
						throw new InvalidOperationException("Cannot find Source");
					}
				}
				propName = parts[1].Trim();
			}
			else if (this.Source != null && parts.Length == 1) //To jest akceptowalne
			{
				propName = parts[0].Trim();
			}
			else
			{
				throw new InvalidOperationException("Invalid Path format");
			}
			this.SourceType = this.Source.GetType();

			this.SourceProperty = this.SourceType.GetProperty(propName);
			if (this.SourceProperty == null)
			{
				this.SourceProperty = this.SourceType.GetField(propName);
				if (this.SourceProperty == null)
				{
					throw new InvalidOperationException(string.Format("Cannot find property '{0}' in source object", propName));
				}
			}
			#endregion

			#region Target
			this.Target = targetProvider.TargetObject;
			this.TargetProperty = targetProvider.TargetProperty as PropertyInfo;
			this.TargetType = (this.TargetProperty as PropertyInfo).PropertyType;
			#endregion

			#region Converters
			if (this.ConverterType != null)
			{
				this.SourceConverter = this.TargetConverter = Activator.CreateInstance(this.ConverterType) as TypeConverter;
			}
			else
			{
				var src = Converters.Utilities.GetTypeConverterFor(this.SourceProperty);
				if (src != null)
				{
					this.SourceConverter = Activator.CreateInstance(src) as TypeConverter;
				}
				var target = Converters.Utilities.GetTypeConverterFor(this.TargetProperty);
				if (target != null)
				{
					this.TargetConverter = Activator.CreateInstance(target) as TypeConverter;
				}
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

			//Dla wszystkich trybów musimy zwrócić aktualną wartość.
			if (this.SourceProperty is PropertyInfo)
			{
				return (this.SourceProperty as PropertyInfo).GetValue(this.Source, null);
			}
			return (this.SourceProperty as FieldInfo).GetValue(this.Source);
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje nowe wiązanie.
		/// </summary>
		public BindingExtension()
		{
			this.Mode = BindingMode.OneWay;
		}

		/// <summary>
		/// Inicjalizuje nowe wiązanie.
		/// </summary>
		/// <param name="path">Ścieżka do elementu.</param>
		public BindingExtension(string path)
		{
			this.Mode = BindingMode.OneWay;
			this.Path = path;
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
			if (this.ControlFlow)
			{
				this.ControlFlow = false;
				return;
			}

			if (this.SourceProperty is PropertyInfo)
			{
				(this.TargetProperty as PropertyInfo).SetValue(this.Target,
					this.GetConvertedSourceToTarget()
					, null);
			}
			else
			{
				(this.TargetProperty as PropertyInfo).SetValue(this.Target,
					this.GetConvertedSourceToTarget()
					, null);
			}
		}

		/// <summary>
		/// Cel do źródła.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TargetToSource(object sender, PropertyChangedEventArgs e)
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

			if (obj != null && this.SourceConverter != null && this.SourceConverter.CanConvertTo(this.TargetType))
			{
				obj = this.SourceConverter.ConvertTo(obj, this.TargetType);
			}
			else if (obj != null && this.TargetConverter != null && this.TargetConverter.CanConvertFrom(obj.GetType()))
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
			object obj = (this.TargetProperty as PropertyInfo).GetValue(this.Target, null);

			if (obj != null && this.TargetConverter != null && this.TargetConverter.CanConvertTo(this.SourceType))
			{
				obj = this.TargetConverter.ConvertTo(obj, this.SourceType);
			}
			else if (obj != null && this.SourceConverter != null && this.SourceConverter.CanConvertFrom(obj.GetType()))
			{
				obj = this.SourceConverter.ConvertFrom(obj);
			}
			return obj;
		}
		#endregion
	}
}
