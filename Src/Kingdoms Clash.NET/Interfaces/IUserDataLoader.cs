using System;
using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Interfaces
{
	/// <summary>
	/// Interfejs dla loadera danych użytkownika.
	/// </summary>
	public interface IUserDataLoader
	{
		/// <summary>
		/// Ścieżka do głównego folderu z danymi użytkownika.
		/// </summary>
		string Path { get; }

		/// <summary>
		/// Ścieżka do pliku konfiguracyjnego.
		/// </summary>
		string ConfigurationFile { get; }

		/// <summary>
		/// Załadowane nacje.
		/// Dostępne dopiero po wywołaniu LoadNations.
		/// </summary>
		IList<Units.INation> Nations { get; }

		/// <summary>
		/// Ładuje nacje z folderu Path/Nations.
		/// </summary>
		void LoadNations();

		/// <summary>
		/// Ładuje konfigurację.
		/// </summary>
		void LoadConfiguration();
	}
}
