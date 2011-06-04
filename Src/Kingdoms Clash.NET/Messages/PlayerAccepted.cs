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
		public List<IPlayerData> Players;

		/// <summary>
		/// Dostępne nacje.
		/// </summary>
		public List<string> AvailableNations;
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		public PlayerAccepted(uint uid, bool inGame, List<string> availableNations)
		{
			this.UserId = uid;
			this.InGame = inGame;
			this.Players = new List<IPlayerData>();
			this.AvailableNations = availableNations;
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

			ushort nations = s.GetUInt16();
			this.AvailableNations = new List<string>();
			for (int i = 0; i < nations; i++)
			{
				this.AvailableNations.Add(s.GetString());
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
			object[] objs = new object[3 + 3 * this.Players.Count + 1 + this.AvailableNations.Count];
			objs[0] = this.UserId;
			objs[1] = this.InGame;

			objs[2] = (ushort)this.Players.Count;
			for (int i = 0, j = 3; i < this.Players.Count; i++)
			{
				objs[j++] = this.Players[i].UserId;
				objs[j++] = this.Players[i].Nick;
				objs[j++] = this.Players[i].InGame;
			}

			objs[2 + this.Players.Count * 3] = this.AvailableNations.Count;
			for (int i = 0, j = 3 + this.Players.Count * 3; i < this.AvailableNations.Count; i++, j++)
			{
				objs[j] = this.AvailableNations[i];
			}
			byte[] data = BinarySerializer.StaticSerialize(objs);
			return new Message((MessageType)GameMessageType.PlayerAccepted, data);
		}
		#endregion
	}
}
