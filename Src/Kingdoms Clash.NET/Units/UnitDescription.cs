using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Units
{
	using Interfaces.Resources;
	using Interfaces.Units;

	/// <summary>
	/// Opis jednostki.
	/// Jednostka do opisu wymaga tylko trzech właściwości: Health, Width, Height. Reszta jest opcjonalna(ale może być wymagana przez komponenty!).
	/// </summary>
	public class UnitDescription
		: IUnitDescription
	{
		#region IUnitDescription Members
		/// <summary>
		/// Identyfikator jednostki.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Koszta wyprodukowania jednostki.
		/// </summary>
		public IList<IResource> Costs { get; private set; }

		/// <summary>
		/// Komponenty, z których składa się jednostka.
		/// </summary>
		public IList<IUnitComponent> Components { get; private set; }

		/// <summary>
		/// Lista atrybutów jednostki.
		/// </summary>
		public IUnitAttributesCollection Attributes { get; private set; }

		/// <summary>
		/// Życie.
		/// </summary>
		public int Health { get; set; }

		/// <summary>
		/// Szerokość jednostki.
		/// </summary>
		public float Width { get; set; }

		/// <summary>
		/// Wysokość jednostki.
		/// </summary>
		public float Height { get; set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje nowy obiekt pozostawiając parametry niezainicjalizowane.
		/// </summary>
		/// <param name="id">Identyfikator jednostki.</param>
		public UnitDescription(string id)
			: this(id, 0, 0f, 0f)
		{ }

		/// <summary>
		/// Inicjalizuje nowy obiekt.
		/// </summary>
		/// <param name="id">Identyfikator jednostki.</param>
		/// <param name="health">Życie jednostki.</param>
		/// <param name="width">Szerokość jednostki.</param>
		/// <param name="height">Wysokość jednostki.</param>
		public UnitDescription(string id, int health, float width, float height)
		{
			this.Costs = new List<IResource>();
			this.Components = new List<IUnitComponent>();
			this.Attributes = new UnitAttributesCollection();

			this.Id = id;
			this.Health = health;
			this.Width = width;
			this.Height = height;
		}
		#endregion
	}
}
