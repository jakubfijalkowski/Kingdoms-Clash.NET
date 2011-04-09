using System;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Server.Messages
{
	using NET.Interfaces;

	/// <summary>
	/// <see cref="GameMessageType.PlayerChangedNick"/>
	/// </summary>
	public struct PlayerChangedNick
	{
		#region Properties
		/// <summary>
		/// Identyfikator gracza.
		/// </summary>
		public uint UserId;

		/// <summary>
		/// Nowy nick.
		/// </summary>
		public string NewNick;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		/// <param name="uid">Identyfikator gracza.</param>
		/// <param name="newNick">Nowy nick.</param>
		public PlayerChangedNick(uint uid, string newNick)
		{
			this.UserId = uid;
			this.NewNick = newNick;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public PlayerChangedNick(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.PlayerChangedNick)
			{
				throw new InvalidCastException("Cannot convert this message to PlayerChangedNick");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.UserId = s.GetUInt32();
			this.NewNick = s.GetString();
		}
		#endregion

		#region ToMessage
		/// <summary>
		/// Zwraca wyspecjalizowaną wiadomość jako <see cref="Message"/>.
		/// </summary>
		/// <returns></returns>
		public Message ToMessage()
		{
			byte[] data = new byte[0];
			BinarySerializer.StaticSerialize(data, this.UserId, this.NewNick);
			return new Message((MessageType)GameMessageType.PlayerChangedNick, data);
		}
		#endregion
	}
}
