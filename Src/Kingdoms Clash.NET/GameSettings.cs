namespace Kingdoms_Clash.NET
{
	using Interfaces;
	using Interfaces.Controllers;
	using Interfaces.Map;
	using Interfaces.Player;

	/// <summary>
	/// Ustawienia gry.
	/// </summary>
	public class GameSettings
		: IGameSettings
	{
		#region IGameSettings Members
		/// <summary>
		/// Informacje o graczu A.
		/// </summary>
		public IPlayerInfo PlayerA { get; set; }

		/// <summary>
		/// Informacje o graczu B.
		/// </summary>
		public IPlayerInfo PlayerB { get; set; }

		/// <summary>
		/// Informacje o mapie.
		/// </summary>
		public IMap Map { get; set; }

		/// <summary>
		/// Kontroler gry.
		/// </summary>
		public IGameController Controller { get; set; }

		/// <summary>
		/// Ustawienia rozgrywki, są one zależne od kontrolera, który grę obsługuje.
		/// </summary>
		public IGameplaySettings GameplaySettings { get; set; }
		#endregion
	}
}
