#if SERVER
using System;

namespace Kingdoms_Clash.NET.Server
{
	using UserData;

	/// <summary>
	/// Główna klasa dla serwera.
	/// </summary>
	public static class Server
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
			Multiplayer game = new Multiplayer(new ServerGameInfo());
			game.Start();

			//Umożliwia prawidłowe zatrzymanie serwera przy naciśnięciu Ctrl+C
			Console.CancelKeyPress += (s, e) =>
				{
					game.Stop();
				};
		}

		private class ServerGameInfo
			: ClashEngine.NET.Interfaces.IGameInfo
		{
			#region IGameInfo Members
			public ClashEngine.NET.Interfaces.IWindow MainWindow
			{
				get { throw new NotImplementedException("That should not occur"); }
			}

			public ClashEngine.NET.Interfaces.IScreensManager Screens
			{
				get { throw new NotImplementedException("That should not occur"); }
			}

			public ClashEngine.NET.Interfaces.IResourcesManager Content
			{
				get;
				private set;
			}

			public ClashEngine.NET.Interfaces.Graphics.IRenderer Renderer
			{
				get { throw new NotImplementedException("That should not occur"); }
			}
			#endregion

			#region Constructors
			public ServerGameInfo()
			{
				this.Content = new ClashEngine.NET.ResourcesManager();
			}
			#endregion
		}
	}
}
#endif