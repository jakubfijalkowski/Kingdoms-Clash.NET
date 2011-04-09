using System;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Server.Messages
{
	using NET.Interfaces;

	/// <summary>
	/// <see cref="GameMessageType.PlayerConnected"/>
	/// </summary>
	public struct PlayerConnected
	{
		#region Properties
		/// <summary>
		/// Identyfikator użytkownika.
		/// </summary>
		public uint UserId;

		/// <summary>
		/// Nick.
		/// </summary>
		public string Nick;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		/// <param name="uid">Identyfikator nowego użytkownika.</param>
		/// <param name="nick">Nick.</param>
		public PlayerConnected(uint uid, string nick)
		{
			this.UserId = uid;
			this.Nick = nick;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public PlayerConnected(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.PlayerConnected)
			{
				throw new InvalidCastException("Cannot convert this message to ClientConnected");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.UserId = s.GetUInt32();
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
			byte[] data = new byte[6 + this.Nick.Length * 2];
			BinarySerializer.StaticSerialize(data, this.UserId, this.Nick);
			return new Message((MessageType)GameMessageType.PlayerConnected, data);
		}
		#endregion
	}
}
