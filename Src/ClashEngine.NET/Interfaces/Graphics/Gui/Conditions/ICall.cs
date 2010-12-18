namespace ClashEngine.NET.Interfaces.Graphics.Gui.Conditions
{
	/// <summary>
	/// Wyzwalacz wywołujący metodę.
	/// </summary>
	public interface ICall
		: ITrigger
	{
		/// <summary>
		/// Obiekt na którym będziemy wywoływać metodę.
		/// </summary>
		object Object { get; set; }

		/// <summary>
		/// Nazwa metody.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Parametry wywołania metody.
		/// </summary>
		IParametersCollection Parameters { get; }
	}
}
