using System;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;
using OpenTK;

namespace Kingdoms_Clash.NET.Messages
{
	using NET.Interfaces;

	/// <summary>
	/// <see cref="GameMessageType.ResourceAdded"/>
	/// </summary>
	public struct ResourceAdded
	{
		#region Properties
		/// <summary>
		/// Id zasobu.
		/// </summary>
		public string ResourceId;

		/// <summary>
		/// Numeryczny identyfikator tego egzemplarza zasobu.
		/// </summary>
		public uint NumericResourceId;

		/// <summary>
		/// Wartość.
		/// </summary>
		public uint Amount;
		
		/// <summary>
		/// Położenie(oś X).
		/// </summary>
		public float Position;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		public ResourceAdded(string resId, uint numId, uint amount, float pos)
		{
			this.ResourceId = resId;
			this.NumericResourceId = numId;
			this.Amount = amount;
			this.Position = pos;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public ResourceAdded(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.ResourceAdded)
			{
				throw new InvalidCastException("Cannot convert this message to ResourceAdded");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.ResourceId = s.GetString();
			this.NumericResourceId = s.GetUInt32();
			this.Amount = s.GetUInt32();
			this.Position = s.GetFloat();
		}
		#endregion

		#region ToMessage
		/// <summary>
		/// Zwraca wyspecjalizowaną wiadomość jako <see cref="Message"/>.
		/// </summary>
		/// <returns></returns>
		public Message ToMessage()
		{
			byte[] data = new byte[2 + this.ResourceId.Length * 2 + 4 + 4 + 8 + 8];
			BinarySerializer.StaticSerialize(data, this.ResourceId, this.NumericResourceId, this.Amount, this.Position);
			return new Message((MessageType)GameMessageType.ResourceAdded, data);
		}
		#endregion
	}
}
