using ClashEngine.NET.Graphics.Gui;
using ClashEngine.NET.Interfaces.Graphics.Gui;

namespace Kingdoms_Clash.NET.Player.Controllers.Internals
{
	/// <summary>
	/// Ekran kontrolera.
	/// </summary>
	internal class ControlerScreen
		: Screen
	{
		public ControlerScreen(IContainer container)
			: base("PlayerControler", container, new System.Drawing.RectangleF(0, 0, 1, 1))
		{ }

		protected override void CheckControls()
		{
			//throw new System.NotImplementedException();
		}
	}
}
