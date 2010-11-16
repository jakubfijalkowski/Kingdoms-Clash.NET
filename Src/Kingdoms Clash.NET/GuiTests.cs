using System.Drawing;
using ClashEngine.NET;
using ClashEngine.NET.Graphics.Gui;
//using ClashEngine.NET.Graphics.Gui.Controls;
using ClashEngine.NET.Graphics.Resources;
using ClashEngine.NET.Utilities;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Kingdoms_Clash.NET
{
	//Testy GUI
	class GuiTests
		: Game
	{
		public GuiTests()
			: base("Gui tests", 800, 600, false)
		{ }

		public override void Init()
		{
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			this.Screens.AddAndActivate(new FPSCounter(this.Content.Load<SystemFont>("Arial,15,"), Color.Yellow, new OpenTK.Vector2(800, 600)));
			this.Screens.AddAndActivate(new GuiTestsScreen(this));
		}

		static void Main(string[] args)
		{
			using (var g = new GuiTests())
			{
				g.Run();
			}
		}
	}

	class GuiTestsScreen
		: GuiScreen
	{
		Game g;
		string str = string.Empty;

		public GuiTestsScreen(Game g)
			: base("Gui screen", new System.Drawing.RectangleF(0, 0, 800, 600))
		{
			this.g = g;
		}

		public override void OnInit()
		{
			base.OnInit();

			//this.Add(new StaticText("Text1", this.Content.Load<SystemFont>("Arial,15,"), "Tekst", Color.Blue, new Vector2(100, 100)));
			//this.Add(new TextButton("Bt1", new Vector2(150, 100), new Vector2(80, 30), "Zamknij", this.Content.Load<SystemFont>("Arial,14,")));
			//this.Add(new TextBox("TB", new Vector2(100, 140), new Vector2(100, 30), () => str, (s) => str = s, this.Content.Load<SystemFont>("Arial,14,")));
		}

		public override void CheckControls()
		{
			//if (this.Button("Bt1"))
			//{
			//    g.Exit();
			//}
		}
	}
}
