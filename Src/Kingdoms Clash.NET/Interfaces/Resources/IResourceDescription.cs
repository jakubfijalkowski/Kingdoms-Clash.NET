namespace Kingdoms_Clash.NET.Interfaces.Resources
{
	/// <summary>
	/// Opis zasobu.
	/// Używane tylko do opisywania zasobów np. w GUI, nie używane w grze.
	/// </summary>
	public interface IResourceDescription
	{
		/// <summary>
		/// Identyfikator zasobu.
		/// </summary>
		string Id { get; }

		/// <summary>
		/// Dłuższy opis.
		/// </summary>
		string Description { get; }
	}
}
