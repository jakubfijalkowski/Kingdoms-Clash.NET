namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Panel - nie uczestniczy w interakcji z użytkownikiem, jest "statyczny".
	/// </summary>
	public class Panel
		: ControlBase, IPanel
	{
		#region Unused
		public override bool PermanentActive { get { return false; } }

		//public override void Update(double delta)
		//{ }

		public override int Check()
		{
			return 0;
		}

		public override bool ContainsMouse()
		{
			return false;
		}
		#endregion
	}
}
