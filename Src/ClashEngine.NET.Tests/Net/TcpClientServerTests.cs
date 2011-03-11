using System;
using System.Net;
using System.Net.Sockets;
using NUnit.Framework;

namespace ClashEngine.NET.Tests.Net
{
	using NET.Net;  //Wygląda świetnie :D
	using NET.Interfaces.Net;

	[TestFixture(Description = "Testy dla TcpServer")]
	public class TcpClientServerTests
	{
		private const string Name = "Test";
		private const int Port = 12345;
		private static readonly Version Version = new System.Version(1, 0, 0, 0);

		[Test]
		public void ServerStartsAndStopsWithCorrectInfo()
		{
			using (var server = new TcpServer(Port, 1, Name, Version))
			{
				Assert.AreEqual(server.Name, Name);
				Assert.AreEqual(server.Version, Version);
				Assert.AreEqual(0, server.Clients.Count);
				Assert.IsFalse(server.IsRunning);
				server.Start();
				Assert.IsTrue(server.IsRunning);
				Assert.AreEqual(0, server.Clients.Count);
				server.Stop();
				Assert.IsFalse(server.IsRunning);
			}
		}

		[Test]
		public void ClientCreatesWithCorrectInfo()
		{
			using (var client = new TcpClient(new IPEndPoint(IPAddress.Loopback, Port), Version))
			{
				Assert.AreEqual(Version, client.Version);
				Assert.AreEqual(new IPEndPoint(IPAddress.Loopback, Port), client.RemoteEndpoint);
				Assert.AreEqual(0, client.Messages.Count);
				Assert.AreEqual(ClientStatus.Closed, client.Status);
			}
		}

		[Test]
		public void ClientConnectsToServer()
		{
			using (var server = new TcpServer(Port, 1, Name, Version))
			using (var client = new TcpClient(new IPEndPoint(IPAddress.Loopback, Port), Version))
			{
				server.Start();
				client.Open();
				//while (client.Status == ClientStatus.Welcome)
				//    client.Prepare();
				while (client.Status == ClientStatus.Welcome) ;
				
				Assert.AreEqual(ClientStatus.Ok, client.Status);
				Assert.AreEqual(1, server.Clients.Count);
				Assert.AreEqual(ClientStatus.Ok, server.Clients[0].Status);
				Assert.AreEqual(IPAddress.Loopback, server.Clients[0].RemoteEndpoint.Address);

				Assert.AreEqual(client.RemoteEndpoint, server.Clients[0].LocalEndpoint);
				Assert.AreEqual(client.LocalEndpoint, server.Clients[0].RemoteEndpoint);
			}
		}

		[Test]
		public void ServerIsClosedBeforeClientWillDisconnect()
		{
			using (var server = new TcpServer(Port, 1, Name, Version))
			using (var client = new TcpClient(new IPEndPoint(IPAddress.Loopback, Port), Version))
			{
				server.Start();
				client.Open();
				//while (client.Status == ClientStatus.Welcome)
				//    client.Prepare();

				Assert.AreEqual(ClientStatus.Ok, client.Status);

				server.Stop();
				Assert.AreEqual(0, server.Clients.Count);
				Assert.AreEqual(ClientStatus.Closed, client.Status);
			}
		}
	}
}
