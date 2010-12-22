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

		//TODO: wyrzucić to do jakiejś innej klasy wejścia
		/// <summary>
		/// Rozmiar okna.
		/// Nie jest to stricte związane z wejściem, ale jest wymagane, by poprawnie przekształcić pozycję myszki.
		/// </summary>
		Vector2 WindowSize { get; set; }

		/// <summary>
		/// "Transformacja" myszki. Służy skalowania pozycji myszki do, np., aktualnej kamery.
		/// </summary>
		System.Drawing.RectangleF MouseTransformation { get; set; }

		/// <summary>
		/// Pozycja myszki przekształcona przez <see cref="MouseTransformation"/>.
		/// </summary>
		Vector2 TransformedMousePosition { get; }

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
	}
}
