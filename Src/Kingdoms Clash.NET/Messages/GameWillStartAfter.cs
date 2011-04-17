using System;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Messages
{
	using NET.Interfaces;

	/// <summary>
	/// </summary>
	public struct GameWillStartAfter
	{
		#region Properties
		/// <summary>
		/// Czas, za ile się rozpocznie gra.
		/// </summary>
		public TimeSpan Time;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		public GameWillStartAfter(TimeSpan time)
		{
			this.Time = time;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public GameWillStartAfter(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.GameWillStartAfter)
			{
				throw new InvalidCastException("Cannot convert this message to GameWillStartAfter");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.Time = new TimeSpan(s.GetInt64());
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
			BinarySerializer.StaticSerialize(data, this.Time.Ticks);
			return new Message((MessageType)GameMessageType.GameWillStartAfter, data);
		}
		#endregion
	}
}
