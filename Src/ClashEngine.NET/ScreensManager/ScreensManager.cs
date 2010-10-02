using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTK.Input;

namespace ClashEngine.NET.ScreensManager
{
	using Interfaces.ScreensManager;

	/// <summary>
	/// Manager ekranów.
	/// Zobacz <see cref="Screen"/> dla większej ilości informacji.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	public class ScreensManager
		: IScreensManager
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private List<IScreen> Screens = new List<IScreen>();

		#region IScreensManager Members
		#region ICollection<IScreen> Members
		/// <summary>
		/// Czyśli kolekcję.
		/// </summary>
		public void Clear()
		{
			foreach (var s in this.Screens)
			{
				s.OnDeinit();
			}
			this.Screens.Clear();
		}

		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się wskazany ekran. 
		/// </summary>
		/// <param name="item">Ekran.</param>
		/// <returns></returns>
		public bool Contains(IScreen item)
		{
			return this.Screens.Contains(item);
		}

		/// <summary>
		/// Kopiuje kolekcje do tablicy.
		/// </summary>
		/// <param name="array">Tablica wyjściowa.</param>
		/// <param name="arrayIndex">Początkowy indeks.</param>
		public void CopyTo(IScreen[] array, int arrayIndex)
		{
			this.Screens.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Lista ekranów.
		/// </summary>
		public int Count
		{
			get { return this.Screens.Count; }
		}

		/// <summary>
		/// Czy jest tylko do odczytu - zawsze fałsz.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region IEnumerable<IScreen> Members
		public IEnumerator<IScreen> GetEnumerator()
		{
			return this.Screens.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Screens.GetEnumerator();
		}
		#endregion

		#region List management
		/// <summary>
		/// Dodaje ekran do listy.
		/// </summary>
		/// <param name="screen">Ekran do dodania.</param>
		public void Add(IScreen screen)
		{
			if (screen == null)
			{
				throw new ArgumentNullException("screen");
			}
			else if (this.Screens.Contains(screen))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("screen");
			}
			this.Screens.Add(screen);
			screen.Init(this);
			screen.OnInit();
			Logger.Trace("Screen added");
		}

		/// <summary>
		/// Dodaje ekran do listy i od razu go aktywuje.
		/// </summary>
		/// <param name="screen">Ekran do dodania.</param>
		public void AddAndMakeActive(IScreen screen)
		{
			this.Add(screen);
			this.MakeActive(screen);
		}

		/// <summary>
		/// Usuwa ekran z managera.
		/// </summary>
		/// <param name="screen">Ekran do usunięcia.</param>
		public bool Remove(IScreen screen)
		{
			if (screen == null)
			{
				throw new ArgumentNullException("screen");
			}
			bool deleted = this.Screens.Remove(screen);
			if (deleted)
			{
				Logger.Trace("Screen removed");
			}
			screen.OnDeinit();
			return deleted;
		}

		/// <summary>
		/// Pobiera ekran ze wskazanej pozycji.
		/// </summary>
		/// <param name="index">Indeks.</param>
		/// <returns></returns>
		public IScreen this[int index]
		{
			get
			{
				return this.Screens[index];
			}
		}
		#endregion

		#region Moving
		/// <summary>
		/// Przesuwa ekran na wierzch.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		public void MoveToFront(IScreen screen)
		{
			this.MoveTo(screen, 0);
		}

		/// <summary>
		/// Przesuwa ekran we wskazane miejsce.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		/// <param name="newPos">Nowa pozycja na liście(licząc od końca!).</param>
		public void MoveTo(IScreen screen, int newPos)
		{
			if (screen == null)
			{
				throw new ArgumentNullException("screen");
			}
			else if (!this.Screens.Contains(screen))
			{
				throw new Exceptions.ArgumentNotExistsException("screen");
			}
			int position = this.Screens.IndexOf(screen);
			if (position > newPos && newPos > 0)
			{
				newPos -= 1;
			}
			this.Screens.RemoveAt(position);
			this.Screens.Insert((newPos > 0 ? newPos - 1 : newPos), screen);
			Logger.Trace("Screen moved to front");
		}
		#endregion

		#region Changing state
		/// <summary>
		/// Zmienia ekran wskazany ekran na aktywny(tylko jeśli nie zasłania go nic innego).
		/// </summary>
		/// <param name="screen">Ekran.</param>
		/// <returns>Czy zmieniono stan ekranu.</returns>
		public bool MakeActive(IScreen screen)
		{
			if (screen == null)
			{
				throw new ArgumentNullException("screen");
			}
			else if (!this.Screens.Contains(screen))
			{
				throw new Exceptions.ArgumentNotExistsException("screen");
			}
			//Sprawdzamy, czy nic nie zasłania ekranu.
			for (int i = this.Screens.IndexOf(screen); i > 0; i--)
			{
				if (this.Screens[i].State != ScreenState.Closed && !this.Screens[i].IsPopup)
				{
					Logger.Warn("Cannot activate screen - it is covered by other screen");
					return false;
				}
			}
			//Jeśli nie jest popupem, musimy deaktywować ekrany "za".
			int deactivatedCounter = 0;
			if (!screen.IsPopup)
			{
				for (int i = this.Screens.IndexOf(screen); i < this.Screens.Count; i++)
				{
					if (this.Screens[i].State == ScreenState.Active)
					{
						this.Screens[i].ChangeState(ScreenState.Inactive);
						++deactivatedCounter;
						if (this.Screens[i].IsFullscreen) //Pełnoekranowy, i tak dalej nic nie będzie widać.
						{
							break;
						}
					}
				}
			}
			screen.ChangeState(ScreenState.Active);
			Logger.Info("Screen activated({0} deactivated)", deactivatedCounter);
			return true;
		}

		/// <summary>
		/// Zmienia ekran na nieaktywny.
		/// Ekran musi być aktywny by stać się nieaktywnym.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		/// <returns>Czy zmieniono stan ekranu.</returns>
		public bool MakeInactive(IScreen screen)
		{
			if (screen == null)
			{
				throw new ArgumentNullException("screen");
			}
			else if (!this.Screens.Contains(screen))
			{
				throw new Exceptions.ArgumentNotExistsException("screen");
			}
			if (screen.State != ScreenState.Active)
			{
				return false;
			}
			//Jeśli nie jest "popupem" musimy aktywować ekrany które są za nim.
			int activatedCounter = 0;
			if (!screen.IsPopup)
			{
				for (int i = this.Screens.IndexOf(screen); i < this.Screens.Count; i++)
				{
					if (this.Screens[i].State == ScreenState.Inactive)
					{
						this.Screens[i].ChangeState(ScreenState.Active);
						++activatedCounter;
						if (!this.Screens[i].IsFullscreen) //Pełnoekranowy, i tak dalej nic nie będzie widać.
						{
							break;
						}
					}
				}
			}

			screen.ChangeState(ScreenState.Inactive);
			Logger.Info("Screen deactivated({0} activated)", activatedCounter);
			return true;
		}

		/// <summary>
		/// Zamyka ekran.
		/// Przed zamknięciem ekran jest dezaktywowany.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		public void Close(IScreen screen)
		{
			if (screen == null)
			{
				throw new ArgumentNullException("screen");
			}
			else if (!this.Screens.Contains(screen))
			{
				throw new Exceptions.ArgumentNotExistsException("screen");
			}
			if (screen.State == ScreenState.Active)
			{
				this.MakeInactive(screen);
			}
			if (screen.State != ScreenState.Closed)
			{
				screen.ChangeState(ScreenState.Closed);
				Logger.Info("Screen closed");
			}
		}
		#endregion

		#region Rendering/updating
		/// <summary>
		/// Uaktualnia wszystkie ekrany, które powinny zostać uaktualnione(State == Active).
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		public void Update(double delta)
		{
			for (int i = 0; i < this.Screens.Count; i++)
			{
				IScreen screen = this.Screens[i];
				if (screen.State == ScreenState.Active)
				{
					screen.Update(delta);
				}
			}
		}

		/// <summary>
		/// Odrysowywuje wszystkie ekrany, które powinny być odrysowane.
		/// Renderowanie odbywa się od końca - ekran na początku listy jest odrysowywany na końcu.
		/// </summary>
		public void Render()
		{
			if (this.Screens.Count == 0)
			{
				return;
			}
			//Szukamy pierwszego pełnoekranowego ekranu(masło maślane x2...), który nie jest zamknięty.
			int firstFullscreen = 0;
			for(; firstFullscreen < this.Screens.Count
				&& !(this.Screens[firstFullscreen].IsFullscreen && this.Screens[firstFullscreen].State != ScreenState.Closed);
				++firstFullscreen);
			if (firstFullscreen == this.Screens.Count) //Gdy mamy tylko jeden ekran nie-fullscreen firstFullscreen dojdzie do 1, co później objawi się ArgumentOutOfRangeException
			{
				--firstFullscreen;
			}

			for (; firstFullscreen >= 0; --firstFullscreen)
			{
				this.Screens[firstFullscreen].Render();
			}
		}
		#endregion

		#region Firing events
		//Metody obsługujące zdarzenia.
		#region Keyboard
		void FireKeyDown(object sender, KeyboardKeyEventArgs e)
		{
			this.FireEvent(s => s.KeyDown(e));
		}

		void FireKeyUp(object sender, KeyboardKeyEventArgs e)
		{
			this.FireEvent(s => s.KeyUp(e));
		}
		#endregion

		#region Mouse
		void FireMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.FireEvent(s => s.MouseButtonDown(e));
		}

		void FireMouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.FireEvent(s => s.MouseButtonUp(e));
		}

		void FireMouseMove(object sender, MouseMoveEventArgs e)
		{
			this.FireEvent(s => s.MouseMove(e));
		}

		void FireMouseWheelChanged(object sender, MouseWheelEventArgs e)
		{
			this.FireEvent(s => s.MouseWheelChanged(e));
		}
		#endregion

		/// <summary>
		/// Metoda pomocnicza do wysyłania zdarzeń.
		/// </summary>
		/// <param name="e"></param>
		private void FireEvent(Func<IScreen, bool> e)
		{
			foreach (IScreen screen in this.Screens)
			{
				if (screen.State == ScreenState.Active && e(screen))
				{
					break;
				}
			}
		}
		#endregion

		#region IDisposable members
		public void Dispose()
		{
			foreach (var res in this.Screens)
			{
				this.Close(res);
			}
			this.Screens.Clear();
		}
		#endregion
		#endregion

		/// <summary>
		/// Inicjalizuje nowy manager i dodaje zdarzenia dla wejścia.
		/// </summary>
		public ScreensManager(bool addEvents = true)
		{
			if (addEvents)
			{
				Input.Instance.Keyboard.KeyDown += new EventHandler<OpenTK.Input.KeyboardKeyEventArgs>(FireKeyDown);
				Input.Instance.Keyboard.KeyUp += new EventHandler<OpenTK.Input.KeyboardKeyEventArgs>(FireKeyUp);

				Input.Instance.Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(FireMouseButtonDown);
				Input.Instance.Mouse.ButtonUp += new EventHandler<MouseButtonEventArgs>(FireMouseButtonUp);
				Input.Instance.Mouse.Move += new EventHandler<MouseMoveEventArgs>(FireMouseMove);
				Input.Instance.Mouse.WheelChanged += new EventHandler<MouseWheelEventArgs>(FireMouseWheelChanged);
			}
		}

		~ScreensManager()
		{
			this.Clear();
		}
	}
}
