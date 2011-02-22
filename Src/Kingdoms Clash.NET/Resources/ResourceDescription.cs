using OpenTK;

namespace Kingdoms_Clash.NET.Resources
{
	using Interfaces.Resources;

	/// <summary>
	/// Opis zasobu.
	/// Używane tylko do opisywania zasobów w sposób przyjazny dla gracza.
	/// </summary>
	public class ResourceDescription
		: IResourceDescription
	{
		#region IResourceDescription Members
		/// <summary>
		/// Identyfikator(game-friendly) zasobu.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Nazwa(user-friendly) zasobu.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Dłuższy opis.
		/// </summary>
		public string Description { get; private set; }

		/// <summary>
		/// Rozmiar zasobu w grze.
		/// </summary>
		public OpenTK.Vector2 Size { get; private set; }

		/// <summary>
		/// Obrazek z zasobem.
		/// </summary>
		public string Image { get; private set; }

		/// <summary>
		/// Figura, która jest przybliżeniem zasobu. Używane do prezentacji fizyki.
		/// </summary>
		public Vector2[] Polygon { get; private set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje nowy opis zasobu.
		/// </summary>
		/// <param name="id">Identyfikator zasobu, zobacz: <see cref="Id"/>.</param>
		/// <param name="name">Nazwa zasobu, zobacz: <see cref="Name"/>.</param>
		/// <param name="description">Dłuższy opis zasobu.</param>
		/// <param name="size">Rozmiar zasobu.</param>
		/// <param name="image">Obrazek z zasobem.</param>
		/// <param name="polygon">Figura dla zasobu, zobacz <see cref="Polygon"/>.</param>
		public ResourceDescription(string id, string name, string description, OpenTK.Vector2 size, string image, Vector2[] polygon)
		{
			this.Id = id;
			this.Name = name;
			this.Description = description;
			this.Size = size;
			this.Image = image;
			this.Polygon = polygon;
		}
		#endregion
	}
}
