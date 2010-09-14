namespace Kingdoms_Clash.NET.Interfaces.Resources
{
	/// <summary>
	/// Zasób.
	/// Może być używany jako pojedynczy "pakunek" ale również jako całkowity zasób gracza. 
	/// </summary>
	public interface IResource
	{
		/// <summary>
		/// Identyfikator zasobu - stały dla każdej instancji.
		/// </summary>
		string Id { get; }

		/// <summary>
		/// Wartość.
		/// </summary>
		int Value { get; set; }
	}
}
