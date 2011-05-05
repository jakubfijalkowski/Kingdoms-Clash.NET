using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kingdoms_Clash.NET.Server.Interfaces
{
	using NET.Interfaces.Player;

	/// <summary>
	/// Ustawienia gry dla serwera.
	/// </summary>
	public interface IServerGameConfiguration
	{
		/// <summary>
		/// Informacje o graczu A.
		/// </summary>
		IPlayerInfo PlayerA { get; }

		/// <summary>
		/// Informacje o graczu B.
		/// </summary>
		IPlayerInfo PlayerB { get; }
	}
}
