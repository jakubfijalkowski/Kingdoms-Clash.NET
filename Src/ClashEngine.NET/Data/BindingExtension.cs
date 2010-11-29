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
	/// Gdy Source jest nullem i obiekt-rodzic dziedziczy z IDataContex to za source podstawiany jest kontekst.
	/// Gdy Source jest ciągiem znaków traktowany jest jak nazwa XAML i rozwiązywany jest za pomocą IXamlNameResolver lub,
	/// gdy jest równy "self" - do Source przypisywany jest obiekt-rodzic. 
	/// Gdy Source jest pusty i kontekst danych jest nullem - czekamy na ustawienie kontekstu.
	/// </remarks>
	[MarkupExtensionReturnType(typeof(object))]
	public class BindingExtension
		: MarkupExtension, IBindingExtension
	{
		#region Private fields
		private Binding Binding = null;
		#endregion

		#region IBindingExtension Members
		/// <summary>
		/// Tryb bindowania.
		/// </summary>
		public BindingMode Mode { get; set; }

		/// <summary>
		/// Ścieżka do elementu docelowego.
		/// </summary>
		/// <remarks>
		/// Zachowanie po zmianie wartości, gdy całość jest już zainicjalizowana, nie odniesie skutku.
		/// </remarks>
		[TypeConverter(typeof(Converters.PropertyPathConverter))]
		public IPropertyPath Path
		{
			get { return this.SourcePath; }
			set { this.SourcePath = value; }
		}
		#endregion

		#region IBinding Members
		/// <summary>
		/// Obiekt źródłowy.
		/// </summary>
		// Nie używamy konwertera NameReferenceConverter, gdyż odwołanie do rodzica spowoduje zależność cykliczną
		// używanie IXamlNameResolver rozwiązuje ten problem
		public object Source { get; set; }

		/// <summary>
		/// Źródłowa właściwość, z której będą pobierane wartości.
		/// </summary>
		public IPropertyPath SourcePath { get; private set; }

		/// <summary>
		/// Obiekt docelowy.
		/// </summary>
		public object Target { get; private set; }

		/// <summary>
		/// Docelowa właściwość, z której będą pobierane wartości.
		/// </summary>
		public IPropertyPath TargetPath { get; private set; }

		/// <summary>
		/// Typ konwertera użytego do konwersji pomiędzy końcami wiązania.
		/// </summary>
		public Type ConverterType { get; set; }
		#endregion

		#region MarkupExtension Members
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			#region Providers
			var targetProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
			if (targetProvider == null || targetProvider.TargetObject == null || targetProvider.TargetProperty == null)
			{
				throw new InvalidOperationException("IProvideValueTarget");
			}
			#endregion

			#region Target
			this.Target = targetProvider.TargetObject;
			this.TargetPath = new OneLevelPropertyPath(targetProvider.TargetProperty as PropertyInfo);
			#endregion

			#region Source
			if (this.Source == null && (this.Target is IDataContext))
			{
				(this.Target as IDataContext).PropertyChanged += this.DataContextChanged;
				this.Source = (this.Target as IDataContext).DataContext;
				if (this.Source == null)
					return null;
			}
			else if (this.Source == null)
			{
				this.Source = targetProvider.TargetObject;
			}
			else if (this.Source is string)
			{
				if ((this.Source as string).ToLower() == "self")
				{
					this.Source = this.Target;
				}
				else
				{
					var nameResolver = serviceProvider.GetService(typeof(IXamlNameResolver)) as IXamlNameResolver;
					if (nameResolver == null)
					{
						throw new InvalidOperationException("IXamlNameResolver");
					}
					bool fulliInit = false;
					object source = nameResolver.Resolve(this.Source as string, out fulliInit);
					if (source == null)
					{
						if (nameResolver.IsFixupTokenAvailable)
						{
							return nameResolver.GetFixupToken(new string[] { this.Source as string });
						}
						else
						{
							throw new InvalidOperationException("Cannot find Source");
						}
					}
					this.Source = source;
				}
			}
			#endregion

			this.Binding = new Binding(this.Source, this.SourcePath, this.Target, this.TargetPath, false, this.Mode, this.ConverterType);

			return this.SourcePath.Value;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje puste rozszerzenie XAML.
		/// </summary>
		public BindingExtension()
		{ }

		/// <summary>
		/// Inicjalizuje rozszerzenie XAML.
		/// </summary>
		/// <param name="path">Ścieżka do źródła.</param>
		public BindingExtension(string path)
		{
			this.Path = new PropertyPath(path);
		}
		#endregion

		#region Private members
		private void DataContextChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "DataContext")
			{
				var newDataContext = (this.Target as IDataContext).DataContext;
				if (this.Source == null) //Nowy kontekst
				{
					if (newDataContext != null)
					{
						this.Source = newDataContext;
						this.Binding = new Binding(this.Source, this.SourcePath, this.Target, this.TargetPath, true, this.Mode, this.ConverterType);
					}
				}
				else //Aktualizacja
				{
					if (newDataContext != null && newDataContext.GetType() != this.Binding.SourcePath.RootType)
					{
						throw new InvalidOperationException("Cannot change context type after");
					}
					this.Binding.Source = this.Source = newDataContext;
				}
			}
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			if (this.Target is INotifyPropertyChanged)
			{
				(this.Target as IDataContext).PropertyChanged -= this.DataContextChanged;
			}
			this.Binding.Dispose();
		}
		#endregion
	}
}
