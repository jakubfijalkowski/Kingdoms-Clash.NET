﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClashEngine.NET.ScreensManager
{
	/// <summary>
	/// Manager ekranów.
	/// </summary>
	public class ScreensManager
	{
		private List<Screen> _Screens = new List<Screen>();

		#region Properties
		/// <summary>
		/// Lista ekranów w managerze.
		/// Bardziej przypomina stos/kolejkę FIFO(pierwszy ekran na liście jest pierwszym "w rzeczywistości").
		/// </summary>
		public ReadOnlyCollection<Screen> Screens
		{
			get { return this._Screens.AsReadOnly(); }
		}
		#endregion

		#region Methods
		#region List management
		/// <summary>
		/// Dodaje ekran do listy.
		/// </summary>
		/// <param name="screen">Ekran do dodania.</param>
		public void AddScreen(Screen screen)
		{
			if (screen == null)
			{
				throw new ArgumentNullException("screen");
			}
			else if (this._Screens.Contains(screen))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("screen");
			}
			this._Screens.Add(screen);
		}

		/// <summary>
		/// Usuwa ekran z managera.
		/// </summary>
		/// <param name="screen">Ekran do usunięcia.</param>
		public void RemoveScreen(Screen screen)
		{
			if (screen == null)
			{
				throw new ArgumentNullException("screen");
			}
			else if (!this._Screens.Contains(screen))
			{
				throw new Exceptions.ArgumentNotExistsException("screen");
			}
			this._Screens.Remove(screen);
		}
		#endregion

		#region Moving
		/// <summary>
		/// Przesuwa ekran na wierzch.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		public void MoveToFront(Screen screen)
		{
			this.MoveTo(screen, 0);
		}

		/// <summary>
		/// Przesuwa ekran we wskazane miejsce.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		/// <param name="newPos">Nowa pozycja na liście(licząc od końca!).</param>
		public void MoveTo(Screen screen, int newPos)
		{
			if (screen == null)
			{
				throw new ArgumentNullException("screen");
			}
			else if (!this._Screens.Contains(screen))
			{
				throw new Exceptions.ArgumentNotExistsException("screen");
			}
			int position = this._Screens.IndexOf(screen);
			if (position > newPos && newPos > 0)
			{
				newPos -= 1;
			}
			this._Screens.RemoveAt(position);
			this._Screens.Insert((newPos > 0 ? newPos - 1 : newPos), screen);
		}
		#endregion

		#region Changing state
		/// <summary>
		/// Zmienia ekran wskazany ekran na aktywny(tylko jeśli nie zasłania go nic innego).
		/// </summary>
		/// <param name="screen">Ekran.</param>
		/// <returns>Czy zmieniono stan ekranu.</returns>
		public bool MakeActive(Screen screen)
		{
			if (screen == null)
			{
				throw new ArgumentNullException("screen");
			}
			else if (!this._Screens.Contains(screen))
			{
				throw new Exceptions.ArgumentNotExistsException("screen");
			}
			//Sprawdzamy, czy nic nie zasłania ekranu.
			for (int i = this._Screens.IndexOf(screen); i > 0; i--)
			{
				if (this._Screens[i].State != ScreenState.Closed && !this._Screens[i].IsPopup)
				{
					return false;
				}
			}
			//Pełnoekranowy ekran(masło maślane...), więc musimy aktywować ekrany "za".
			if (screen.IsFullscreen)
			{
				for (int i = this._Screens.IndexOf(screen); i < this._Screens.Count; i++)
				{
					if (this._Screens[i].State == ScreenState.Active)
					{
						this._Screens[i].State = ScreenState.Inactive;
						this._Screens[i].StateChanged();
						if (this._Screens[i].IsFullscreen) //Pełnoekranowy, i tak dalej nic nie będzie widać.
						{
							break;
						}
					}
				}
			}
			screen.State = ScreenState.Active;
			screen.StateChanged();
			return true;
		}

		/// <summary>
		/// Zmienia ekran na nieaktywny.
		/// Ekran musi być aktywny by stać się nieaktywnym.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		/// <returns>Czy zmieniono stan ekranu.</returns>
		public bool MakeInactive(Screen screen)
		{
			if (screen == null)
			{
				throw new ArgumentNullException("screen");
			}
			else if (!this._Screens.Contains(screen))
			{
				throw new Exceptions.ArgumentNotExistsException("screen");
			}
			if (screen.State != ScreenState.Active)
			{
				return false;
			}
			//Jeśli jest na pełnym ekranie musimy aktywować ekrany które są za nim.
			if (screen.IsFullscreen)
			{
				for (int i = this._Screens.IndexOf(screen); i < this._Screens.Count; i++)
				{
					if (this._Screens[i].State == ScreenState.Inactive)
					{
						this._Screens[i].State = ScreenState.Active;
						this._Screens[i].StateChanged();
						if (!this._Screens[i].IsPopup) //Pełnoekranowy, i tak dalej nic nie będzie widać.
						{
							break;
						}
					}
				}
			}

			screen.State = ScreenState.Inactive;
			screen.StateChanged();
			return true;
		}

		/// <summary>
		/// Zamyka ekran.
		/// Przed zamknięciem ekran jest dezaktywowany.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		public void Close(Screen screen)
		{
			if (screen == null)
			{
				throw new ArgumentNullException("screen");
			}
			else if (!this._Screens.Contains(screen))
			{
				throw new Exceptions.ArgumentNotExistsException("screen");
			}
			if (screen.State == ScreenState.Active)
			{
				this.MakeInactive(screen);
			}
			if (screen.State != ScreenState.Closed)
			{
				screen.State = ScreenState.Closed;
				screen.StateChanged();
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
			foreach (Screen screen in this._Screens)
			{
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
			//Szukamy pierwszego pełnoekranowego ekranu(masło maślane x2...), który nie jest zamknięty.
			int firstFullscreen = 0;
			for(; firstFullscreen < this._Screens.Count
				&& !(this._Screens[firstFullscreen].IsFullscreen && this._Screens[firstFullscreen].State != ScreenState.Closed);
				++firstFullscreen);

			for (; firstFullscreen >= 0; --firstFullscreen)
			{
				this._Screens[firstFullscreen].Render();
			}
		}
		#endregion
		#endregion
	}
}