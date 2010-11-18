namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	using Data;

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
		/// Ścieżka do elementu docelowego.
		/// </summary>
		string Path { get; set; }
	}
}
