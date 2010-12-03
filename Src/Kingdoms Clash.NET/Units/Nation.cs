using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Units
{
	using Interfaces.Units;

	/// <summary>
	/// Nacja.
	/// </summary>
	[System.Windows.Markup.ContentProperty("AvailableUnits")]
	public class Nation
		: INation
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");
		
		#region INation Members
		/// <summary>
		/// Nazwa nacji.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Ścieżka do obrazka zamku.
		/// </summary>
		public string CastleImage { get; set; }

		/// <summary>
		/// Dostępne jednostki nacji.
		/// </summary>
		public IUnitDescriptionsCollection AvailableUnits { get; private set; }

		/// <summary>
		/// Tworzy jednostkę na podstawie id.
		/// </summary>
		/// <param name="id">Identyfikator jednostki.</param>
		/// <param name="owner">Właściciel.</param>
		/// <returns>Nowoutworzona jednostka bądź null, gdy nie znajduje się w kolekcji AvailableUnits.</returns>
		public IUnit CreateUnit(string id, Interfaces.Player.IPlayer owner)
		{
			IUnitDescription desc = this.AvailableUnits[id];
			if (desc == null)
			{
				return null;
			}
			Logger.Debug("Creating unit {0} from nation {1}", id, this.Name);
			return new Unit(desc, owner);
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje nową nacje.
		/// </summary>
		/// <param name="name">Nazwa nacji.</param>
		/// <param name="image">Ścieżka do obrazka zamku.</param>
		public Nation(string name, string image, IEnumerable<IUnitDescription> descriptions)
		{
			this.Name = name;
			this.CastleImage = image;
			this.AvailableUnits = new UnitDescriptionsCollection(descriptions);
		}

		/// <summary>
		/// Domyślny konstruktor - nic nie ustawia.
		/// </summary>
		public Nation()
		{
			this.AvailableUnits = new UnitDescriptionsCollection();
		}
		#endregion

		public override string ToString()
		{
			return string.Format("Name = {0}", this.Name);
		}
	}
}
