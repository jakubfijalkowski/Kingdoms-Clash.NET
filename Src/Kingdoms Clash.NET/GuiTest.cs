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
					<Panel Id='Test' Size='10,10'>
						<Panel Id='Test2' Size='1,1'/>
					</Panel>
					</XamlGuiContainer>";
			XamlGuiContainer container = new XamlGuiContainer(this.Info);
			XamlXmlReader reader = new XamlXmlReader(new StringReader(xaml));
			XamlObjectWriter writer = new XamlObjectWriter(reader.SchemaContext, new XamlObjectWriterSettings
			{
				RootObjectInstance = container
			});
			XamlServices.Transform(reader, writer);
		}

		public override void OnDeinit()
		{
		}
	}


}
