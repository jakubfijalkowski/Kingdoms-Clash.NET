using System;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	/// <summary>
	/// Token rządania o wytworzenie jednostki.
	/// </summary>
	public interface IUnitRequestToken
	{
		/// <summary>
		/// Czas, w sekundach, do stworzenia jednostki.
		/// </summary>
		float TimeLeft { get; }

		/// <summary>
		/// Procent ukończenia jednostki.
		/// Wartość jest z zakresu 0..1.
		/// </summary>
		float Percentage { get; }

		/// <summary>
		/// Czy token jest poprawny i nadal można go używać.
		/// </summary>
		bool IsValidToken { get; }

		/// <summary>
		/// Czy ukończono już jednostkę.
		/// </summary>
		bool IsCompleted { get; }

		/// <summary>
		/// Czy tworzenie jednostki jest spauzowane.
		/// </summary>
		bool IsPaused { get; }

		/// <summary>
		/// Jednostka, która zostanie stworzona.
		/// </summary>
		IUnitDescription Unit { get; }

		/// <summary>
		/// Właściciel jednostki.
		/// </summary>
		Player.IPlayer Owner { get; }

		/// <summary>
		/// Zdarzenie wywoływane przy stworzeniu jednostki.
		/// </summary>
		event EventHandler UnitCreated;
		
		/// <summary>
		/// Pauzuje tworzenie jednostki.
		/// </summary>
		/// <remarks>
		///	Wykonuje się tylko jeśli IsPaused = true.
		/// </remarks>
		void Pause();

		/// <summary>
		/// Wznawia tworzenie jednostki.
		/// </summary>
		/// <remarks>
		///	Wykonuje się tylko jeśli IsPaused = false.
		/// </remarks>
		void Resume();

		/// <summary>
		/// Anuluje tworzenie jednostki.
		/// </summary>
		void Abort();
	}
}
