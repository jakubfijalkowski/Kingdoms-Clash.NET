using System;
using System.Collections.Generic;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Messages
{
	using NET.Interfaces;

	/// <summary>
	/// <see cref="GameMessageType.PlayersFirstConfiguration"/>
	/// </summary>
	public struct PlayersFirstConfiguration
	{
		#region Properties
		/// <summary>
		/// Nick.
		/// </summary>
		public string Nick;

		/// <summary>
		/// Dostęne nacje i ich sumy kontrolne.
		/// </summary>
		public List<KeyValuePair<string, byte[]>> Nations;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		/// <param name="nick">Nick</param>
		public PlayersFirstConfiguration(string nick, List<KeyValuePair<string, byte[]>> nations)
		{
			this.Nick = nick;
			this.Nations = nations;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public PlayersFirstConfiguration(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.PlayersFirstConfiguration)
			{
				throw new InvalidCastException("Cannot convert this message to PlayersFirstConfiguration");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.Nick = s.GetString();

			this.Nations = new List<KeyValuePair<string, byte[]>>();
			ushort nations = s.GetUInt16();
			for (int i = 0; i < nations; i++)
			{
				this.Nations.Add(new KeyValuePair<string, byte[]>(s.GetString(), s.GetByteArray()));
			}
		}
		#endregion

		#region ToMessage
		/// <summary>
		/// Zwraca wyspecjalizowaną wiadomość jako <see cref="Message"/>.
		/// </summary>
		/// <returns></returns>
		public Message ToMessage()
		{
			object[] objs = new object[2 + this.Nations.Count * 2];
			objs[0] = this.Nick;
			objs[1] = (UInt16)this.Nations.Count;

			for (int i = 0, j = 2; i < this.Nations.Count; i++)
			{
				objs[j++] = this.Nations[i].Key;
				objs[j++] = this.Nations[i].Value;
			}

			byte[] data = BinarySerializer.StaticSerialize(objs);
			return new Message((MessageType)GameMessageType.PlayersFirstConfiguration, data);
		}
		#endregion
	}
}
