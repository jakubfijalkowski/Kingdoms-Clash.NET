using System;
using System.Collections.Generic;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Messages
{
	using NET.Interfaces;
	using NET.Interfaces.Player;

	/// <summary>
	/// <see cref="GameMessageType.PlayerAccepted"/>
	/// </summary>
	public struct PlayerAccepted
	{
		#region Properties
		/// <summary>
		/// Identyfikator.
		/// </summary>
		public uint UserId;

		/// <summary>
		/// Czy jest w trakcie gry.
		/// </summary>
		public bool InGame;

		/// <summary>
		/// Lista aktualnie podłączonych graczy.
		/// Przesyłane są tylko trzy pola - UserId, Nick i InGame.
		/// </summary>
		public IList<IPlayerData> Players;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		public PlayerAccepted(uint uid, bool inGame)
		{
			this.UserId = uid;
			this.InGame = inGame;
			this.Players = new List<IPlayerData>();
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public PlayerAccepted(Message msg)
		{
			if (msg.Type != (MessageType)GameMessageType.PlayerAccepted)
			{
				throw new InvalidCastException("Cannot convert this message to PlayerAccepted");
			}
			BinarySerializer s = new BinarySerializer(msg.Data);
			this.UserId = s.GetUInt32();
			this.InGame = s.GetBool();
			ushort players = s.GetUInt16();
			this.Players = new List<IPlayerData>();
			for (int i = 0; i < players; i++)
			{
				var player = new Player.PlayerData(s.GetUInt32());
				player.Nick = s.GetString();
				player.InGame = s.GetBool();
				this.Players.Add(player);
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
			object[] objs = new object[3 + 3 * this.Players.Count];
			objs[0] = this.UserId;
			objs[1] = this.InGame;
			objs[2] = (ushort)this.Players.Count;

			int len = 7;
			for (int i = 0, j = 3; i < this.Players.Count; i++)
			{
				objs[j++] = this.Players[i].UserId;
				objs[j++] = this.Players[i].Nick;
				objs[j++] = this.Players[i].InGame;
				len += 7 + this.Players[i].Nick.Length * 2;
			}
			byte[] data = new byte[len];
			BinarySerializer.StaticSerialize(data, objs);
			return new Message((MessageType)GameMessageType.PlayerAccepted, data);
		}
		#endregion
	}
}
