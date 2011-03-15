namespace Kingdoms_Clash.NET.Interfaces
{
	using Controllers;
	using Map;
	using Player;

	/// <summary>
	/// Ustawienia gry.
	/// </summary>
	public interface IGameSettings
	{
		/// <summary>
		/// Informacje o graczu A.
		/// </summary>
		IPlayerInfo PlayerA { get; set; }

		/// <summary>
		/// Informacje o graczu B.
		/// </summary>
		IPlayerInfo PlayerB { get; set; }

		/// <summary>
		/// Informacje o mapie.
		/// </summary>
		IMap Map { get; set; }

		/// <summary>
		/// Kontroler gry.
		/// </summary>
		IGameController Controller { get; set; }

		/// <summary>
		/// Ustawienia rozgrywki, są one zależne od kontrolera, który grę obsługuje.
		/// </summary>
		IGameplaySettings GameplaySettings { get; set; }
	}
}
