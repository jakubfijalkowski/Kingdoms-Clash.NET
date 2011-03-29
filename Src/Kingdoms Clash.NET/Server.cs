#if SERVER
using System;
using System.Threading;

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
		}
	}
}
#endif