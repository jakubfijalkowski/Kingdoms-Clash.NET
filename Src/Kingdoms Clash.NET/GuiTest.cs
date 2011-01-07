using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClashEngine.NET.Graphics.Gui;
using ClashEngine.NET.Interfaces;
using ClashEngine.NET;
using ClashEngine.NET.Graphics.Gui.Controls;
using System.Diagnostics;

namespace Kingdoms_Clash.NET
{
	public class GuiTest
		: Game
	{
		static void Main(string[] args)
		{
			using (var g = new GuiTest())
			{
				g.Run();
			}
		}

		public GuiTest()
			: base(800, 600, "test")
		{

		}

		public override void OnInit()
		{
			Container container = new Container(this.Info);
			Panel panel1 = new Panel() { Id = "panel1" };
			container.Root.Controls.Add(panel1);
			Panel panel2 = new Panel() { Id = "panel2" };
			panel1.Controls.Add(panel2);

			Panel panel3 = new Panel() { Id = "panel3" };
			Panel panel4 = new Panel() { Id = "panel4" };
			panel3.Controls.Add(panel4);
			container.Root.Controls.Add(panel3);
		}

		public override void OnDeinit()
		{
		}
	}


}
