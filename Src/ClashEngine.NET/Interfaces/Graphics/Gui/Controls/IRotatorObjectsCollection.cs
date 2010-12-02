using System;
using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Controls
{
	/// <summary>
	/// Interfejs dla kolekcji obiektów rotatora.
	/// Wymaga, by wszystkie obiekty w kolekcji były tego samego typu.
	/// </summary>
	public interface IRotatorObjectsCollection
		: ICollection<object>
	{
		/// <summary>
		/// Typ obiektów w kolekcji.
		/// </summary>
		Type ObjectsType { get; }

		/// <summary>
		/// Pobiera obiekt na wskazanej pozycji.
		/// </summary>
		/// <param name="index">Indeks.</param>
		/// <returns></returns>
		object this[int index] { get; }
	}
}
