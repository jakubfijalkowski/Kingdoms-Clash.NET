using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClashEngine.NET.Graphics.Gui;
using ClashEngine.NET.Interfaces;
using ClashEngine.NET;
using ClashEngine.NET.Graphics.Gui.Controls;
using System.Diagnostics;
using System.Xaml;
using System.IO;

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
			string xaml = @"<XamlGuiContainer xmlns='http://schemas.fiolek.org/gui' xmlns:cni='clr-namespace:ClashEngine.NET.Internals;assembly=ClashEngine.NET' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
					<Panel Id='Test' Size='100,100' Position='50,50'>
						<Button Id='b1' Size='100,100'>
							<Rectangle Color='1,0,0,1' />
						</Button>
					</Panel>
					</XamlGuiContainer>";
			XamlGuiContainer container = new XamlGuiContainer(this.Info);
			XamlXmlReader reader = new XamlXmlReader(new StringReader(xaml));
			XamlObjectWriter writer = new XamlObjectWriter(reader.SchemaContext, new XamlObjectWriterSettings
			{
				RootObjectInstance = container
			});
			XamlServices.Transform(reader, writer);
			this.Screens.AddAndActivate(new ClashEngine.NET.Graphics.Gui.Screen("Test", container, this.Window.ClientRectangle));
		}

		public override void OnDeinit()
		{
		}
	}


}
