using System;
using System.Linq;

namespace Kingdoms_Clash.NET.Controllers
{
	using Interfaces;
	using Interfaces.Controllers;

	/// <summary>
	/// Atrybut dla klasy kontrolera określający klasę z ustawieniami
	/// </summary>
	/// <remarks>
	/// Typ zawsze dziedziczy z IGameplaySettings i posiada domyślny konstruktor.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class ControllerSettingsAttribute
		: Attribute, IControllerSettingsAttribute
	{
		#region IControllerSettingsAttribute Members
		/// <summary>
		/// Typ klasy z ustawieniami.
		/// </summary>
		public Type SettingsType { get; private set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje atrybut.
		/// </summary>
		/// <param name="settingsType"></param>
		public ControllerSettingsAttribute(Type settingsType)
		{
			if (settingsType == null)
			{
				throw new ArgumentNullException("settingsType");
			}
			else if(!settingsType.GetInterfaces().Any(t => t == typeof(Interfaces.IGameplaySettings)))
			{
				throw new ArgumentException("Must implement IGameplaySettings", "settingsType");
			}
			else if (settingsType.GetConstructor(Type.EmptyTypes) == null)
			{
				throw new ArgumentException("Must have default constructor", "settingsType");
			}
			this.SettingsType = settingsType;
		}
		#endregion

		#region Static utilities
		/// <summary>
		/// Pobiera nowy obiekt ustawień dla kontrolera.
		/// </summary>
		/// <remarks>
		/// Jeśli kontroler nie definiuje tego atrybutu zwracany jest obiekt <see cref="DefaultGameplaySettings"/>.
		/// </remarks>
		/// <param name="controller">Kontroler.</param>
		/// <returns>Nowy obiekt ustawień dla klasy.</returns>
		public static IGameplaySettings GetSettingsFor(IGameController controller)
		{
			var attributes = controller.GetType().GetCustomAttributes(typeof(ControllerSettingsAttribute), false);
			if (attributes.Length > 0)
			{
				return Activator.CreateInstance(((ControllerSettingsAttribute)attributes[0]).SettingsType) as IGameplaySettings;
			}
			return new DefaultGameplaySettings();
		}
		#endregion
	}
}
