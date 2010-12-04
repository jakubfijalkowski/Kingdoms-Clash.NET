namespace Kingdoms_Clash.NET.Units.XAML
{
	/// <summary>
	/// Opisuje zasób - używane do ładnej integracji z XAML-em.
	/// </summary>
	public interface IResource
	{
		/// <summary>
		/// Nazwa zasobu.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Wartość.
		/// </summary>
		uint Value { get; set; }
	}
}
