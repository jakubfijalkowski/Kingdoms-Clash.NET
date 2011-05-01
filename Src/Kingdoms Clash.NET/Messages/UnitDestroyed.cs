using System;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Messages
{
	using NET.Interfaces;

	/// <summary>
	/// <see cref="GameMessageType.UnitDestroyed"/>
	/// </summary>
	public struct UnitDestroyed
	{
		#region Properties
		/// <summary>
		/// Identyfikator gracza.
		/// </summary>
		public byte PlayerId;

		/// <summary>
		/// Identyfikator jednostki.
		/// </summary>
		public uint UnitId;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		public UnitDestroyed(byte playerId, uint unitId)
		{
			this.PlayerId = playerId;
			this.UnitId = unitId;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public UnitDestroyed(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.UnitDestroyed)
			{
				throw new InvalidCastException("Cannot convert this message to UnitDestroyed");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.PlayerId = s.GetByte();
			this.UnitId = s.GetUInt32();
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
			BinarySerializer.StaticSerialize(data, this.PlayerId, this.UnitId);
			return new Message((MessageType)GameMessageType.UnitDestroyed, data);
		}
		#endregion
	}
}
