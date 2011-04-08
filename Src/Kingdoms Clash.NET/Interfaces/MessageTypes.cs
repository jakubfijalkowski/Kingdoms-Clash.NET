namespace Kingdoms_Clash.NET.Interfaces
{
	/// <summary>
	/// Typy wiadomości dla użytkownika.
	/// </summary>
	public enum MessageTypes
		: ushort
	{
		/// <summary>
		/// Połączenie się użytkownika.
		/// </summary>
		ClientConnected = ClashEngine.NET.Interfaces.Net.MessageType.UserCommand + 1,

		/// <summary>
		/// Rozłączenie użytkownika.
		/// </summary>
		ClientDisconnected,

		/// <summary>
		/// Użytkownik zmienił nick.
		/// </summary>
		ClientChangedNick,
	}
}
