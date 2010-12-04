using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	using Resources;
	
	/// <summary>
	/// Opis jednostki.
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
		IResourcesCollection Costs { get; }

		/// <summary>
		/// Komponenty, z których składa się jednostka.
		/// </summary>
		IUnitComponentDescriptionsCollection Components { get; }

		/// <summary>
		/// Życie.
		/// </summary>
		uint Health { get; }

		/// <summary>
		/// Czas potrzebny na wyprodukowanie jednostki.
		/// </summary>
		float CreationTime { get; }

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
