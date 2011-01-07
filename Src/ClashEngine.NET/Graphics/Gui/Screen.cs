using System.Drawing;
using OpenTK.Input;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces;
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Ekran jako kontener na kontrolki.
	/// </summary>
	public class Screen
		: NET.Screen, Interfaces.Graphics.Gui.IScreen
	{
		#region IScreen Members
		/// <summary>
		/// Prostokąt, w którym zawiera się GUI.
		/// </summary>
		public RectangleF Rectangle
		{
			get { return (base.Camera as Cameras.Movable2DCamera).Borders; }
		}

		/// <summary>
		/// Kontener GUI.
		/// </summary>
		public IContainer Gui { get; protected set; }
		#endregion

		#region Screen overrides
		/// <summary>
		/// Ukrywamy przed innymi.
		/// </summary>
		private new EntitiesManager.EntitiesManager Entities
		{
			get { return base.Entities; }
		}

		/// <summary>
		/// Wyświetla GUI.
		/// </summary>
		public override void Render()
		{
			if (this.Camera != null)
			{
				this.GameInfo.Renderer.Camera = this.Camera;
			}
			this.Gui.Render();
		}

		/// <summary>
		/// Aktualizuje GUI.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{
			this.Gui.Update(delta);
			this.CheckControls();
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje ekran.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="container">Gui.</param>
		/// <param name="type">Typ ekranu.</param>
		public Screen(string id, RectangleF rect, ScreenType type = ScreenType.Popup)
			: base(id, type)
		{
			this.Camera = new Cameras.Movable2DCamera(new OpenTK.Vector2(rect.Width, rect.Height), rect);
		}

		/// <summary>
		/// Inicjalizuje ekran.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="container">Gui.</param>
		/// <param name="type">Typ ekranu.</param>
		public Screen(string id, IContainer container, RectangleF rect, ScreenType type = ScreenType.Popup)
			: this(id, rect, type)
		{
			this.Gui = container;
		}
		#endregion

		#region Protected Members
		/// <summary>
		/// Metoda do sprawdzania, czy zaszły jakieś zdarzenia w GUI.
		/// </summary>
		protected virtual void CheckControls()
		{ }
		#endregion
	}
}
