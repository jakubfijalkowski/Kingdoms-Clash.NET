using System.Collections.Generic;
using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Layout
{
	/// <summary>
	/// Silnik odpowiedzialny za układanie kontrolek/obiektów.
	/// </summary>
	public interface ILayoutEngine
	{
		/// <summary>
		/// Układa wszystkie elementy.
		/// </summary>
		/// <remarks>
		/// Możemy mieć pewność, że poszczególne elementu listy dziedziczą z IPositionableElement.
		/// </remarks>
		/// <param name="elements">Lista elementów do ułożenia.</param>
		/// <param name="size">Rozmiar kontrrolki w której powinny zmieścić się elementy.</param>
		/// <returns>Nowy rozmiar(zostanie zaaplikowany do kontrolki).</returns>
		Vector2 Layout<T>(IList<T> elements, Vector2 size)
			where T : IPositionableElement;
	}
}
