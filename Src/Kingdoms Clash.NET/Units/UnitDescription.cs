using System.Diagnostics;

namespace Kingdoms_Clash.NET.Units
{
	using Interfaces.Resources;
	using Interfaces.Units;

	/// <summary>
	/// Opis jednostki.
	/// Jednostka do opisu wymaga tylko trzech właściwości: Health, Width, Height. Reszta jest opcjonalna(ale może być wymagana przez komponenty!).
	/// </summary>
	[DebuggerDisplay("Id = {Id,nq}")]
	[System.Windows.Markup.ContentProperty("Components")]
	public class UnitDescription
		: IUnitDescription
	{
		#region Private fields
		private IResourcesCollection _Costs = null;
		#endregion

		#region IUnitDescription Members
		/// <summary>
		/// Identyfikator jednostki.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Koszta wyprodukowania jednostki.
		/// </summary>
		IResourcesCollection IUnitDescription.Costs { get { return this._Costs; } }

		/// <summary>
		/// Komponenty, z których składa się jednostka.
		/// </summary>
		public IUnitComponentDescriptionsCollection Components { get; private set; }

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

		#region XAML integration
		/// <summary>
		/// Koszta wyprodukowania jednostki.
		/// </summary>
		public XAML.ResourcesCollection Costs { get; private set; }
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
			: this()
		{
			this.Id = id;
			this.Health = health;
			this.Width = width;
			this.Height = height;
		}

		/// <summary>
		/// Domyślny konstruktor - nic nie ustawia.
		/// </summary>
		public UnitDescription()
		{
			this._Costs = new Resources.ResourcesCollection();
			this.Costs = new XAML.ResourcesCollection(this._Costs);
			this.Components = new UnitComponentDescriptionsCollection();
		}
		#endregion
	}
}
