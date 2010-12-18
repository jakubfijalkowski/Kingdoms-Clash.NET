using System.ComponentModel;

namespace ClashEngine.NET.Graphics.Gui.Controls.Internals
{
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

		public void SendChanged()
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs("Item"));
			}
		}

		public void SendChanged(int idx)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs("Item." + idx));
			}
		}

		public RotatorSelectedItems(IRotator rotator)
		{
			this.Rotator = rotator;
		}
	}
}
