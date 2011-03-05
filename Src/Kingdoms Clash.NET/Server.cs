#if SERVER
using System.Net.Sockets;
using System.Threading;
using ClashEngine.NET.Net;
using System.Net;
using System;

namespace Kingdoms_Clash.NET.Server
{
	/// <summary>
	/// Główna klasa dla serwera.
	/// </summary>
	public class Server
	{
		static void Main(string[] args)
		{
			TcpServer server = new TcpServer(12345, 5, "TestServer", new Version(0, 1));
			server.Start();

			Thread.Sleep(1000);
			TcpClient client = new TcpClient();
			client.Connect(new IPEndPoint(IPAddress.Loopback, 12345));
			var stream = client.GetStream();
			while (!stream.DataAvailable) ;
			byte[] buffer = new byte[2048];
			int i = 0;
			while (stream.DataAvailable)
			{
				i += stream.Read(buffer, i, buffer.Length - i);
			}
			client.Close();
			Array.Resize(ref buffer, i);

			ClashEngine.NET.Net.Messages.ServerWelcome welcome = new ClashEngine.NET.Net.Messages.ServerWelcome(new ClashEngine.NET.Interfaces.Net.Message(buffer));

			while (server.IsRunning)
			{
			}
		}
	}
}
#endif