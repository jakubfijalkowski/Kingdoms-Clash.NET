using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	using Resources;

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

		#region Statistics
		/// <summary>
		/// Życie.
		/// </summary>
		int Health { get; }

		/// <summary>
		/// Szybkość jednostki.
		/// </summary>
		int Speed { get; }
		#endregion
	}
}
