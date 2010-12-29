using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ClashEngine.NET
{
	using Interfaces;
	using Interfaces.Graphics;

	/// <summary>
	/// Manager ekranów.
	/// Zobacz <see cref="Screen"/> dla większej ilości informacji.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	public class ScreensManager
		: KeyedCollection<string, IScreen>, IScreensManager
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");

		#region Private fields
		private IGameInfo GameInfo = null;
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
			int idx = base.Items.IndexOf(screen);
			if (idx == -1)
			{
				throw new Exceptions.ArgumentNotExistsException("screen");
			}
			base.Items.RemoveAt(idx);
			if (screen.State == ScreenState.Activated ||
				(screen.State < ScreenState.Hidden && screen.Type == ScreenType.Fullscreen)) //Tylko, gdy ekran był aktywny coś się może zmienić
			{
				this.SetStates(0, idx);
			}

			base.Items.Insert(newPos, screen);
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
			int idx = base.Items.IndexOf(screen);
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
			int idx = base.Items.IndexOf(screen);
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
			for (int i = 0; i < base.Items.Count; i++)
			{
				if (base.Items[i].State == ScreenState.Deactivated)
				{
					continue;
				}
				else if (base.Items[i].State >= ScreenState.Covered)
				{
					break;
				}
				base.Items[i].Update(delta);
			}
		}

		/// <summary>
		/// Odrysowywuje wszystkie ekrany, które powinny być odrysowane.
		/// Renderowanie odbywa się od końca - ekran na początku listy jest odrysowywany na końcu.
		/// </summary>
		public void Render()
		{
			int last = 0;
			for (int i = this.Items.Count - 1; i >= 0; i--)
			{
				if (base[i].State <= ScreenState.Covered)
				{
					last = i;
					break;
				}
			}

			for (int i = last; i >= 0; i--)
			{
				if (base.Items[i].State != ScreenState.Deactivated)
				{
					base.Items[i].Render();
					base.Items[i].GameInfo.Renderer.Flush();
				}
			}
		}
		#endregion

		#region KeyedCollection Members
		protected override string GetKeyForItem(IScreen item)
		{
			if (string.IsNullOrWhiteSpace(item.Id))
			{
				throw new ArgumentNullException("item.Id");
			}
			return item.Id;
		}

		protected override void InsertItem(int index, IScreen item)
		{
			item.OwnerManager = this;
			item.GameInfo = this.GameInfo;
			item.State = ScreenState.Deactivated;
			item.OnInit();
			Logger.Debug("Screen {0} added to manager", item.Id);
			base.InsertItem(index, item);
		}

		protected override void RemoveItem(int index)
		{
			var s = base[index];
			s.OnDeinit();
			Logger.Debug("Screen {0} removed from manager", s.Id);
			base.RemoveItem(index);
		}

		protected override void ClearItems()
		{
			foreach (var item in this)
			{
				item.OnDeinit();
			}
			base.ClearItems();
		}
		#endregion

		#region Constructors/Descructors
		/// <summary>
		/// Inicjalizuje nowy manager i dodaje zdarzenia dla wejścia.
		/// </summary>
		/// <param name="input">Wejście, które zostanie przypisane ekranom.</param>
		/// <param name="content">Manager zasobów.</param>
		/// <param name="renderer">Renderer.</param>
		public ScreensManager(IGameInfo gameInfo)
		{
			this.GameInfo = gameInfo;
		}

		~ScreensManager()
		{
			this.Dispose();
		}
		#endregion

		#region Private methods
		///// <summary>
		///// Metoda pomocnicza do wysyłania zdarzeń.
		///// </summary>
		///// <param name="e"></param>
		//private void FireEvent(Func<IScreen, bool> e)
		//{
		//    foreach (IScreen screen in base.Items)
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
				if (base.Items[i].State != ScreenState.Deactivated)
				{
					if (base.Items[i].Type == ScreenType.Normal)
					{
						state = ScreenState.Covered;
					}
					else if (base.Items[i].Type == ScreenType.Fullscreen)
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
			ScreenState state = base.Items[startIdx].State;
			if (state == ScreenState.Deactivated)
			{
				state = ScreenState.Activated;
			}
			else if (base.Items[startIdx].Type == ScreenType.Fullscreen)
			{
				state = ScreenState.Hidden;
			}
			else if (base.Items[startIdx].Type == ScreenType.Normal)
			{
				state = ScreenState.Covered;
			}
			for (int i = startIdx + 1; i < endIdx; i++)
			{
				if (base.Items[i].State != ScreenState.Deactivated)
				{
					base.Items[i].State = state;
					if (base.Items[i].Type == ScreenType.Fullscreen)
					{
						state = ScreenState.Hidden;
					}
					else if (state < ScreenState.Covered && base.Items[i].Type == ScreenType.Normal)
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
			this.SetStates(startIdx, base.Items.Count);
		}
		#endregion

		#region IDisposable members
		public void Dispose()
		{
			this.Clear();
		}
		#endregion
	}
}
