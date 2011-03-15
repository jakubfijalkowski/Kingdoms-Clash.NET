namespace Kingdoms_Clash.NET.Interfaces
{
	/// <summary>
	/// Ustawienia rozgrywki.
	/// </summary>
	public interface IGameplaySettings
	{
		/// <summary>
		/// Czas pomiędzy poszczególnymi odnowieniami zasobów.
		/// </summary>
		float ResourceRenewalTime { get; }

		/// <summary>
		/// Wartość zasobu.
		/// </summary>
		uint ResourceRenewalValue { get; }

		/// <summary>
		/// Ilość zasobów na start.
		/// </summary>
		uint StartResources { get; }
	}
}
