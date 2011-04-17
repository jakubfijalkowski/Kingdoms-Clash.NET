using System;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Messages
{
	using NET.Interfaces;

	/// <summary>
	/// <see cref="GameMessageType.PlayerChangedState"/>
	/// </summary>
	public struct PlayerChangedState
	{
		#region Properties
		/// <summary>
		/// Identyfikator gracza.
		/// </summary>
		public uint UserId;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		/// <param name="uid">Identyfikator gracza.</param>
		/// <param name="newNick">Nowy nick.</param>
		public PlayerChangedState(uint uid)
		{
			this.UserId = uid;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public PlayerChangedState(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.PlayerChangedNick)
			{
				throw new InvalidCastException("Cannot convert this message to PlayerChangedNick");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.UserId = s.GetUInt32();
		}
		#endregion

		#region ToMessage
		/// <summary>
		/// Zwraca wyspecjalizowaną wiadomość jako <see cref="Message"/>.
		/// </summary>
		/// <returns></returns>
		public Message ToMessage()
		{
			byte[] data = new byte[4];
			BinarySerializer.StaticSerialize(data, this.UserId);
			return new Message((MessageType)GameMessageType.PlayerChangedNick, data);
		}
		#endregion
	}
}
