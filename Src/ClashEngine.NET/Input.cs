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
		private KeyboardDevice _Keyboard;
		private MouseDevice _Mouse;
		private IList<JoystickDevice> _Joysticks;

		#region Singleton
		private static Input _Instance;

		/// <summary>
		/// Instancja.
		/// </summary>
		public static Input Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new Input();
				}
				return _Instance;
			}
		}
		#endregion

		#region IInput members
		/// <summary>
		/// Klawiatura.
		/// </summary>
		public KeyboardDevice Keyboard
		{
			get { return this._Keyboard; }
		}

		/// <summary>
		/// Mysz.
		/// </summary>
		public MouseDevice Mouse
		{
			get { return this._Mouse; }
		}

		/// <summary>
		/// Lista joysticków zainstalowanych w systemie.
		/// </summary>
		public IList<JoystickDevice> Joysticks
		{
			get { return this._Joysticks; }
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
			this._Keyboard = keyboard;
			this._Mouse = mouse;
			this._Joysticks = joysticks;
		}
		#endregion
	}
}
