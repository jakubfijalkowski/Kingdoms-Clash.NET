using System;
using OpenTK.Graphics.OpenGL;

namespace ClashEngine.NET
{
	using Interfaces;
	using Interfaces.ResourcesManager;
	using Interfaces.ScreensManager;

	/// <summary>
	/// Klasa gry.
	/// Używać tylko JEDNEJ klasy dziedziczącej po Game i NIE wywoływać żadnych metod z singletonów po wyłączeniu gry!
	/// Przy inicjalizacji domyślnie używa najnowszej możliwej wersji OpenGL.
	/// </summary>
	public abstract class Game
		: IGame, IDisposable
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");

		private GameWindow Window;

		/// <summary>
		/// Inicjalizuje obiekt gry.
		/// Przyjmowany jest domyślny tryb graficzny.
		/// </summary>
		/// <param name="name">Nazwa(równa nazwie okna).</param>
		/// <param name="width">Szerokość okna.</param>
		/// <param name="height">Wysokość okna.</param>
		/// <param name="fullscreen">Czy używać pełnego ekranu.</param>
		/// <param name="useVSync">Czy używać synchronizacji pionowej.</param>
		public Game(string name, int width, int height, bool fullscreen = true, bool useVSync = true)
			: this(name, width, height, fullscreen, useVSync, OpenTK.Graphics.GraphicsMode.Default)
		{ }

		/// <summary>
		/// Inicjalizuje obiekt gry.
		/// </summary>
		/// <param name="name">Nazwa(równa nazwie okna).</param>
		/// <param name="width">Szerokość okna.</param>
		/// <param name="height">Wysokość okna.</param>
		/// <param name="fullscreen">Czy używać pełnego ekranu.</param>
		/// <param name="useVSync">Czy używać synchronizacji pionowej.</param>
		/// <param name="mode">Tryb graficzny.</param>s
		public Game(string name, int width, int height, bool fullscreen, bool useVSync, OpenTK.Graphics.GraphicsMode mode)
		{
			Logger.Info("Creating game object");
			Logger.Info("Game name: {0}", name);
			Logger.Info("Window size: {0} x {1}", width, height);
			Logger.Info("Fullscreen: {0}", fullscreen);
			Logger.Info("VSync: {0}", (useVSync ? "on" : "off"));
			this.Name = name;
			this.Window = new GameWindow(this, name, width, height, fullscreen, useVSync, mode);
			(Input as Input).Init(this.Window.Keyboard, this.Window.Mouse, this.Window.Joysticks);
			this.ScreensManager = new ClashEngine.NET.ScreensManager.ScreensManager();
			Logger.Info("Window created");
		}

		#region IGame members
		#region Properties
		/// <summary>
		/// Nazwa gry.
		/// </summary>
		public string Name { get; private set; }

		#region Managers
		/// <summary>
		/// Manager ekranów dla gry.
		/// </summary>
		public IScreensManager ScreensManager { get; private set; }

		/// <summary>
		/// Manager zasobów.
		/// </summary>
		public IResourcesManager ResourcesManager
		{
			get
			{
				return NET.ResourcesManager.ResourcesManager.Instance;
			}
		}
		#endregion

		#region Window info
		/// <summary>
		/// Szerokość okna.
		/// </summary>
		public int Width
		{
			get
			{
				return this.Window.Width;
			}
		}

		/// <summary>
		/// Wysokość okna.
		/// </summary>
		public int Height
		{
			get
			{
				return this.Window.Height;
			}
		}

		/// <summary>
		/// Czy używać synchronizacji pionowej.
		/// </summary>
		public bool VSync
		{
			get
			{
				return this.Window.VSync == OpenTK.VSyncMode.On;
			}
		}

		/// <summary>
		/// Czy okno jest pełnoekranowe.
		/// </summary>
		public bool IsFullscreen
		{
			get
			{
				return this.Window.WindowState == OpenTK.WindowState.Fullscreen;
			}
		}

		/// <summary>
		/// Czy okno jest aktywne.
		/// </summary>
		public bool IsActive
		{
			get
			{
				return this.Window.Focused;
			}
		}

		/// <summary>
		/// Tryb graficzny.
		/// </summary>
		public OpenTK.Graphics.GraphicsMode Mode
		{
			get
			{
				return this.Window.Context.GraphicsMode;
			}
		}
		#endregion

		/// <summary>
		/// Wejście.
		/// </summary>
		public IInput Input
		{
			get
			{
				return NET.Input.Instance;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Inicjalizacja gry.
		/// Wywoływana np. przy ładowaniu okna.
		/// </summary>
		public virtual void Init()
		{ }

		/// <summary>
		/// Deinicjalizacja gry.
		/// Wywoływana przy zamykaniu okna.
		/// Zwalnia niezwolnione zasoby i zamyka wszystkie ekrany.
		/// </summary>
		public virtual void DeInit()
		{
			this.ScreensManager.Dispose();
			this.ResourcesManager.Dispose();
		}

		/// <summary>
		/// Metoda do uaktualnień.
		/// </summary>
		/// <param name="delta">Czas od ostatniego uaktualnienia.</param>
		public virtual void Update(double delta)
		{
			PhysicsManager.PhysicsManager.Instance.World.Step((float)delta);
			this.ScreensManager.Update(delta);
		}

		/// <summary>
		/// Odrysowywanie.
		/// </summary>
		public virtual void Render()
		{
			this.ScreensManager.Render();
		}

		#region Running
		/// <summary>
		/// Uruchamia grę z maksymalną wydajnością.
		/// </summary>
		public void Run()
		{
			this.Window.Run();
		}

		/// <summary>
		/// Uruchamia grę ze stałą liczbą wywołań metod Update i Render.
		/// </summary>
		/// <param name="updatesPerSecond">Liczba uaktualnień i odrysowań na sekundę.</param>
		public void Run(double updatesPerSecond)
		{
			this.Window.Run(updatesPerSecond);
		}

		/// <summary>
		/// Uruchamia grę ze stałą liczbą wywołań metod Update i Render.
		/// </summary>
		/// <param name="updatesPerSecond">Liczba uaktualnień na sekundę.</param>
		/// <param name="framesPerSecond">Liczba klatek na sekundę.</param>
		public void Run(double updatesPerSecond, double framesPerSecond)
		{
			this.Window.Run(updatesPerSecond, framesPerSecond);
		}
		#endregion

		/// <summary>
		/// Zamyka grę.
		/// </summary>
		public void Exit()
		{
			this.Window.Exit();
		}
		#endregion
		#endregion

		#region IDisposable members
		public void Dispose()
		{
			this.Window.Dispose();
		}
		#endregion

		#region Internal window class
		/// <summary>
		/// Wewnętrzna klasa okna, by nie zaśmiecać obiektu gry.
		/// </summary>
		private class GameWindow
			: OpenTK.GameWindow
		{
			private Game Parent;

			public GameWindow(Game parent, string name, int width, int height, bool fullscreen, bool useVSync, OpenTK.Graphics.GraphicsMode mode)
				: base(width, height, mode, name, (fullscreen ? OpenTK.GameWindowFlags.Fullscreen : OpenTK.GameWindowFlags.Default),
				OpenTK.DisplayDevice.Default, int.MaxValue, int.MaxValue, OpenTK.Graphics.GraphicsContextFlags.Default)
			{
				this.WindowBorder = OpenTK.WindowBorder.Fixed;
				base.VSync = (useVSync ? OpenTK.VSyncMode.On : OpenTK.VSyncMode.Off);
				this.Parent = parent;
			}

			protected override void OnLoad(EventArgs e)
			{
				Logger.Trace("Initializing...");
				//Ustawiamy viewport
				GL.Viewport(this.ClientRectangle);
				this.Parent.Init();
				Logger.Trace("Initialized");
			}

			protected override void OnUnload(EventArgs e)
			{
				Logger.Trace("Deinitializing...");
				this.Parent.DeInit();
				Logger.Trace("Deinitialized");
			}

			protected override void OnUpdateFrame(OpenTK.FrameEventArgs e)
			{
				MainThreadCallbacksManager.Instance.Call();
				this.Parent.Update(e.Time);
			}

			protected override void OnRenderFrame(OpenTK.FrameEventArgs e)
			{
				this.Parent.Render();
				this.SwapBuffers();
			}
		}
		#endregion
	}
}
