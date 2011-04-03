using ClashEngine.NET.Interfaces;

namespace Kingdoms_Clash.NET.Server
{
	using Interfaces;

	/// <summary>
	/// Obsługuje grę multiplayer - połączenie z klientami, czas oczekiwania na rozpoczęcie, itp.
	/// </summary>
	public class Multiplayer
		: IMultiplayer
	{
		#region IMultiplayer Members
		/// <summary>
		/// Uruchamia grę.
		/// </summary>
		public void Start()
		{

		}

		/// <summary>
		/// Kończy grę.
		/// </summary>
		public void Stop()
		{

		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje.
		/// </summary>
		/// <param name="info"></param>
		public Multiplayer(IGameInfo info)
		{

		}
		#endregion
	}
}
