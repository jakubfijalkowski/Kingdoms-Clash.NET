namespace ClashEngine.NET.Interfaces.EntitiesManager
{
	/// <summary>
	/// Delegat dla zmiany wartości atrybutu.
	/// </summary>
	/// <param name="attribute">Atrybut, który wysyłał zdarzenie.</param>
	public delegate void ValueChangedDelegate(IAttribute attribute);

	/// <summary>
	/// Bazowy interfejs dla atrybutów encji.
	/// </summary>
	public interface IAttribute
	{
		/// <summary>
		/// Identyfikator atrybutu.
		/// </summary>
		string Id { get; }

		/// <summary>
		/// Wartość atrybutu.
		/// </summary>
		object Value { get; set; }

		/// <summary>
		/// Zdarzenie wywoływane przy zmianie wartości atrybutu.
		/// </summary>
		event ValueChangedDelegate ValueChanged;
	}
}
