using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	using Resources;

	/// <summary>
	/// Typ jednostki.
	/// </summary>
	//public enum UnitType
	//{
	//    Worker,
	//    Soldier
	//}

	/// <summary>
	/// Opis/identyfikator jednostki.
	/// </summary>
	public interface IUnitDescription
	{
		/// <summary>
		/// Identyfikator jednostki.
		/// </summary>
		string Id { get; }

		/// <summary>
		/// Koszta wyprodukowania jednostki.
		/// </summary>
		IList<IResource> Costs { get; }

		/// <summary>
		/// Komponenty, z których składa się jednostka.
		/// </summary>
		IList<IUnitComponent> Components { get; }

		/// <summary>
		/// Typ jednostki.
		/// </summary>
		//UnitType Type { get; }

		/// <summary>
		/// Życie.
		/// Tylko ta "statystyka" jest wymagana przez jednostkę, reszta jest zależna od komponentów z których jednostka będzie się składać.
		/// </summary>
		int Health { get; }
	}
}
