using System;

namespace ClashEngine.NET.Interfaces
{
	/// <summary>
	/// Bazowy interfejs dla obiektu gry.
	/// </summary>
	public interface IGame
		: IDisposable
	{
		/// <summary>
		/// Informacje o grze.
		/// </summary>
		IGameInfo Info { get; }

		/// <summary>
		/// Inicjalizacja gry.
		/// </summary>
		void OnInit();

		/// <summary>
		/// Deinicjalizacja gry.
		/// </summary>
		void OnDeinit();

		/// <summary>
		/// Metoda do uaktualnień.
		/// </summary>
		/// <param name="delta">Czas od ostatniego uaktualnienia.</param>
		void OnUpdate(double delta);

		/// <summary>
		/// Odrysowywanie.
		/// </summary>
		void OnRender();

		/// <summary>
		/// Uruchamia grę z maksymalną wydajnością.
		/// </summary>
		void Run();

		/// <summary>
		/// Zamyka grę.
		/// </summary>
		void Exit();
	}
}
