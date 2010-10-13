using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Interfaces.Map
{
	/// <summary>
	/// Interfejs dla zasobu na mapie(umożliwia jego wyświetlanie).
	/// </summary>
	public interface IResourceOnMap
		: IGameEntity
	{
		/// <summary>
		/// Stan gry, do którego przynależy zasób.
		/// </summary>
		IGameState GameState { get; set; }

		/// <summary>
		/// Wartość.
		/// </summary>
		uint Value { get; set; }

		/// <summary>
		/// Zbiera zasób z mapy.
		/// </summary>
		void Gather();
	}
}
