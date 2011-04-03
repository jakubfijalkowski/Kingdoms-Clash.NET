#if SERVER
using System;
using System.Runtime.InteropServices;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Server
{
	using UserData;

	/// <summary>
	/// Główna klasa dla serwera.
	/// </summary>
	public class Server
	{		
		static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("To close server use Ctrl+C combination.");
			Console.WriteLine("DO NOT use 'X' button!");
			Console.ResetColor();

			ServerLoader loader = new ServerLoader(Defaults.RootDirectory, Defaults.UserData);
			try
			{
				loader.LoadConfiguration();
			}
			catch
			{
				return;
			}
			loader.LoadNations();
			loader.LoadResources();
			TcpServer server = new TcpServer(ServerConfiguration.Instance.Port,
				ServerConfiguration.Instance.MaxSpectators + 2,
				ServerConfiguration.Instance.Name, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
			server.Start();

			//Umożliwia prawidłowe zatrzymanie serwera przy naciśnięciu Ctrl+C
			Console.CancelKeyPress += (s, e) =>
				{
					server.Stop();
				};
		}
	}
}
#endif