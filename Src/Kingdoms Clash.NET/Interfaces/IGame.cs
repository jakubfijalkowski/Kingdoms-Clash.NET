using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Interfaces
{
	/// <summary>
	/// Główny obiekt gry.
	/// </summary>
	public interface IGame
	{
		/// <summary>
		/// Ekran rozgrywki.
		/// </summary>
		IGameStateScreen Game { get; }

		/// <summary>
		/// Ekran głównego menu.
		/// </summary>
		IMenuState Menu { get; }

		/// <summary>
		/// Lista nacji.
		/// </summary>
		IList<Units.INation> Nations { get; }
	}
}
