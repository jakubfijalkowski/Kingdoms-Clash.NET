using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;
using System.Xaml;

namespace ClashEngine.NET.Data
{
	using Interfaces.Data;

	/// <summary>
	/// Binduje wartość wewnątrz kontenera Xaml.
	/// </summary>
	/// <remarks>
	/// By stworzyć wiązania ręcznie należy użyć metod statycznych <see cref="Create"/>.
	/// </remarks>
	[MarkupExtensionReturnType(typeof(object))]
	public class BindingExtension
		: MarkupExtension, IBindingExtension
	{
		#region Private fields
		private Binding Binding = null;
		private string SourcePropertyName = string.Empty;
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
		[TypeConverter(typeof(NameReferenceConverter))]
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

		/// <summary>
		/// Usuwa binding.
		/// </summary>
		public void Clear()
		{
			this.Binding.Clear();
		}
		#endregion

		#region MarkupExtension Members
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			#region Providers
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

			#region Target
			this.Target = targetProvider.TargetObject;
			this.TargetProperty = targetProvider.TargetProperty as PropertyInfo;
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
			else if (this.Target is IDataContext && parts.Length == 1)
			{
				if (this.Target is INotifyPropertyChanged)
				{
					(this.Target as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(DataContextChanged);
				}
				this.Source = (this.Target as IDataContext).DataContext;
				if (this.Source == null) //Jeszcze nie przypisano kontekstu - musimy wesprzeć późną inicjalizację
				{
					this.SourcePropertyName = parts[0].Trim();
					return null;
				}
				propName = parts[0].Trim();
			}
			else if (this.Source != null && parts.Length == 1) //To jest akceptowalne
			{
				propName = parts[0].Trim();
			}
			else
			{
				throw new InvalidOperationException("Invalid Path format");
			}
			var sourceType = this.Source.GetType();

			this.SourceProperty = sourceType.GetProperty(propName);
			if (this.SourceProperty == null)
			{
				this.SourceProperty = sourceType.GetField(propName);
				if (this.SourceProperty == null)
				{
					throw new InvalidOperationException(string.Format("Cannot find property '{0}' in source object", propName));
				}
			}
			#endregion

			this.Binding = new Binding(this.Source,	this.SourceProperty, this.Target, this.TargetProperty, false, this.Mode);

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
		/// Do użytku tylko przez parser XAML.
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
		void DataContextChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "DataContext" && (this.Target as IDataContext).DataContext != null)
			{
				if (this.Source == null)
				{
					this.Source = (this.Target as IDataContext).DataContext;
					this.SourceProperty = this.Source.GetType().GetProperty(this.SourcePropertyName);
					if (this.SourceProperty == null)
					{
						this.SourceProperty = this.Source.GetType().GetField(this.SourcePropertyName);
						if (this.SourceProperty == null)
						{
							throw new InvalidOperationException(string.Format("Cannot find property '{0}' in source object", this.SourcePropertyName));
						}
					}
					this.Binding = new Binding(this.Source, this.SourceProperty, this.Target, this.TargetProperty, true, this.Mode);
				}
				else
				{
					if (this.Source.GetType() != (this.Target as IDataContext).DataContext.GetType())
					{
						throw new InvalidOperationException("Changing DataContext to other type is not supported");
					}
					this.Source = this.Binding.Source = (this.Target as IDataContext).DataContext;
					this.Binding.ForceUpdateTarget();
				}
			}
		}
		#endregion
	}
}
