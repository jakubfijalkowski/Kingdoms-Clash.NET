using ClashEngine.NET.Interfaces;

namespace Kingdoms_Clash.NET.Interfaces.Player
{
	/// <summary>
	/// Kontroler gracza.
	/// </summary>
	public interface IPlayerController
	{
		/// <summary>
		/// Gracz, którym kontroler steruje.
		/// </summary>
		IPlayer Player { get; set; }

		/// <summary>
		/// Stan gry do której należy gracz.
		/// </summary>
		IGameState GameState { get; set; }

		/// <summary>
		/// Inicjalizuje kontroler.
		/// Wywoływane przez IGameState.
		/// </summary>
		/// <param name="screens">Manager ekranów, do którego może się kontroler podpiąć.</param>
		/// <param name="input">Wejście, do którego może się kontroler podpiąć.</param>
		void Initialize(IScreensManager screens, IInput input);
	}
}
