using System;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Messages
{
	using NET.Interfaces;

	/// <summary>
	/// <see cref="GameMessageType.UnitQueued"/>
	/// </summary>
	public struct UnitQueued
	{
		#region Properties
		/// <summary>
		/// Czy serwer zakceptował dodanie jednostki do kolejki.
		/// </summary>
		public bool Accepted;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		public UnitQueued(bool accepted)
		{
			this.Accepted = accepted;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public UnitQueued(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.UnitQueued)
			{
				throw new InvalidCastException("Cannot convert this message to UnitQueued");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.Accepted = s.GetBool();
		}
		#endregion

		#region ToMessage
		/// <summary>
		/// Zwraca wyspecjalizowaną wiadomość jako <see cref="Message"/>.
		/// </summary>
		/// <returns></returns>
		public Message ToMessage()
		{
			byte[] data = new byte[1];
			BinarySerializer.StaticSerialize(data, this.Accepted);
			return new Message((MessageType)GameMessageType.UnitQueued, data);
		}
		#endregion
	}
}
