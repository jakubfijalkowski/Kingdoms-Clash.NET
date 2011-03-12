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
		#region Main settings
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
		#endregion

		//#region Gameplay settings
		///// <summary>
		///// Czas pomiędzy poszczególnymi odnowieniami zasobów.
		///// </summary>
		//float ResourceRenewalTime { get; }

		///// <summary>
		///// Wartość zasobu.
		///// </summary>
		//uint ResourceRenewalValue { get; }

		///// <summary>
		///// Ilość zasobów na start.
		///// </summary>
		//uint StartResources { get; }
		//#endregion
	}
}
