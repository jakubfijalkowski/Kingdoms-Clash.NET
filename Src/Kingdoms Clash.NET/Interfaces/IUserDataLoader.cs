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
		/// Lista komponentów użytkownika.
		/// </summary>
		IList<Type> Components { get; }

		/// <summary>
		/// Załadowane nacje.
		/// Dostępne dopiero po wywołaniu LoadNations.
		/// </summary>
		IList<Units.INation> Nations { get; }

		/// <summary>
		/// Ładuje nacje z folderu Path/Nations.
		/// </summary>
		/// <returns>Lista nacji.</returns>
		IList<Units.INation> LoadNations();
	}
}
