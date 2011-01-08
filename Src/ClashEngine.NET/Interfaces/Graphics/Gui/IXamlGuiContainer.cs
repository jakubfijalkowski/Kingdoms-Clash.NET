namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	using Conditions;

	/// <summary>
	/// Kontener XAML.
	/// </summary>
	public interface IXamlGuiContainer
		: IResource, IContainer
	{
		/// <summary>
		/// Warunki do stylizacji GUI.
		/// </summary>
		IConditionsCollection Triggers { get; }
	}
}
