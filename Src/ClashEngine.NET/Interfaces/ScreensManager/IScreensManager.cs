using System;
using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.ScreensManager
{
	/// <summary>
	/// Bazowy interfejs dla managera ekranów.
	/// Przy dodawaniu ekranu musi ustawiać mu również właściwość Input.
	/// W kolekcji nie mogą znajdować się dwa ekrany o takim samym Id.
	/// </summary>
	public interface IScreensManager
		: IDisposable, ICollection<IScreen>
	{
		#region List management
		/// <summary>
		/// Dodaje ekran do listy i od razu go aktywuje.
		/// </summary>
		/// <param name="screen">Ekran do dodania.</param>
		void AddAndActivate(IScreen screen);

		/// <summary>
		/// Pobiera ekran o wskazanym Id.
		/// </summary>
		/// <param name="index">Identyfikator ekranu.</param>
		/// <returns>Ekran, bądź null, gdy nie znaleziono</returns>
		IScreen this[string id] { get; }
		#endregion

		#region Moving
		/// <summary>
		/// Przesuwa ekran we wskazane miejsce.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		/// <param name="newPos">Nowa pozycja na liście(licząc od końca!).</param>
		void MoveTo(IScreen screen, int newPos);

		/// <summary>
		/// Przesuwa ekran na wierzch.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		void MoveToFront(IScreen screen);

		/// <summary>
		/// Przesuwa ekran we wskazane miejsce.
		/// </summary>
		/// <param name="id">Identyfikator ekranu.</param>
		/// <param name="newPos">Nowa pozycja na liście(licząc od końca!).</param>
		void MoveTo(string id, int newPos);

		/// <summary>
		/// Przesuwa ekran na wierzch.
		/// </summary>
		/// <param name="id">Identyfikator ekranu.</param>
		void MoveToFront(string id);
		#endregion

		#region Changing state
		/// <summary>
		/// Aktywuje(jeśli może) wskazany ekran.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		void Activate(IScreen screen);

		/// <summary>
		/// Deaktywuje wskazany ekran.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		void Deactivate(IScreen screen);

		/// <summary>
		/// Aktywuje(jeśli może) wskazany ekran.
		/// </summary>
		/// <param name="id">Identyfikator ekranu.</param>
		void Activate(string id);

		/// <summary>
		/// Deaktywuje wskazany ekran.
		/// </summary>
		/// <param name="id">Identyfikator ekranu.</param>
		void Deactivate(string id);
		#endregion

		#region Rendering/updating
		/// <summary>
		/// Uaktualnia wszystkie ekrany, które powinny zostać uaktualnione(State == Activated).
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		void Update(double delta);

		/// <summary>
		/// Odrysowywuje wszystkie ekrany, które powinny być odrysowane.
		/// Renderowanie odbywa się od końca - ekran na początku listy jest odrysowywany na końcu.
		/// </summary>
		void Render();
		#endregion
	}
}
