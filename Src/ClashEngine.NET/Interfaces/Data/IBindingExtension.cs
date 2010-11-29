using System.ComponentModel;
namespace ClashEngine.NET.Interfaces.Data
{
	/// <summary>
	/// Rozszerzenie XAML wiążące wartości.
	/// </summary>
	public interface IBindingExtension
		: IBinding
	{
		/// <summary>
		/// Tryb bindowania.
		/// </summary>
		new BindingMode Mode { get; set; }

		/// <summary>
		/// Ścieżka do elementu źródłowego.
		/// Alias dla IBinding.SourcePath.
		/// </summary>
		[TypeConverter(typeof(Converters.PropertyPathConverter))]
		IPropertyPath Path { get; set; }
	}
}
