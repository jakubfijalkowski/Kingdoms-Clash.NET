using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	using Resources;
	
	/// <summary>
	/// Opis/identyfikator jednostki.
	/// Jednostka do opisu wymaga tylko trzech właściwości: Health, Width, Height. Reszta jest opcjonalna(ale może być wymagana przez komponenty!).
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
		/// Lista atrybutów jednostki.
		/// </summary>
		IUnitAttributesCollection Attributes { get; }

		/// <summary>
		/// Życie.
		/// </summary>
		int Health { get; }

		/// <summary>
		/// Szerokość jednostki.
		/// </summary>
		float Width { get; }

		/// <summary>
		/// Wysokość jednostki.
		/// </summary>
		float Height { get; }
	}
}
