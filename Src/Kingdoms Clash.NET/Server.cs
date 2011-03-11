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

			var key = Console.ReadKey();
			do
			{
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

				else if (key.Key == ConsoleKey.T)
				{
					Console.WriteLine("Client:");
					Console.WriteLine("\trenote: {0}:{1}", client.RemoteEndpoint.Address, client.RemoteEndpoint.Port);
					Console.WriteLine("\tlocal: {0}:{1}", client.LocalEndpoint.Address, client.LocalEndpoint.Port);
					Console.WriteLine("Server's client:");
					Console.WriteLine("\tlocal: {0}:{1}", server.Clients[0].LocalEndpoint.Address, server.Clients[0].LocalEndpoint.Port);
					Console.WriteLine("\tremote: {0}:{1}", server.Clients[0].RemoteEndpoint.Address, server.Clients[0].RemoteEndpoint.Port);
				}
				key = Console.ReadKey();
				Console.WriteLine();
			} while (key.Key != ConsoleKey.Q);
			//client.Close();
			server.Stop();

			Console.ReadLine();
		}
	}
}
#endif