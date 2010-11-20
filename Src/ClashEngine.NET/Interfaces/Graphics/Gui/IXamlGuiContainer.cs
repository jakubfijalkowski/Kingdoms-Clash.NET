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
		/// Zmienne służące do wymiany informacji pomiędzy kontrolkami wew. XAML lub pomiędzy kontrolkami XAML a światem zewnętrznym.
		/// </summary>
		IVariablesCollection Variables { get; }

		/// <summary>
		/// Przypisuje kontener do wskazanego kontenera.
		/// </summary>
		/// <param name="container">Kontener.</param>
		void Bind(IContainer container);

		/// <summary>
		/// Zapisuje kontener.
		/// </summary>
		/// <param name="output"></param>
		void Save(System.IO.TextWriter output);
	}
}
