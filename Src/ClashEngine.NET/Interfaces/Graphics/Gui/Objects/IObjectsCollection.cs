﻿using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Objects
{
	/// <summary>
	/// Kolekcja obiektów renderera GUI.
	/// </summary>
	public interface IObjectsCollection
		: ICollection<IObject>
	{
		/// <summary>
		/// Właściciel.
		/// </summary>
		IStylizableControl Owner { get; }
	}
}