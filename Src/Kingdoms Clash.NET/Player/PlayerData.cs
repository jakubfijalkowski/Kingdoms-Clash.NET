using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kingdoms_Clash.NET.Server.Player
{
	using NET.Interfaces.Player;
	using Interfaces.Player;

	/// <summary>
	/// Dane o użytkowniku.
	/// Są przypisywane do IClient.UserData.
	/// </summary>
	public class PlayerData
		: Interfaces.Player.IPlayerData
	{
		#region IPlayerData Members
		/// <summary>
		/// Identyfikator numeryczny, przypisywany przez serwer.
		/// </summary>
		public uint UserId { get; private set; }

		/// <summary>
		/// Nick użytkownika.
		/// </summary>
		public string Nick { get; set; }

		/// <summary>
		/// Czy jest w grze.
		/// </summary>
		public bool InGame { get; set; }

		/// <summary>
		/// Obiekt gracza, jeśli gracz gra.
		/// </summary>
		public IPlayer Player { get; set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje dane użytkownika.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		public PlayerData(uint id)
		{
			this.UserId = id;
		}
		#endregion
	}
}
