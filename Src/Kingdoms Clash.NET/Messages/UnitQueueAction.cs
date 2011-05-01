using System;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Messages
{
	using NET.Interfaces;

	/// <summary>
	/// <see cref="GameMessageType.UnitQueueAction"/>
	/// </summary>
	public struct UnitQueueAction
	{
		#region Properties
		/// <summary>
		/// Identyfikator jednostki.
		/// </summary>
		public string UnitId;

		/// <summary>
		/// Czy jednostka została stowrzona, czy usunięta.
		/// </summary>
		public bool Created;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		public UnitQueueAction(string unitId, bool created)
		{
			this.UnitId = unitId;
			this.Created = created;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public UnitQueueAction(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.UnitQueueAction)
			{
				throw new InvalidCastException("Cannot convert this message to CreateUnit");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.UnitId = s.GetString();
			this.Created = s.GetBool();
		}
		#endregion

		#region ToMessage
		/// <summary>
		/// Zwraca wyspecjalizowaną wiadomość jako <see cref="Message"/>.
		/// </summary>
		/// <returns></returns>
		public Message ToMessage()
		{
			byte[] data = new byte[2 + 2 * this.UnitId.Length + 1];
			BinarySerializer.StaticSerialize(data, this.UnitId, this.Created);
			return new Message((MessageType)GameMessageType.UnitQueueAction, data);
		}
		#endregion
	}
}
