using ClashEngine.NET.Interfaces;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Server
{
	using Interfaces;

	/// <summary>
	/// Obsługuje grę multiplayer - połączenie z klientami, czas oczekiwania na rozpoczęcie, itp.
	/// </summary>
	public class Multiplayer
		: IMultiplayer
	{
		#region Private fields
		IGameInfo GameInfo = null;
		//private bool InGame = false;
		private bool StopMainLoop = false;
		#endregion

		#region IMultiplayer Members
		/// <summary>
		/// Uruchamia grę.
		/// </summary>
		public void Start()
		{
			TcpServer server = new TcpServer(ServerConfiguration.Instance.Port, ServerConfiguration.Instance.MaxSpectators + 2,
				ServerConfiguration.Instance.Name, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version,
				ServerConfiguration.Instance.InfoPort);

			server.Start();
			while (!this.StopMainLoop)
			{

			}
			server.Stop();
		}

		/// <summary>
		/// Kończy grę.
		/// </summary>
		public void Stop()
		{
			this.StopMainLoop = true;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje.
		/// </summary>
		/// <param name="info"></param>
		public Multiplayer(IGameInfo info)
		{
			this.GameInfo = info;
		}
		#endregion
	}
}
