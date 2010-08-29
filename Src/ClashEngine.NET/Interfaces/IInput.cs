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
		#endregion
	}
}
