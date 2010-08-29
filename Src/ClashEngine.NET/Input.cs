using System.Collections.Generic;
using OpenTK.Input;

namespace ClashEngine.NET
{
	using Interfaces;

	/// <summary>
	/// Klasa wejścia.
	/// </summary>
	public class Input
		: IInput
	{
		private KeyboardDevice Keyboard_;
		private MouseDevice Mouse_;
		private IList<JoystickDevice> Joysticks_;

		#region Singleton
		private static Input Instance_;

		/// <summary>
		/// Instancja.
		/// </summary>
		public static Input Instance
		{
			get
			{
				if (Instance_ == null)
				{
					Instance_ = new Input();
				}
				return Instance_;
			}
		}
		#endregion

		#region IInput members
		/// <summary>
		/// Klawiatura.
		/// </summary>
		public KeyboardDevice Keyboard
		{
			get { return this.Keyboard_; }
		}

		/// <summary>
		/// Mysz.
		/// </summary>
		public MouseDevice Mouse
		{
			get { return this.Mouse_; }
		}

		/// <summary>
		/// Lista joysticków zainstalowanych w systemie.
		/// </summary>
		public IList<JoystickDevice> Joysticks
		{
			get { return this.Joysticks_; }
		}
		#endregion

		#region Initialization
		/// <summary>
		/// Inicjalizuje klasę wejścia.
		/// Niestety, OpenTK nie udostępnia wejścia dostępnego z innych klas, więc musimy to zainicjalizować w obiekcie gry.
		/// </summary>
		/// <param name="keyboard"></param>
		/// <param name="mouse"></param>
		/// <param name="joysticks"></param>
		internal void Init(KeyboardDevice keyboard, MouseDevice mouse, IList<JoystickDevice> joysticks)
		{
			this.Keyboard_ = keyboard;
			this.Mouse_ = mouse;
			this.Joysticks_ = joysticks;
		}
		#endregion
	}
}
