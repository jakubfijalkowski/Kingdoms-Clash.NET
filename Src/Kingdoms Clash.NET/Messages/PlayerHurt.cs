using System;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Messages
{
	using NET.Interfaces;

	/// <summary>
	/// <see cref="GameMessageType.PlayerHurt"/>
	/// </summary>
	public struct PlayerHurt
	{
		#region Properties
		/// <summary>
		/// Identyfikator gracza, który "oberwał".
		/// </summary>
		public byte PlayerId;

		/// <summary>
		/// Wartość ataku.
		/// </summary>
		public int Value;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		public PlayerHurt(byte playerId, int value)
		{
			this.PlayerId = playerId;
			this.Value = value;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public PlayerHurt(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.PlayerHurt)
			{
				throw new InvalidCastException("Cannot convert this message to PlayerHurt");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.PlayerId = s.GetByte();
			this.Value = s.GetInt32();
		}
		#endregion

		#region ToMessage
		/// <summary>
		/// Zwraca wyspecjalizowaną wiadomość jako <see cref="Message"/>.
		/// </summary>
		/// <returns></returns>
		public Message ToMessage()
		{
			byte[] data = new byte[1 + 4];
			BinarySerializer.StaticSerialize(data, this.PlayerId, this.Value);
			return new Message((MessageType)GameMessageType.PlayerHurt, data);
		}
		#endregion
	}
}
