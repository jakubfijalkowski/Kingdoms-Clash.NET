using ClashEngine.NET.Graphics.Gui;

namespace Kingdoms_Clash.NET.Player.Controllers.Internals
{
	/// <summary>
	/// Ekran kontrolera.
	/// </summary>
	internal class ControlerScreen
		: Screen
	{
		public ControlerScreen()
			: base("PlayerControler", new System.Drawing.RectangleF(0, 0, 1, 1))
		{ }

		public override void CheckControls()
		{
			//throw new System.NotImplementedException();
		}
	}
}
