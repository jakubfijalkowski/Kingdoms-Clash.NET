using System.Diagnostics;

namespace Kingdoms_Clash.NET.Player
{
	using NET.Interfaces.Player;

	/// <summary>
	/// Dane o użytkowniku.
	/// Są przypisywane do IClient.UserData.
	/// </summary>
	[DebuggerDisplay("({UserId}) {Nick,nq}{InGame ? \", in game\" : \"\" ,nq}")]
	public class PlayerData
		: IPlayerData
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

		/// <summary>
		/// Czy gracz jest gotowy do gry.
		/// </summary>
		public bool ReadyToPlay { get; set; }
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
