namespace Kingdoms_Clash.NET.Interfaces.Controllers
{
	/// <summary>
	/// Kontroler gry klasycznej - zgodnej z założeniami oryginału.
	/// </summary>
	/// <remarks>
	/// Klasa ustawień: <see cref="IClassicGameSettings"/>
	/// </remarks>
	public interface IClassicGame
		: IGameController
	{ }

	/// <summary>
	/// Ustawienia dla kontrolera <see cref="IClassicGame"/>.
	/// </summary>
	public interface IClassicGameSettings
		: IGameplaySettings
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
