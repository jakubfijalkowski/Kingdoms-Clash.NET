using System;

namespace ClashEngine.NET.Graphics.Components
{
	using EntitiesManager;
	using Interfaces.Graphics;
	using Interfaces.Graphics.Components;

	/// <summary>
	/// Komponent do bezpośredniego dodawania komponentów do encji.
	/// </summary>
	public class ObjectHolder
		: RenderableComponent, IObjectHolder
	{
		#region IObjectHolder Members
		/// <summary>
		/// Obiekt.
		/// </summary>
		public IObject Object { get; private set; }
		#endregion

		#region RenderableComponent Members
		public override void Render()
		{
			this.Renderer.Draw(this.Object);
		}

		public override void Update(double delta)
		{ } 
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje komponent.
		/// </summary>
		public ObjectHolder(IObject obj)
			: base("ObjectHolder." + (obj != null ? obj.GetType().Name : ""))
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			this.Object = obj;
		}
		#endregion
	}
}
