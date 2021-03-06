﻿using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Interfaces
{
	/// <summary>
	/// Interfejs dla loadera danych użytkownika.
	/// </summary>
	public interface IUserDataLoader
	{
		/// <summary>
		/// Ścieżka do głównego folderu z danymi.
		/// </summary>
		string RootPath { get; }

		/// <summary>
		/// Ścieżka do folderu z danymi użytkownika.
		/// </summary>
		string UserDataPath { get; }

		/// <summary>
		/// Załadowane nacje.
		/// Dostępne dopiero po wywołaniu LoadNations.
		/// </summary>
		List<Units.INation> Nations { get; }

		/// <summary>
		/// Sumy kontrolne nacji.
		/// </summary>
		IList<byte[]> NationsCheckSums { get; }

		/// <summary>
		/// Ładuje nacje z folderu Path/Nations.
		/// </summary>
		void LoadNations();

		/// <summary>
		/// Ładuje konfigurację.
		/// </summary>
		void LoadConfiguration();

		/// <summary>
		/// Ładuje zasoby.
		/// </summary>
		void LoadResources();
	}
}
