using System.ComponentModel;

namespace ClashEngine.NET.Graphics.Gui.Controls.Internals
{
	using Extensions;
	using Interfaces.Graphics.Gui.Controls;

	internal class RotatorSelectedItems
		: IRotatorSelectedItems, INotifyPropertyChanged
	{
		private IRotator Rotator;

		#region IRotatorSelectedItems Members
		public object this[int index]
		{
			get { return this.Rotator[index]; }
		}
		#endregion

		#region INotifyPropertyChanged Members
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Raising events
		public void RaiseChanged()
		{
			this.PropertyChanged.Raise(this, "Item");
		}

		public void RaiseChanged(int idx)
		{
			this.PropertyChanged.Raise(this, "Item");
		}
		#endregion

		public RotatorSelectedItems(IRotator rotator)
		{
			this.Rotator = rotator;
		}
	}
}
