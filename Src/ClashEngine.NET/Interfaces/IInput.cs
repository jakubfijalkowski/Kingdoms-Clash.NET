using System.Collections.Generic;
using OpenTK.Input;

namespace ClashEngine.NET.Interfaces
{
	/// <summary>
	/// Bazowy interfejs dla wejścia.
	/// </summary>
	public interface IInput
	{
		#region Properties
		/// <summary>
		/// Klawiatura.
		/// </summary>
		KeyboardDevice Keyboard { get; }

		/// <summary>
		/// Mysz.
		/// </summary>
		MouseDevice Mouse { get; }

		/// <summary>
		/// Lista joysticków zainstalowanych w systemie.
		/// </summary>
		IList<JoystickDevice> Joysticks { get; }
		#endregion
	}
}
