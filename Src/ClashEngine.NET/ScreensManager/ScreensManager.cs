using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClashEngine.NET.ScreensManager
{
	using Interfaces;
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

		#region Private fields
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private List<IScreen> Screens = new List<IScreen>();
		private IInput Input = null;
		#endregion

		#region IScreensManager Members
		/// <summary>
		/// Dodaje ekran do listy i od razu go aktywuje.
		/// <see cref="Add(IScreen)"/>
		/// </summary>
		/// <param name="screen">Ekran do dodania.</param>
		public void AddAndActivate(IScreen screen)
		{
			this.Add(screen);
			this.Activate(screen);
		}

		/// <summary>
		/// Pobiera ekran o wskazanym Id.
		/// </summary>
		/// <param name="index">Identyfikator ekranu.</param>
		/// <returns>Ekran, bądź null, gdy nie znaleziono</returns>
		public IScreen this[string id]
		{
			get { return this.Screens.Find(s => s.Id == id); }
		}

		/// <summary>
		/// Przesuwa ekran we wskazane miejsce.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		/// <param name="newPos">Nowa pozycja na liście(licząc od końca!).</param>
		/// <exception cref="Exceptions.NotFoundException">Nie znaleziono wskazanego ekranu.</exception>
		/// <exception cref="ArgumentNullException">Parametr screen == null.</exception>
		/// <exception cref="Exceptions.ArgumentNotExistsException">Ekran nie jest w managerze.</exception>
		public void MoveTo(IScreen screen, int newPos)
		{
			if (screen == null)
			{
				throw new ArgumentNullException("screen");
			}
			int idx = this.Screens.IndexOf(screen);
			if (idx == -1)
			{
				throw new Exceptions.ArgumentNotExistsException("screen");
			}
			this.Screens.RemoveAt(idx);
			if (screen.State == ScreenState.Activated ||
				(screen.State < ScreenState.Hidden && screen.Type == ScreenType.Fullscreen)) //Tylko, gdy ekran był aktywny coś się może zmienić
			{
				this.SetStates(0, idx);
			}

			this.Screens.Insert(newPos, screen);
			screen.State = this.CheckStateAt(newPos);

			//Jeśli jest to popup to nie musimy nic pod zmieniać
			//Tak samo gdy stan tego ekranu to "ukryty" - reszta już musi być ukryta
			if (screen.Type != ScreenType.Popup && screen.State != ScreenState.Hidden)
			{
				this.SetStates(newPos);
			}
			Logger.Debug("Screen {0} moved to position {1}({2})", screen.Id, newPos, screen.State);
		}

		/// <summary>
		/// Przesuwa ekran na wierzch.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		/// <exception cref="Exceptions.NotFoundException">Nie znaleziono wskazanego ekranu.</exception>
		/// <exception cref="Exceptions.ArgumentNotExistsException">Ekran nie jest w managerze.</exception>
		public void MoveToFront(IScreen screen)
		{
			this.MoveTo(screen, 0);
		}

		/// <summary>
		/// Przesuwa ekran we wskazane miejsce.
		/// </summary>
		/// <param name="id">Identyfikator ekranu.</param>
		/// <param name="newPos">Nowa pozycja na liście(licząc od końca!).</param>
		/// <exception cref="Exceptions.NotFoundException">Nie znaleziono wskazanego ekranu.</exception>
		public void MoveTo(string id, int newPos)
		{
			this.MoveTo(this.GetOrThrow(id), newPos);
		}

		/// <summary>
		/// Przesuwa ekran na wierzch.
		/// </summary>
		/// <param name="id">Identyfikator ekranu.</param>
		/// <exception cref="Exceptions.NotFoundException">Nie znaleziono wskazanego ekranu.</exception>
		public void MoveToFront(string id)
		{
			this.MoveTo(this.GetOrThrow(id), 0);
		}

		/// <summary>
		/// Aktywuje(jeśli może) wskazany ekran.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		/// <exception cref="Exceptions.NotFoundException">Nie znaleziono wskazanego ekranu.</exception>
		/// <exception cref="ArgumentNullException">Parametr screen == null.</exception>
		/// <exception cref="Exceptions.ArgumentNotExistsException">Ekran nie jest w managerze.</exception>
		public void Activate(IScreen screen)
		{
			if (screen == null)
			{
				throw new ArgumentNullException("screen");
			}
			int idx = this.Screens.IndexOf(screen);
			if (idx == -1)
			{
				throw new ArgumentException("Not member of manager", "screen");
			}

			if (screen.State != ScreenState.Deactivated) //Nie aktywować tylko niekatywny
			{
				return;
			}
			screen.State = this.CheckStateAt(idx);

			//Jeśli jest to popup to nie musimy nic pod zmieniać
			//Tak samo gdy stan tego ekranu to "ukryty" - reszta już musi być ukryta
			if (screen.Type != ScreenType.Popup && screen.State != ScreenState.Hidden)
			{
				this.SetStates(idx);
			}

			Logger.Debug("Screen {0} activated({1})", screen.Id, screen.State);
		}

		/// <summary>
		/// Deaktywuje wskazany ekran.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		/// <exception cref="Exceptions.NotFoundException">Nie znaleziono wskazanego ekranu.</exception>
		/// <exception cref="ArgumentNullException">Parametr screen == null.</exception>
		/// <exception cref="Exceptions.ArgumentNotExistsException">Ekran nie jest w managerze.</exception>
		public void Deactivate(IScreen screen)
		{
			if (screen == null)
			{
				throw new ArgumentNullException("screen");
			}
			int idx = this.Screens.IndexOf(screen);
			if (idx == -1)
			{
				throw new Exceptions.ArgumentNotExistsException("screen");
			}
			if (screen.State == ScreenState.Deactivated)
			{
				return;
			}

			var oldState = screen.State;
			screen.State = ScreenState.Deactivated;

			if (oldState == ScreenState.Activated)
			{
				this.SetStates(idx);
			}
			Logger.Debug("Screen {0} deactivated", screen.Id);
		}

		/// <summary>
		/// Aktywuje(jeśli może) wskazany ekran.
		/// </summary>
		/// <param name="id">Identyfikator ekranu.</param>
		/// <exception cref="Exceptions.NotFoundException">Nie znaleziono wskazanego ekranu.</exception>
		public void Activate(string id)
		{
			this.Activate(this.GetOrThrow(id));
		}

		/// <summary>
		/// Deaktywuje wskazany ekran.
		/// </summary>
		/// <param name="id">Identyfikator ekranu.</param>
		/// <exception cref="Exceptions.NotFoundException">Nie znaleziono wskazanego ekranu.</exception>
		public void Deactivate(string id)
		{
			this.Deactivate(this.GetOrThrow(id));
		}

		/// <summary>
		/// Uaktualnia wszystkie ekrany, które powinny zostać uaktualnione(State == Active).
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		public void Update(double delta)
		{
			for (int i = 0; i < this.Screens.Count; i++)
			{
				if (this.Screens[i].State == ScreenState.Deactivated)
				{
					continue;
				}
				else if (this.Screens[i].State >= ScreenState.Covered)
				{
					break;
				}
				this.Screens[i].Update(delta);
			}
		}

		/// <summary>
		/// Odrysowywuje wszystkie ekrany, które powinny być odrysowane.
		/// Renderowanie odbywa się od końca - ekran na początku listy jest odrysowywany na końcu.
		/// </summary>
		public void Render()
		{
			for (int i = this.Screens.FindLastIndex(s => s.State <= ScreenState.Covered); i >= 0; i--)
			{
				if (this.Screens[i].State != ScreenState.Deactivated)
				{
					this.Screens[i].Render();
				}
			}
		}
		#endregion

		#region ICollection<IScreen> Members
		/// <summary>
		/// Dodaje ekran do kolekcji.
		/// </summary>
		/// <param name="item">Ekran.</param>
		/// <exception cref="ArgumentNullException">item == null</exception>
		/// <exception cref="Exceptions.ArgumentAlreadyExistsException">Ekran o Id równym wskazanemu już istnieje.</exception>
		public void Add(IScreen item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (this.Contains(item))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("item");
			}
			this.Screens.Add(item);
			item.Manager = this;
			item.Input = this.Input;
			item.State = ScreenState.Deactivated;
			item.OnInit();
			Logger.Debug("Screen {0} added to manager", item.Id);
		}

		/// <summary>
		/// Usuwa ekran z kolekcji(o takim samym id).
		/// </summary>
		/// <param name="item">Ekran.</param>
		/// <returns>Czy usunięto.</returns>
		public bool Remove(IScreen item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.Screens.RemoveAll(s =>
				{
					if (s.Id == item.Id)
					{
						s.OnDeinit();
						Logger.Debug("Screen {0} removed from manager", item.Id);
						return true;
					}
					return false;
				}) > 0;
		}

		/// <summary>
		/// Czyści kolekcję.
		/// </summary>
		public void Clear()
		{
			foreach (var screen in this.Screens)
			{
				screen.OnDeinit();
			}
			this.Screens.Clear();
			Logger.Debug("Screens manager cleared");
		}

		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się ekran o Id równym wskazanemu.
		/// </summary>
		/// <param name="item">Ekran do porównywania.</param>
		/// <returns>Czy zawiera, czy nie.</returns>
		public bool Contains(IScreen item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.Screens.Find(s => s.Id == item.Id) != null;
		}

		/// <summary>
		/// Kopiuje kolekcje do tablicy.
		/// </summary>
		/// <param name="array">Tablica.</param>
		/// <param name="arrayIndex">Indeks początkowy.</param>
		public void CopyTo(IScreen[] array, int arrayIndex)
		{
			this.Screens.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Pobiera liczbę ekranów w kolekcji.
		/// </summary>
		public int Count
		{
			get { return this.Screens.Count; }
		}

		/// <summary>
		/// Czy kolekcja jest tylko do odczytu - zawsze false.
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

		#region IDisposable members
		public void Dispose()
		{
			this.Clear();
		}
		#endregion

		#region Constructors/Descructors
		/// <summary>
		/// Inicjalizuje nowy manager i dodaje zdarzenia dla wejścia.
		/// </summary>
		/// <param name="input">Wejście, które zostanie przypisane ekranom.</param>
		public ScreensManager(IInput input)
		{
			this.Input = input;
		}

		~ScreensManager()
		{
			this.Clear();
		}
		#endregion

		#region Private methods
		///// <summary>
		///// Metoda pomocnicza do wysyłania zdarzeń.
		///// </summary>
		///// <param name="e"></param>
		//private void FireEvent(Func<IScreen, bool> e)
		//{
		//    foreach (IScreen screen in this.Screens)
		//    {
		//        if (screen.State == ScreenState.Activated && e(screen))
		//        {
		//            break;
		//        }
		//    }
		//}

		/// <summary>
		/// Pobiera ekran albo rzuca wyjątek.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="Exceptions.NotFoundException">Nie znaleziono wskazanego ekranu.</exception>
		private IScreen GetOrThrow(string id)
		{
			var screen = this[id];
			if (screen == null)
			{
				throw new Exceptions.NotFoundException("screen");
			}
			return screen;
		}

		/// <summary>
		/// Sprawdza jaki stan powinien mieć ekran na danej pozycji.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		ScreenState CheckStateAt(int position)
		{
			ScreenState state = ScreenState.Activated;
			for (int i = 0; i < position; i++)
			{
				if (this.Screens[i].State != ScreenState.Deactivated)
				{
					if (this.Screens[i].Type == ScreenType.Normal)
					{
						state = ScreenState.Covered;
					}
					else if (this.Screens[i].Type == ScreenType.Fullscreen)
					{
						state = ScreenState.Hidden;
						break;
					}
				}
			}
			return state;
		}

		/// <summary>
		/// Zmienia stany ekranów w danym zakresie pobierając stan z pierwszego ekranu.
		/// </summary>
		/// <param name="startIdx"></param>
		/// <param name="endIdx"></param>
		void SetStates(int startIdx, int endIdx)
		{
			ScreenState state = this.Screens[startIdx].State;
			if (state == ScreenState.Deactivated)
			{
				state = ScreenState.Activated;
			}
			else if (this.Screens[startIdx].Type == ScreenType.Fullscreen)
			{
				state = ScreenState.Hidden;
			}
			else if (this.Screens[startIdx].Type == ScreenType.Normal)
			{
				state = ScreenState.Covered;
			}
			for (int i = startIdx + 1; i < endIdx; i++)
			{
				if (this.Screens[i].State != ScreenState.Deactivated)
				{
					this.Screens[i].State = state;
					if (this.Screens[i].Type == ScreenType.Fullscreen)
					{
						state = ScreenState.Hidden;
					}
					else if (state < ScreenState.Covered && this.Screens[i].Type == ScreenType.Normal)
					{
						state = ScreenState.Covered;
					}
				}
			}
		}

		/// <summary>
		/// Zmienia stany ekranów począwszy od wskazanego do końca pobierając z niego stan.
		/// </summary>
		/// <param name="startIdx"></param>
		void SetStates(int startIdx)
		{
			this.SetStates(startIdx, this.Screens.Count);
		}
		#endregion
	}
}
