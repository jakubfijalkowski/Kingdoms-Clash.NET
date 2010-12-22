using System;
using OpenTK;
using OpenTK.Input;

namespace ClashEngine.NET.Interfaces
{
	#region Event data classes
	/// <summary>
	/// Dane do zdarzenia klawiatury.
	/// </summary>
	public class KeyEventArgs
		: EventArgs
	{
		/// <summary>
		/// Klawisz.
		/// </summary>
		public Key Key { get; private set; }
		
		/// <summary>
		/// Czy jest wciśnięty, czy puszczony.
		/// </summary>
		public bool IsPressed { get; private set; }

		public KeyEventArgs(Key key, bool isPressed)
		{
			this.Key = key;
			this.IsPressed = isPressed;
		}
	}
	#endregion
	
	/// <summary>
	/// Bazowy interfejs dla wejścia.
	/// </summary>
	public interface IInput
	{
		/// <summary>
		/// Okno(gra), do którego wejście sie odnosi.
		/// </summary>
		Game Owner { get; }

		#region Keyboard
		/// <summary>
		/// Pobiera stan danego klawisza.
		/// </summary>
		/// <param name="index">Indeks klawisza.</param>
		/// <returns>Czy został wciśnięty.</returns>
		bool this[Key index] { get; }

		/// <summary>
		/// Ostatni znak wprowadzony przez użytkownika.
		/// 0, gdy nie wprowadzono nic.
		/// </summary>
		char LastCharacter { get; set; }
		#endregion

		#region Mouse
		/// <summary>
		/// Pozycja myszki.
		/// </summary>
		Vector2 MousePosition { get; }

		/// <summary>
		/// Pobiera stan danego przycisku.
		/// </summary>
		/// <param name="index">Przycisk.</param>
		/// <returns>Czy jest wciśnięty</returns>
		bool this[MouseButton index] { get; }

		/// <summary>
		/// Położenie kółka myszki.
		/// </summary>
		float Wheel { get; }
		#endregion

		#region Events
		/// <summary>
		/// Zdarzenie klawiatury - albo naciśnięcie, albo zwolnienie klawisza.
		/// </summary>
		event EventHandler<KeyEventArgs> KeyChanged;

		/// <summary>
		/// Zdarzenie naciśnięca/zwolnienia przycisku myszy.
		/// </summary>
		event EventHandler<MouseButtonEventArgs> MouseButton;

		/// <summary>
		/// Zdarzenie zmiany położenia myszki.
		/// </summary>
		event EventHandler<MouseMoveEventArgs> MouseMove;

		/// <summary>
		/// Zdarzenie zmiany kółka myszy.
		/// </summary>
		event EventHandler<MouseWheelEventArgs> MouseWheel;
		#endregion

		#region Methods
		/// <summary>
		/// Skaluje pozycję myszki do perpektywy kamery.
		/// </summary>
		/// <remarks>
		/// Powinno obsługiwać podawanie nulla za kamerę.
		/// </remarks>
		/// <param name="camera">Kamera.</param>
		/// <returns>Przeskalowana pozycja myszki.</returns>
		Vector2 TransformMousePosition(Graphics.ICamera camera);
		#endregion
	}
}
