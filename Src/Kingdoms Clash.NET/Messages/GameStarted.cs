using System;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Messages
{
	using NET.Interfaces;

	/// <summary>
	/// <see cref="GameMessageType.GameStarted"/>
	/// </summary>
	public struct GameStarted
	{
		#region Properties
		/// <summary>
		/// Gracz A.
		/// </summary>
		public uint PlayerA;

		/// <summary>
		/// Gracz B.
		/// </summary>
		public uint PlayerB;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		public GameStarted(uint a, uint b)
		{
			this.PlayerA = a;
			this.PlayerB = b;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public GameStarted(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.GameStarted)
			{
				throw new InvalidCastException("Cannot convert this message to GameStarted");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.PlayerA = s.GetUInt32();
			this.PlayerB = s.GetUInt32();
		}
		#endregion

		#region ToMessage
		/// <summary>
		/// Zwraca wyspecjalizowaną wiadomość jako <see cref="Message"/>.
		/// </summary>
		/// <returns></returns>
		public Message ToMessage()
		{
			byte[] data = new byte[8];
			BinarySerializer.StaticSerialize(data, this.PlayerA, this.PlayerB);
			return new Message((MessageType)GameMessageType.GameStarted, data);
		}
		#endregion
	}
}
