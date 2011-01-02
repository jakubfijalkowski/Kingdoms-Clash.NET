using System;
using ClashEngine.NET.Interfaces;

namespace ClashEngine.NET.Tests.TestObjects
{
	/// <summary>
	/// Zaślepka na implementacje IWindow.
	/// </summary>
	public class Window
		: IWindow
	{
		#region IWindow Members
		public OpenTK.Graphics.IGraphicsContext Context { get; set; }
		public OpenTK.Vector2 Size { get; set; }
		public WindowFlags Flags { get; set; }
		public bool IsCurrent { get; set; }
		public IInput Input { get; set; }
		public bool IsClosing { get; set; }
		public Version OpenGLVersion { get; set; }

		public void Use() { }
		public void Show() { }
		#endregion

		#region INativeWindow Members
		public System.Drawing.Rectangle Bounds { get; set; }
		public System.Drawing.Rectangle ClientRectangle { get; set; }
		public System.Drawing.Size ClientSize { get; set; }
		public bool Exists { get; set; }
		public bool Focused { get; set; }
		public int Height { get; set; }
		public System.Drawing.Icon Icon { get; set; }
		public System.Drawing.Point Location { get; set; }
		public OpenTK.Input.IInputDriver InputDriver { get; set; }
		System.Drawing.Size OpenTK.INativeWindow.Size { get; set; }
		public string Title { get; set; }
		public bool Visible { get; set; }
		public int Width { get; set; }
		public OpenTK.Platform.IWindowInfo WindowInfo { get; set; }
		public OpenTK.WindowBorder WindowBorder { get; set; }
		public OpenTK.WindowState WindowState { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		#pragma warning disable 0067
		public event EventHandler<EventArgs> Closed;
		public event EventHandler<System.ComponentModel.CancelEventArgs> Closing;
		public event EventHandler<EventArgs> Disposed;
		public event EventHandler<EventArgs> FocusedChanged;
		public event EventHandler<EventArgs> IconChanged;
		public event EventHandler<OpenTK.KeyPressEventArgs> KeyPress;
		public event EventHandler<EventArgs> MouseEnter;
		public event EventHandler<EventArgs> MouseLeave;
		public event EventHandler<EventArgs> Move;
		public event EventHandler<EventArgs> Resize;
		public event EventHandler<EventArgs> TitleChanged;
		public event EventHandler<EventArgs> VisibleChanged;
		public event EventHandler<EventArgs> WindowBorderChanged;
		public event EventHandler<EventArgs> WindowStateChanged;
		#pragma warning restore 0067

		public void Close() { }
		public System.Drawing.Point PointToClient(System.Drawing.Point point) { return System.Drawing.Point.Empty; }
		public System.Drawing.Point PointToScreen(System.Drawing.Point point) { return System.Drawing.Point.Empty; }
		public void ProcessEvents() { }
		#endregion

		#region IDisposable Members
		public void Dispose() { }
		#endregion
	}
}
