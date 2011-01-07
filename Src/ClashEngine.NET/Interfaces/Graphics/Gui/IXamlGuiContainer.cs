namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Kontener XAML.
	/// </summary>
	public interface IXamlGuiContainer
		: IResource
	{
		/// <summary>
		/// Kontrolki.
		/// </summary>
		IControlsCollection Controls { get; }

		/// <summary>
		/// Warunki do stylizacji GUI.
		/// </summary>
		IConditionsCollection Triggers { get; }

		/// <summary>
		/// Przypisuje kontener do wskazanego kontenera.
		/// </summary>
		/// <param name="container">Kontener.</param>
		void Bind(IContainer container);
	}
}
