namespace ClashEngine.NET.Interfaces.Graphics.Gui.Conditions
{
	/// <summary>
	/// Interfejs bazowy dla warunków do stylizacji GUI.
	/// </summary>
	public interface ICondition
	{
		/// <summary>
		/// Wyzwalacze wywoływane przy spełnieniu warunku.
		/// </summary>
		ITriggersCollection Triggers { get; }
	}
}
