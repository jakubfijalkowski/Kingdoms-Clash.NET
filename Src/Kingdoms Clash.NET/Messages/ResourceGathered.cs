using System;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Messages
{
	using NET.Interfaces;

	/// <summary>
	/// <see cref="GameMessageType.ResourceGathered"/>
	/// </summary>
	public struct ResourceGathered
	{
		#region Properties
		/// <summary>
		/// Id zasobu.
		/// </summary>
		public uint ResourceId;

		/// <summary>
		/// Gracz, który zebrał.
		/// </summary>
		public byte PlayerId;

		/// <summary>
		/// Identyfikator jednostki, która go zebrała.
		/// </summary>
		public uint UnitId;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		public ResourceGathered(uint resId, byte playerId, uint unitId)
		{
			this.ResourceId = resId;
			this.PlayerId = playerId;
			this.UnitId = unitId;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public ResourceGathered(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.ResourceGathered)
			{
				throw new InvalidCastException("Cannot convert this message to ResourceGathered");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.ResourceId = s.GetUInt32();
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
			byte[] data = new byte[4 + 1 + 4];
			BinarySerializer.StaticSerialize(data, this.ResourceId, this.PlayerId, this.UnitId);
			return new Message((MessageType)GameMessageType.ResourceGathered, data);
		}
		#endregion
	}
}
