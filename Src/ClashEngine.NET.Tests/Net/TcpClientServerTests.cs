using System;
using System.Net;
using System.Net.Sockets;
using NUnit.Framework;

namespace ClashEngine.NET.Tests.Net
{
	using System.Text;
	using NET.Interfaces.Net;
	using NET.Net;  //Wygląda świetnie :D

	[TestFixture(Description = "Testy dla TcpServer")]
	public class TcpClientServerTests
	{
		private const string Name = "Test";
		private const string AdditionalData = "AdditionalData";
		private const int Port = 12345;
		private const int InfoPort = 12346;
		private static readonly Version Version = new System.Version(1, 0, 0, 0);

		[Test]
		public void ServerStartsAndStopsWithCorrectInfoWait()
		{
			using (var server = new TcpServer(Port, 1, Name, Version))
			{
				Assert.AreEqual(server.Name, Name);
				Assert.AreEqual(server.Version, Version);
				Assert.AreEqual(0, server.Clients.Count);
				Assert.AreEqual(ServerState.Stopped, server.State);
				server.Start(true);
				Assert.AreEqual(ServerState.Running, server.State);
				Assert.AreEqual(0, server.Clients.Count);
				server.Stop(true);
				Assert.AreEqual(ServerState.Stopped, server.State);
			}
		}

		[Test]
		public void ServerStartsAndStopsWithCorrectInfoNoWait()
		{
			using (var server = new TcpServer(Port, 1, Name, Version))
			{
				Assert.AreEqual(server.Name, Name);
				Assert.AreEqual(server.Version, Version);
				Assert.AreEqual(0, server.Clients.Count);

				Assert.AreEqual(ServerState.Stopped, server.State);
				server.Start(false);
				Assert.AreEqual(ServerState.Starting, server.State);
				while (server.State == ServerState.Starting) ;
				Assert.AreEqual(ServerState.Running, server.State);

				Assert.AreEqual(0, server.Clients.Count);
				server.Stop(false);
				Assert.AreEqual(ServerState.Running, server.State);
				while (server.State == ServerState.Running) ;
				Assert.AreEqual(ServerState.Stopped, server.State);
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
		public void ClientConnectsAndDisconnectsWait()
		{
			using (var server = new TcpServer(Port, 1, Name, Version))
			using (var client = new TcpClient(new IPEndPoint(IPAddress.Loopback, Port), Version))
			{
				server.Start();
				client.Open(true);

				Assert.AreEqual(ClientStatus.Ok, client.Status);
				Assert.AreEqual(1, server.Clients.Count);
				Assert.AreEqual(ClientStatus.Ok, server.Clients[0].Status);
				Assert.AreEqual(IPAddress.Loopback, server.Clients[0].RemoteEndpoint.Address);

				Assert.AreEqual(client.RemoteEndpoint, server.Clients[0].LocalEndpoint);
				Assert.AreEqual(client.LocalEndpoint, server.Clients[0].RemoteEndpoint);

				client.Close(true);
				Assert.AreEqual(ClientStatus.Closed, client.Status);
			}
		}

		[Test]
		public void ClientConnectsAndDisconnectsNoWait()
		{
			using (var server = new TcpServer(Port, 1, Name, Version))
			using (var client = new TcpClient(new IPEndPoint(IPAddress.Loopback, Port), Version))
			{
				server.Start();
				client.Open(false);
				Assert.AreEqual(ClientStatus.Welcome, client.Status);
				while (client.Status == ClientStatus.Welcome) ;

				Assert.AreEqual(ClientStatus.Ok, client.Status);
				Assert.AreEqual(1, server.Clients.Count);
				Assert.AreEqual(ClientStatus.Ok, server.Clients[0].Status);
				Assert.AreEqual(IPAddress.Loopback, server.Clients[0].RemoteEndpoint.Address);

				Assert.AreEqual(client.RemoteEndpoint, server.Clients[0].LocalEndpoint);
				Assert.AreEqual(client.LocalEndpoint, server.Clients[0].RemoteEndpoint);

				client.Close(false);
				Assert.AreEqual(ClientStatus.Ok, client.Status);
				while (client.Status == ClientStatus.Ok) ;
				Assert.AreEqual(ClientStatus.Closed, client.Status);
			}
		}

		[Test]
		public void ClosingServerDisconnectsClients()
		{
			using (var server = new TcpServer(Port, 1, Name, Version))
			using (var client = new TcpClient(new IPEndPoint(IPAddress.Loopback, Port), Version))
			{
				server.Start();
				client.Open();
				Assert.AreEqual(ClientStatus.Ok, client.Status);

				server.Stop();
				Assert.AreEqual(0, server.Clients.Count);
				Assert.AreEqual(ClientStatus.Closed, client.Status);
			}
		}

		[Test]
		public void ServersInformationChannelSendsCorrectInformation()
		{
			using (var server = new TcpServer(Port, 10, Name, Version, InfoPort))
			{
				server.AdditionalData = AdditionalData;
				server.Start();
				Assert.AreEqual(new IPEndPoint(IPAddress.Any, InfoPort), server.InfoEndpoint);

				Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); //Gniazdo
				try
				{
					sock.Bind(new IPEndPoint(IPAddress.Any, 0));

					byte[] data = new byte[]
					{
						0x02, 0x00, 0xAD, 0xDE, 0xAD, 0xDE
					};
					sock.SendTo(data, new IPEndPoint(IPAddress.Loopback, InfoPort));

					data = new byte[1024];
					EndPoint end = new IPEndPoint(IPAddress.Any, 0);
					sock.ReceiveFrom(data, ref end);

					ushort nameLen = BitConverter.ToUInt16(data, 0);
					string name = Encoding.Unicode.GetString(data, 2, nameLen * 2);
					Version ver = new Version(data[nameLen * 2 + 2 + 0], data[nameLen * 2 + 2 + 1], data[nameLen * 2 + 2 + 2], data[nameLen * 2 + 2 + 3]);
					uint current = BitConverter.ToUInt32(data, 6 + nameLen * 2);
					uint maxClients = BitConverter.ToUInt32(data, 10 + nameLen * 2);
					ushort addLen = BitConverter.ToUInt16(data, 14 + nameLen * 2);
					string addData = Encoding.Unicode.GetString(data, 16 + nameLen * 2, addLen * 2);

					Assert.AreEqual(Name.Length, nameLen);
					Assert.AreEqual(Name, name);
					Assert.AreEqual(Version, ver);
					Assert.AreEqual(0, current);
					Assert.AreEqual(10, maxClients);
					Assert.AreEqual(AdditionalData.Length, addLen);
					Assert.AreEqual(AdditionalData, addData);
				}
				finally
				{
					sock.Close();
				}
			}
		}
	}
}
