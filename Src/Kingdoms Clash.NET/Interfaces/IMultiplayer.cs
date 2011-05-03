using ClashEngine.NET.Interfaces;

namespace Kingdoms_Clash.NET.Server.Interfaces
{
	using NET.Interfaces;
	using NET.Interfaces.Units;
	using NET.Interfaces.Map;

	/// <summary>
	/// Interfejs dla gry multiplayer.
	/// </summary>
	public interface IMultiplayer
	{
		/// <summary>
		/// Informacje o grze.
		/// </summary>
		IGameInfo GameInfo { get; }

		/// <summary>
		/// Uruchamia grę.
		/// </summary>
		void Start();

		/// <summary>
		/// Kończy grę.
		/// </summary>
		void Stop();

		/// <summary>
		/// Wywoływane przez <see cref="IGameState"/> przy dodaniu nowej jednostki.
		/// </summary>
		/// <param name="unit"></param>
		void UnitAdded(IUnit unit);

		/// <summary>
		/// Wywoływane przy usunięciu jednostki z <see cref="IGameState"/>.
		/// </summary>
		/// <param name="unit"></param>
		void UnitDestroyed(IUnit unit);

		/// <summary>
		/// Wywoływane przy dodaniu nowego zasobu przez <see cref="IGameState"/>.
		/// </summary>
		/// <param name="res"></param>
		void ResourceAdded(IResourceOnMap res);

		/// <summary>
		/// Wywoływane przy usunięciu zasobu przez <see cref="IGameState"/>.
		/// </summary>
		/// <param name="res"></param>
		void ResourceRemoved(IResourceOnMap res);
	}
}
