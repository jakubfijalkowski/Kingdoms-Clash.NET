using System;

namespace Kingdoms_Clash.NET.Interfaces.Controllers
{
	/// <summary>
	/// Atrybut dla klasy kontrolera określający klasę z ustawieniami
	/// </summary>
	/// <remarks>
	/// Typ zawsze dziedziczy z IGameplaySettings i posiada domyślny konstruktor.
	/// </remarks>
	public interface IControllerSettingsAttribute
	{
		/// <summary>
		/// Typ klasy z ustawieniami.
		/// </summary>
		Type SettingsType { get; }
	}
}
