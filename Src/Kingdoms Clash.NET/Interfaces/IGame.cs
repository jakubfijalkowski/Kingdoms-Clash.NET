namespace Kingdoms_Clash.NET.Interfaces
{
	/// <summary>
	/// Główny obiekt gry.
	/// </summary>
	public interface IGame
		: ClashEngine.NET.Interfaces.IGame
	{
		/// <summary>
		/// Ekran rozgrywki.
		/// </summary>
		IGameStateScreen Game { get; }

		/// <summary>
		/// Ekran głównego menu.
		/// </summary>
		IMenuState Menu { get; }
	}
}
