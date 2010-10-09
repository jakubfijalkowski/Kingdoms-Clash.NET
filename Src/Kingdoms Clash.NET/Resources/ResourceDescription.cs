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
		#endregion

		/// <summary>
		/// Inicjalizuje nowy opis zasobu.
		/// </summary>
		/// <param name="id">Identyfikator zasobu, zobacz: <see cref="Id"/></param>
		/// <param name="name">Nazwa zasobu, zobacz: <see cref="Name"/></param>
		/// <param name="description">Dłuższy opis zasobu.</param>
		public ResourceDescription(string id, string name, string description)
		{
			this.Id = id;
			this.Name = name;
			this.Description = description;
		}
	}
}
