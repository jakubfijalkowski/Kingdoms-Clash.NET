namespace Kingdoms_Clash.NET.Interfaces.Player
{
	/// <summary>
	/// Dane o użytkowniku.
	/// Są przypisywane do IClient.UserData.
	/// </summary>
	public interface IPlayerData
	{
		/// <summary>
		/// Identyfikator numeryczny, przypisywany przez serwer.
		/// </summary>
		uint UserId { get; }

		/// <summary>
		/// Nick użytkownika.
		/// </summary>
		string Nick { get; set; }

		/// <summary>
		/// Czy jest w grze.
		/// </summary>
		bool InGame { get; set; }

		/// <summary>
		/// Obiekt gracza, jeśli gracz gra.
		/// </summary>
		IPlayer Player { get; set; }
	}
}
