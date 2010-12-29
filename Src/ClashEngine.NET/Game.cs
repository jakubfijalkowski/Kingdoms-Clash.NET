using System.Diagnostics;

namespace ClashEngine.NET
{
	using Interfaces;

	/// <summary>
	/// Gra.
	/// </summary>
	public abstract class Game
		: IGame
	{
		#region Private fields
		private bool IsRunning = false;
		private bool IsExiting = false;
		private Internals.GameInfo _Info = new Internals.GameInfo();
		#endregion

		#region IGame Members
		/// <summary>
		/// Informacje o grze.
		/// </summary>
		public IGameInfo Info { get { return this._Info; } }

		/// <summary>
		/// Inicjalizacja gry.
		/// </summary>
		public abstract void OnInit();

		/// <summary>
		/// Deinicjalizacja gry.
		/// </summary>
		public abstract void OnDeinit();

		/// <summary>
		/// Metoda do uaktualnień.
		/// </summary>
		/// <param name="delta">Czas od ostatniego uaktualnienia.</param>
		public virtual void OnUpdate(double delta)
		{
			PhysicsManager.Instance.Update(delta);
			this.Info.Screens.Update(delta);
		}

		/// <summary>
		/// Odrysowywanie.
		/// </summary>
		public virtual void OnRender()
		{
			if (this.Info.Renderer != null)
			{
				this.Info.Renderer.Begin();
				this.Info.Screens.Render();
				this.Info.Renderer.End();
			}
			else
			{
				this.Info.Screens.Render();
			}
		}

		/// <summary>
		/// Uruchamia grę z maksymalną wydajnością.
		/// </summary>
		public void Run()
		{
			if (!this.IsRunning)
			{
				this.IsRunning = true;
				Stopwatch elapsedTime = new Stopwatch();
				this.OnInit();
				elapsedTime.Start();
				while (this.Info.MainWindow.Exists && !this.IsExiting)
				{
					elapsedTime.Stop();
					double delta = elapsedTime.Elapsed.TotalSeconds;
					elapsedTime.Reset();
					elapsedTime.Start();

					if (delta > 1.0)
						delta = 1.0; //Przycinamy do max. 1 sekundy

					this.Info.MainWindow.ProcessEvents();
					this.OnUpdate(delta);
					this.OnRender();
					this.Info.MainWindow.Show();
				}

				if (this.IsExiting)
				{
					this.Info.MainWindow.Close();
				}
				this.OnDeinit();
			}
		}

		/// <summary>
		/// Zamyka grę.
		/// </summary>
		public void Exit()
		{
			if (this.IsRunning)
			{
				this.IsExiting = true;
			}
		}
		#endregion

		#region Constructors
		public Game(int width, int height, string title, WindowFlags flags = WindowFlags.Default)
			: this(new Window(width, height, title, flags))
		{ }

		public Game(IWindow window)
		{
			this._Info.MainWindow = window;
			
			#if (DEBUG || FORCEHOTREPLACEMANAGER) && !FORCENOTUSINGHOTREPLACEMANAGER
			//Debug - używamy managera udostępniającego "gorącą podmianę"
			this._Info.Content = new HotReplaceResourcesManager();
			#else
			this._Info.Content = new ResourcesManager();
			#endif

			this._Info.Renderer = new Graphics.Renderer(this._Info.MainWindow.Size);
			this._Info.Screens = new ScreensManager(this._Info.MainWindow.Input, this._Info.Content, this._Info.Renderer);
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			this.Info.Screens.Dispose();
			this.Info.Content.Dispose();
			this.Info.MainWindow.Dispose();
		}
		#endregion
	}
}
