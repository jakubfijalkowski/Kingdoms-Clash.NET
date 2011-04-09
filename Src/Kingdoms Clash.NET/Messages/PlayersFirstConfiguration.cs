using System;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Server.Messages
{
	using NET.Interfaces;

	/// <summary>
	/// <see cref="GameMessageType.PlayersFirstConfiguration"/>
	/// </summary>
	public struct PlayersFirstConfiguration
	{
		#region Properties
		/// <summary>
		/// Nick.
		/// </summary>
		public string Nick;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		/// <param name="nick">Nick</param>
		public PlayersFirstConfiguration(string nick)
		{
			this.Nick = nick;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public PlayersFirstConfiguration(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.PlayersFirstConfiguration)
			{
				throw new InvalidCastException("Cannot convert this message to PlayersFirstConfiguration");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.Nick = s.GetString();
		}
		#endregion

		#region ToMessage
		/// <summary>
		/// Zwraca wyspecjalizowaną wiadomość jako <see cref="Message"/>.
		/// </summary>
		/// <returns></returns>
		public Message ToMessage()
		{
			byte[] data = new byte[2 + this.Nick.Length * 2];
			BinarySerializer.StaticSerialize(data, this.Nick);
			return new Message((MessageType)GameMessageType.PlayersFirstConfiguration, data);
		}
		#endregion
	}
}
