namespace ClashEngine.NET.Interfaces.EntitiesManager
{
	/// <summary>
	/// Bazowy interfejs dla silnie typowanych atrybutów encji gry.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IAttribute<T>
		: IAttribute
	{
		new T Value { get; set; }
	}
}
