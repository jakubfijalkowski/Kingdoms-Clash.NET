﻿using System.Collections.ObjectModel;

namespace ClashEngine.NET.Interfaces.ScreensManager
{
	public interface IScreensManager
	{
		#region Properties
		/// <summary>
		/// Lista ekranów w managerze.
		/// Bardziej przypomina stos/kolejkę FIFO(pierwszy ekran na liście jest pierwszym "w rzeczywistości").
		/// </summary>
		ReadOnlyCollection<IScreen> Screens
		{
			get;
		}
		#endregion

		#region Methods
		#region List management
		/// <summary>
		/// Dodaje ekran do listy.
		/// </summary>
		/// <param name="screen">Ekran do dodania.</param>
		void AddScreen(IScreen screen);

		/// <summary>
		/// Usuwa ekran z managera.
		/// </summary>
		/// <param name="screen">Ekran do usunięcia.</param>
		void RemoveScreen(IScreen screen);
		#endregion

		#region Moving
		/// <summary>
		/// Przesuwa ekran na wierzch.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		void MoveToFront(IScreen screen);

		/// <summary>
		/// Przesuwa ekran we wskazane miejsce.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		/// <param name="newPos">Nowa pozycja na liście(licząc od końca!).</param>
		void MoveTo(IScreen screen, int newPos);
		#endregion

		#region Changing state
		/// <summary>
		/// Zmienia ekran wskazany ekran na aktywny(tylko jeśli nie zasłania go nic innego).
		/// </summary>
		/// <param name="screen">Ekran.</param>
		/// <returns>Czy zmieniono stan ekranu.</returns>
		bool MakeActive(IScreen screen);

		/// <summary>
		/// Zmienia ekran na nieaktywny.
		/// Ekran musi być aktywny by stać się nieaktywnym.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		/// <returns>Czy zmieniono stan ekranu.</returns>
		bool MakeInactive(IScreen screen);

		/// <summary>
		/// Zamyka ekran.
		/// Przed zamknięciem ekran jest dezaktywowany.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		void Close(IScreen screen);
		#endregion

		#region Rendering/updating
		/// <summary>
		/// Uaktualnia wszystkie ekrany, które powinny zostać uaktualnione(State == Active).
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		void Update(double delta);

		/// <summary>
		/// Odrysowywuje wszystkie ekrany, które powinny być odrysowane.
		/// Renderowanie odbywa się od końca - ekran na początku listy jest odrysowywany na końcu.
		/// </summary>
		void Render();
		#endregion
		#endregion
	}
}