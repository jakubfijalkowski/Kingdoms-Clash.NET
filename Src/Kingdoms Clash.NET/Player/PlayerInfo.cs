namespace Kingdoms_Clash.NET.Player
{
	using Interfaces.Player;

	/// <summary>
	/// Informacje o graczu przekazywane do <see cref="IGameState.Initialize"/>.
	/// </summary>
	public class PlayerInfo
		: IPlayerInfo
	{
		#region IPlayerInfo Members
		/// <summary>
		/// Nazwa gracza.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Wybrana nacja.
		/// </summary>
		public Interfaces.Units.INation Nation { get; private set; }

		/// <summary>
		/// Kontroler.
		/// </summary>
		public IPlayerController Controller { get; private set; }

		/// <summary>
		/// Czy pokazywać statystyki dla tego gracza.
		/// </summary>
		public bool ShowStatistics { get; private set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje obiekt.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nation"></param>
		/// <param name="controller"></param>
		/// <param name="showStatistics"></param>
		public PlayerInfo(string name, Interfaces.Units.INation nation, IPlayerController controller, bool showStatistics)
		{
			this.Name = name;
			this.Nation = nation;
			this.Controller = controller;
			this.ShowStatistics = showStatistics;
		}
		#endregion
	}
}
