using ClashEngine.NET.Interfaces.Graphics.Gui;

namespace ClashEngine.NET.Tests.TestObjects
{
	public abstract class Control
			: IControl
	{
		public string Id { get; private set; }

		public Control(string id)
		{
			this.Id = id;
		}

		#region IControl Members
		public abstract IContainer Owner { get; set; }
		public abstract IUIData Data { get; set; }
		public abstract OpenTK.Vector2 ContainerOffset { get; set; }
		public abstract OpenTK.Vector2 Position { get; set; }
		public abstract OpenTK.Vector2 AbsolutePosition { get; set; }
		public abstract OpenTK.Vector2 Size { get; set; }
		public abstract bool PermanentActive { get; set; }
		public abstract bool IsActive { get; set; }
		public abstract bool IsHot { get; set; }
		public abstract bool Visible { get; set; }
		public abstract IObjectsCollection Objects { get; set; }
		public abstract bool ContainsMouse();
		public abstract void Update(double delta);
		public abstract void Render();
		public abstract int Check();
		public abstract void OnAdd();
		public abstract void OnRemove();
		#endregion

		#region IDataContext Members
		public abstract object DataContext { get; set; }
		#endregion

		#region INotifyPropertyChanged Members
		#pragma warning disable 0067
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}
