using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace ClashEngine.NET
{
	using Interfaces;

	/// <summary>
	/// Okno.
	/// </summary>
	public class Window
		: NativeWindow, IWindow
	{
		#region Private fields
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");
		private WindowFlags _Flags = WindowFlags.None;
		private IGraphicsContext _Context = null;
		private IInput _Input = null;
		#endregion

		#region IWindow Members
		/// <summary>
		/// Kontekst grafiki.
		/// </summary>
		public IGraphicsContext Context
		{
			get
			{
				base.EnsureUndisposed();
				return this._Context;
			}
		}

		/// <summary>
		/// Rozmiar okna.
		/// </summary>
		/// <remarks>
		/// Działa nawet wtedy, gdy ustawiona jest flaga <see cref="WindowFlags.FixedSize"/>.
		/// </remarks>
		public new Vector2 Size
		{
			get
			{
				base.EnsureUndisposed();
				return new Vector2(base.ClientSize.Width, base.ClientSize.Height);
			}
			set
			{
				base.EnsureUndisposed();
				base.ClientSize = new System.Drawing.Size((int)value.X, (int)value.Y);
			}
		}

		/// <summary>
		/// Flagi okna.
		/// Część można używać zamiennie z właściwościami. Patrz sekcje "Remarks" <see cref="WindowFlags"/>.
		/// </summary>
		public WindowFlags Flags
		{
			get
			{
				base.EnsureUndisposed();
				if (this.Context.VSync)
					this._Flags |= WindowFlags.VSync;
				else
					this._Flags &= ~WindowFlags.VSync;
				return this._Flags;
			}
			set
			{
				base.EnsureUndisposed();
				this._Flags = value;
				this.UpdateFlags();
			}
		}

		/// <summary>
		/// Czy kontekst graficzny okna jest aktualnie "wybrany".
		/// </summary>
		public bool IsCurrent
		{
			get
			{
				base.EnsureUndisposed();
				return this.Context.IsCurrent;
			}
		}

		/// <summary>
		/// Wejście okna.
		/// </summary>
		public IInput Input
		{
			get
			{
				base.EnsureUndisposed();
				return this._Input;
			}
		}

		/// <summary>
		/// Czy okno jest akutalnie zamykane.
		/// </summary>
		public bool IsClosing { get; private set; }

		/// <summary>
		/// Używana wersja OpenGL.
		/// </summary>
		public Version OpenGLVersion { get; private set; }

		/// <summary>
		/// Używa kontekstu tego okna jako aktualnego.
		/// </summary>
		public void Use()
		{
			base.EnsureUndisposed();
			if (!this.IsCurrent && !this.Context.IsDisposed)
			{
				this.Context.MakeCurrent(base.WindowInfo);
			}
		}

		/// <summary>
		/// Wyświetla zawartość okna.
		/// </summary>
		/// <remarks>
		/// Wybiera okno(jeśli nie jest wybrane) i wywołuje <see cref="OpenTK.Graphics.IGraphicsContext.SwapBuffers"/>.
		/// </remarks>
		public void Show()
		{
			if (this.Exists && !this.IsClosing)
			{
				this.Use();
				this.Context.SwapBuffers();
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje nowe okno.
		/// </summary>
		/// <param name="width">Szerokość okna.</param>
		/// <param name="height">Wysokość okna.</param>
		/// <param name="title">Tytuł.</param>
		/// <param name="flags">Flagi.</param>
		public Window(int width, int height, string title, WindowFlags flags = WindowFlags.Default)
			: this(width, height, title, flags, new Version(int.MaxValue, int.MaxValue), null, null)
		{ }

		/// <summary>
		/// Inicjalizuje nowe okno.
		/// </summary>
		/// <param name="width">Szerokość okna.</param>
		/// <param name="height">Wysokość okna.</param>
		/// <param name="title">Tytuł.</param>
		/// <param name="flags">Flagi.</param>
		/// <param name="openGLVer">Żądana wersja OpenGL.</param>
		public Window(int width, int height, string title, WindowFlags flags, Version openGLVer)
			: this(width, height, title, flags, openGLVer, null, null)
		{ }

		/// <summary>
		/// Inicjalizuje nowe okno.
		/// </summary>
		/// <param name="width">Szerokość okna.</param>
		/// <param name="height">Wysokość okna.</param>
		/// <param name="title">Tytuł.</param>
		/// <param name="flags">Flagi.</param>
		/// <param name="openGLVer">Żądana wersja OpenGL.</param>
		/// <param name="mode">Używany tryb graficzny.</param>
		/// <param name="device">Ekran, na którym będzie wyświetlone dane okno.</param>
		public Window(int width, int height, string title, WindowFlags flags, Version openGLVer, GraphicsMode mode, DisplayDevice device = null)
			: base(width, height, title, ((flags & WindowFlags.Fullscreen) == WindowFlags.Fullscreen ? GameWindowFlags.Fullscreen : GameWindowFlags.Default),
			(mode != null ? mode : GraphicsMode.Default), (device != null ? device : DisplayDevice.Default))
		{
			Logger.Info("Creating window");
			Logger.Info("\tTitle: {0}", title);
			Logger.Info("\tWindow size: {0} x {1}", width, height);
			Logger.Info("\tFlags: {0}", flags.ToString());
			if (openGLVer.Major == int.MaxValue)
				Logger.Info("\tOpenGL Version: newest");
			else
				Logger.Info("\tOpenGL Version: {0}.{1}", openGLVer.Major, openGLVer.Minor);

			this._Input = new Internals.WindowInput(this);

			this._Context = new GraphicsContext((mode != null ? mode : GraphicsMode.Default), this.WindowInfo, openGLVer.Major, openGLVer.Minor, GraphicsContextFlags.Default);
			this._Context.MakeCurrent(this.WindowInfo);
			this._Context.LoadAll();

			try
			{
				string versionStr = GL.GetString(StringName.Version);
				int idx = versionStr.IndexOf(' ');
				if (idx > 0)
				{
					versionStr = versionStr.Substring(0, idx);
				}
				this.OpenGLVersion = Version.Parse(versionStr);
			}
			catch (Exception ex)
			{
				Logger.WarnException("Cannot parse OpenGL version string " + GL.GetString(StringName.Version) + ". This should never occur.", ex);
				this.OpenGLVersion = new Version(0, 0, 0, 0);
			}

			this.Flags = flags;

			this.Closing += new EventHandler<System.ComponentModel.CancelEventArgs>(Window_Closing);
			this.Closed += new EventHandler<EventArgs>(Window_Closed);
			this.Resize += new EventHandler<EventArgs>(Window_Resize);
			this.VisibleChanged += new EventHandler<EventArgs>(Window_VisibleChanged);
			this.WindowBorderChanged += new EventHandler<EventArgs>(Window_WindowBorderChanged);
			this.WindowStateChanged += new EventHandler<EventArgs>(Window_WindowStateChanged);

			Logger.Info("Window created(OpenGL version: {0}.{1})", this.OpenGLVersion.Major, this.OpenGLVersion.Minor);
		}
		#endregion

		#region Overrides
		new public void ProcessEvents()
		{
			this.Input.LastCharacter = '\0';
			base.ProcessEvents();
		}

		public override void Dispose()
		{
			this.Context.Dispose();
			base.Dispose();
		}
		#endregion

		#region Private events
		private void Window_Closed(object sender, EventArgs e)
		{
			this.IsClosing = true;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.IsClosing = false;
		}

		private void Window_Resize(object sender, EventArgs e)
		{
			GL.Viewport(base.ClientRectangle);
		}

		private void Window_VisibleChanged(object sender, EventArgs e)
		{
			if (base.Visible)
				this._Flags |= WindowFlags.Visible;
			else
				this._Flags &= ~WindowFlags.Visible;
		}

		private void Window_WindowBorderChanged(object sender, EventArgs e)
		{
			if (base.WindowBorder == OpenTK.WindowBorder.Fixed)
				this._Flags |= WindowFlags.FixedSize;
			else
				this._Flags &= ~WindowFlags.FixedSize;
		}

		private void Window_WindowStateChanged(object sender, EventArgs e)
		{
			if (base.WindowState == OpenTK.WindowState.Fullscreen)
				this._Flags |= WindowFlags.Fullscreen;
			else
				this._Flags &= ~WindowFlags.Fullscreen;
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Aktualizuje flagi.
		/// </summary>
		private void UpdateFlags()
		{
			if ((this.Flags & WindowFlags.Fullscreen) == WindowFlags.Fullscreen)
				base.WindowState = OpenTK.WindowState.Fullscreen;
			else
				base.WindowState = OpenTK.WindowState.Normal;

			if ((this.Flags & WindowFlags.FixedSize) == WindowFlags.FixedSize)
				base.WindowBorder = OpenTK.WindowBorder.Fixed;
			else
				base.WindowBorder = OpenTK.WindowBorder.Resizable;

			this.Context.VSync = (this.Flags & WindowFlags.VSync) == WindowFlags.VSync;
			base.Visible = (this.Flags & WindowFlags.Visible) == WindowFlags.Visible;

			if ((this.Flags & WindowFlags.AltF4) == WindowFlags.AltF4)
				this.Input.KeyChanged += this.SupportAltF4;
			else
				this.Input.KeyChanged -= this.SupportAltF4;
		}

		/// <summary>
		/// Obsługa zamykania okna przez Alt+F4.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="keyPress"></param>
		private void SupportAltF4(object sender, KeyEventArgs keyPress)
		{
			if ((this.Input[Key.AltLeft] || this.Input[Key.AltRight]) && this.Input[Key.F4])
			{
				this.Close();
			}
		}
		#endregion
	}
}
