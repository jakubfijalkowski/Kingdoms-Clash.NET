using ClashEngine.NET.Interfaces.Graphics.Gui;

namespace ClashEngine.NET.Tests.TestObjects
{
	public class PositionableElement
		: IPositionableElement
	{
		#region IPositionableElement Members
		public OpenTK.Vector2 Position { get; set; }
		public OpenTK.Vector2 Size { get; set; }
		public bool Visible { get; set; }
		#endregion

		public PositionableElement(float w, float h, bool v = true)
			: this(0, 0, w, h, v)
		{ }

		public PositionableElement(float x, float y, float w, float h, bool v = false)
		{
			this.Position = new OpenTK.Vector2(x, y);
			this.Size = new OpenTK.Vector2(w, h);
			this.Visible = v;
		}
	}
}
