﻿using FarseerPhysics.Dynamics;
using OpenTK;

namespace ClashEngine.NET.Interfaces
{
	/// <summary>
	/// Manager fizyki.
	/// </summary>
	public interface IPhysicsManager
	{
		/// <summary>
		/// Świat.
		/// </summary>
		World World { get; }

		/// <summary>
		/// Grawitacja.
		/// Odpowiada World.Gravity.
		/// </summary>
		Vector2 Gravity { get; set; }

		/// <summary>
		/// Krok czasowy obliczeń.
		/// </summary>
		float TimeStep { get; set; }

		/// <summary>
		/// Uaktualnia World.
		/// </summary>
		void Update(double delta);
	}
}
