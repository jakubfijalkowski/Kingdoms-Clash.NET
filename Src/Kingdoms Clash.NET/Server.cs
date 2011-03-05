#if SERVER
using System;
using System.Net;
using System.Net.Sockets;
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
			Array.Resize(ref buffer, i);
			ClashEngine.NET.Net.Messages.ServerWelcome welcome = new ClashEngine.NET.Net.Messages.ServerWelcome(new ClashEngine.NET.Interfaces.Net.Message(buffer, 0, i));

			byte[] dataToSend = new byte[] { 0x02, 0x00, 0x00, 0x01, 0x00, 0x00, 0xFF, 0xFF };
			stream.Write(dataToSend, 0, dataToSend.Length);
			//client.Close();

			while (server.IsRunning)
			{
			}
		}
	}
}
#endif