using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Units.Sample
{
	using Interfaces.Player;
	using Interfaces.Units;

	/// <summary>
	/// Przykładowa nacja - do celów testowych.
	/// Docelowo nacje będą ładowane z plików.
	/// </summary>
	public class SampleNation
		: INation
	{
		private static readonly IList<IUnitDescription> Units = new List<IUnitDescription>(new IUnitDescription[]
		{
			new SampleWorkerDescription()
		});

		#region INation Members
		public IList<IUnitDescription> AvailableUnits
		{
			get { return Units; }
		}

		public IUnit CreateUnit(IUnitDescription id, IPlayer owner)
		{
			if (this.AvailableUnits.Contains(id))
			{
				if (id is IWorkerDescription)
				{
					return new Worker(id as IWorkerDescription, owner);
				}
				else
				{
					return null;
				}
			}
			return null;
		}
		#endregion
	}
}
