using System.Drawing;
using ClashEngine.NET;
using ClashEngine.NET.Graphics.Gui;
using ClashEngine.NET.Graphics.Resources;
using ClashEngine.NET.Utilities;
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

	class SampleData
	{
		public int this[int index]
		{
			get { return index; }
		}
	}

	class GuiTestsScreen
		: ClashEngine.NET.Graphics.Gui.Screen
	{
		Game g;
		public bool IsButton1Clicked
		{
			get { return false; }
			set
			{
			}
		}

		public GuiTestsScreen(Game g)
			: base("Gui screen", new System.Drawing.RectangleF(0, 0, 800, 600))
		{
			this.g = g;
		}

		public override void OnInit()
		{
			base.OnInit();
			this.Content.ContentDirectory = "Content";
			var c = this.Content.Load<XamlGuiContainer>("TestGui.xml");
			
			var ctrl = c.Controls["Button1"];
			new ClashEngine.NET.Data.Binding(ctrl, "Clicked", this, "IsButton1Clicked");

			c.Bind(this);
		}

		public override void CheckControls()
		{
			if (this.Control("Button1") == 1)
			{
				g.Exit();
			}
			this.Control("Button2");
			this.Control("Button3");
		}
	}
}
