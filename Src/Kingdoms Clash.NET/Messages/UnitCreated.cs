using System;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;
using OpenTK;

namespace Kingdoms_Clash.NET.Messages
{
	using NET.Interfaces;

	/// <summary>
	/// <see cref="GameMessageType.UnitCreated"/>
	/// </summary>
	public struct UnitCreated
	{
		#region Properties
		/// <summary>
		/// Identyfikator gracza.
		/// </summary>
		public byte PlayerId;

		/// <summary>
		/// Identyfikator typu jednostki.
		/// </summary>
		public string UnitId;

		/// <summary>
		/// Identyfikator liczbowy jednostki.
		/// </summary>
		public uint NumericUnitId;

		/// <summary>
		/// Pozycja początkowa.
		/// </summary>
		public Vector2 Position;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		public UnitCreated(byte playerId, string unitId, uint nUnitId, Vector2 pos)
		{
			this.PlayerId = playerId;
			this.UnitId = unitId;
			this.NumericUnitId = nUnitId;
			this.Position = pos;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public UnitCreated(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.UnitCreated)
			{
				throw new InvalidCastException("Cannot convert this message to UnitCreated");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.PlayerId = s.GetByte();
			this.UnitId = s.GetString();
			this.NumericUnitId = s.GetUInt32();
			this.Position = new Vector2(s.GetFloat(), s.GetFloat());
		}
		#endregion

		#region ToMessage
		/// <summary>
		/// Zwraca wyspecjalizowaną wiadomość jako <see cref="Message"/>.
		/// </summary>
		/// <returns></returns>
		public Message ToMessage()
		{
			byte[] data = new byte[1 + 2 + this.UnitId.Length * 2 + 4];
			BinarySerializer.StaticSerialize(data, this.PlayerId, this.UnitId, this.NumericUnitId, (float)this.Position.X, (float)this.Position.Y);
			return new Message((MessageType)GameMessageType.UnitCreated, data);
		}
		#endregion
	}
}
