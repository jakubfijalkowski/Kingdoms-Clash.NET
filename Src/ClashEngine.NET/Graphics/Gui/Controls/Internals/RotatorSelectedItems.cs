namespace ClashEngine.NET.Graphics.Gui.Controls.Internals
{
	using Interfaces.Graphics.Gui.Controls;

	internal class RotatorSelectedItems
		: IRotatorSelectedItems
	{
		private IRotator Rotator;

		#region IRotatorSelectedItems Members
		public object this[int index]
		{
			get { return this.Rotator[index]; }
		}
		#endregion

		public RotatorSelectedItems(IRotator rotator)
		{
			this.Rotator = rotator;
		}
	}
}
