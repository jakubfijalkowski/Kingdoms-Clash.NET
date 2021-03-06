﻿using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Objects
{
	using Graphics.Resources;

	/// <summary>
	/// Tekst.
	/// </summary>
	public interface IText
		: IObject
	{
		/// <summary>
		/// Użyta czcionka.
		/// </summary>
		IFont Font { get; set; }

		/// <summary>
		/// Tekst.
		/// </summary>
		string TextValue { get; set; }

		/// <summary>
		/// Kolor.
		/// </summary>
		Vector4 Color { get; set; }

		/// <summary>
		/// Prawdziwy rozmiar
		/// </summary>
		Vector2 RealSize { get; }
	}
}
