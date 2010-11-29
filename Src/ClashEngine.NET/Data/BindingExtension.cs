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
			var targetProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
			if (targetProvider == null || targetProvider.TargetObject == null || targetProvider.TargetProperty == null)
			{
				throw new InvalidOperationException("IProvideValueTarget");
			}

			if (this.Source == null)
			{
				this.Source = targetProvider.TargetObject;
			}
			else if (this.Source is string)
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

			this.Target = targetProvider.TargetObject;
			this.TargetPath = new OneLevelPropertyPath(targetProvider.TargetProperty as PropertyInfo);

			this.Binding = new Binding(this.Source, this.SourcePath, this.Target, this.TargetPath, false, this.Mode, this.ConverterType);

			return this.SourcePath.Value;
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			this.Binding.Dispose();
		}
		#endregion
	}
}
