#if SERVER
using System;
using System.Threading;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Server
{
	/// <summary>
	/// Główna klasa dla serwera.
	/// </summary>
	public class Server
	{
		static void Main(string[] args)
		{
			TcpServer server = new TcpServer(12345, 5, "TestServer", new Version(0, 1, 0, 0));
			server.Start();

			Thread.Sleep(1000);
			TcpClient client = new TcpClient(new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 12345), new Version(0, 1, 0, 0));
			client.Open();
			while (client.Status == ClashEngine.NET.Interfaces.Net.ClientStatus.Welcome)
				client.Prepare();


			var key = Console.ReadKey();
			do
			{
				client.Receive();
				if (key.Key == ConsoleKey.A)
				{
					client.Send(new ClashEngine.NET.Interfaces.Net.Message(ClashEngine.NET.Interfaces.Net.MessageType.UserCommand + 1,
						System.Text.Encoding.Unicode.GetBytes("Cześć!")));
				}
				else if (key.Key == ConsoleKey.S)
				{
					if (server.Clients.Count > 0 && server.Clients[0].Messages.Count > 0)
					{
						var msg = server.Clients[0].Messages[0];
						Console.WriteLine("From client - {0}: {1}", msg.Type, System.Text.Encoding.Unicode.GetString(msg.Data));
						server.Clients[0].Messages.RemoveAt(0);
					}
				}

				else if (key.Key == ConsoleKey.Z)
				{
					if (server.Clients.Count > 0)
					{
						server.Clients[0].Send(new ClashEngine.NET.Interfaces.Net.Message(ClashEngine.NET.Interfaces.Net.MessageType.UserCommand + 1,
							System.Text.Encoding.Unicode.GetBytes("Aye!")));
					}
				}
				else if (key.Key == ConsoleKey.X)
				{
					if (client.Messages.Count > 0)
					{
						var msg = client.Messages[0];
						Console.WriteLine("From server - {0}: {1}", msg.Type, System.Text.Encoding.Unicode.GetString(msg.Data));
						client.Messages.RemoveAt(0);
					}
				}
				key = Console.ReadKey();
				Console.WriteLine();
			} while (key.Key != ConsoleKey.Q);
			server.Stop();

			Console.ReadLine();
		}
	}
}
#endif