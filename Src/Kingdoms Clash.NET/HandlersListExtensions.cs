using System;
using System.Collections.Generic;
using ClashEngine.NET.Interfaces.Net;

namespace Kingdoms_Clash.NET
{
	using Interfaces;

	internal static class HandlersListExtensions
	{
		public static bool Call(this Dictionary<GameMessageType, Func<IClient, Message, bool>> dict, IClient client, Message msg)
		{
			Func<IClient, Message, bool> func;
			if (dict.TryGetValue((GameMessageType)msg.Type, out func))
			{
				return func(client, msg);
			}
			return false;
		}

		public static bool Call(this Dictionary<GameMessageType, Func<Message, bool>> dict, Message msg)
		{
			Func<Message, bool> func;
			if (dict.TryGetValue((GameMessageType)msg.Type, out func))
			{
				return func(msg);
			}
			return false;
		}
	}

}
