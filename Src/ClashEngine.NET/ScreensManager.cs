using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClashEngine.NET
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
		/// Bardziej przypomina stos/kolejkę LIFO(ostatni ekran na liście jest pierwszym "w rzeczywistości").
		/// </summary>
		public ReadOnlyCollection<Screen> Screens
		{
			get { return this._Screens.AsReadOnly(); }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Dodaje ekran do listy.
		/// </summary>
		/// <param name="screen">Ekran do dodania.</param>
		public void AddScreen(Screen screen)
		{ }

		/// <summary>
		/// Usuwa ekran z managera.
		/// </summary>
		/// <param name="screen">Ekran do usunięcia.</param>
		public void RemoveScreen(Screen screen)
		{ }

		/// <summary>
		/// Przesówa ekran na wierzch.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		public void MoveToFront(Screen screen)
		{ }

		/// <summary>
		/// Przesówa ekran we wskazane miejsce.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		/// <param name="position">Pozycja w liście(licząc od końca!).</param>
		public void MoveTo(Screen screen, int position)
		{ }

		/// <summary>
		/// Zmienia ekran wskazany ekran na aktywny.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		public void MakeActive(Screen screen)
		{ }

		/// <summary>
		/// Zmienia ekran na nieaktywny.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		public void MakeInactive(Screen screen)
		{ }

		/// <summary>
		/// Zamyka ekran.
		/// </summary>
		/// <param name="screen">Ekran.</param>
		public void Close(Screen screen)
		{ }
		#endregion
	}
}
