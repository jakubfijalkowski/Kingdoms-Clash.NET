using System;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Messages
{
	using NET.Interfaces;

	/// <summary>
	/// <see cref="GameMessageType.PlayerDisconnected"/>
	/// </summary>
	public struct PlayerDisconnected
	{
		#region Properties
		/// <summary>
		/// Identyfikator gracza.
		/// </summary>
		public uint UserId;

		/// <summary>
		/// Powód rozłączenia się gracza.
		/// </summary>
		public DisconnectionReason Reason;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		/// <param name="uid">Identyfikator gracza.</param>
		public PlayerDisconnected(uint uid, DisconnectionReason reason)
		{
			this.UserId = uid;
			this.Reason = reason;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public PlayerDisconnected(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.PlayerDisconnected)
			{
				throw new InvalidCastException("Cannot convert this message to PlayerDisconnected");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.UserId = s.GetUInt32();
			this.Reason = (DisconnectionReason)s.GetByte();
		}
		#endregion

		#region ToMessage
		/// <summary>
		/// Zwraca wyspecjalizowaną wiadomość jako <see cref="Message"/>.
		/// </summary>
		/// <returns></returns>
		public Message ToMessage()
		{
			byte[] data = new byte[5];
			BinarySerializer.StaticSerialize(data, this.UserId, (byte)this.Reason);
			return new Message((MessageType)GameMessageType.PlayerDisconnected, data);
		}
		#endregion
	}
}
