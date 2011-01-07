using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClashEngine.NET.Graphics.Gui.Controls
{
	using Interfaces.Graphics.Gui.Controls;

	/// <summary>
	/// Panel na same obiekty.
	/// </summary>
	public class Pane
		: StylizableControlBase, IPane
	{
		#region Unused
		public override bool PermanentActive
		{
			get { return false; }
		}

		public override int Check()
		{
			return 0;
		}
		#endregion
	}
}
