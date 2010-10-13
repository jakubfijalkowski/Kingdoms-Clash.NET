namespace Kingdoms_Clash.NET.Interfaces.Units
{
	using Map;
	using Player;

	/// <summary>
	/// Zdarzenie kolizji jednostki z graczem.
	/// Kolizja odbywa się zawsze, niezależnie czy jednostka jest tego gracza, czy przeciwnika.
	/// </summary>
	/// <param name="unit">Jednostka, która koliduje.</param>
	/// <param name="player">Gracz z którym koliduje.</param>
	public delegate void CollisionWithPlayerEventHandler(IUnit unit, IPlayer player);

	/// <summary>
	/// Handler dla zdarzenia kolizji jednostek.
	/// </summary>
	/// <param name="a">Pierwsza jednostka.</param>
	/// <param name="b">Druga jednostka.</param>
	public delegate void CollisionWithUnitEventHandler(IUnit a, IUnit b);

	/// <summary>
	/// Handler zdarzenia zderzenia się jednostki z zasobem.
	/// </summary>
	/// <param name="unit">Jednostka, która się zderza.</param>
	/// <param name="resource">Zasób, z którym się zderzyła.</param>
	/// <returns>Czy zebrano zasób.</returns>
	public delegate bool CollisionWithResourceEventHandler(IUnit unit, IResourceOnMap resource);
}
