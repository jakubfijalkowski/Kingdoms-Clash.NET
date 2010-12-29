using System;
using OpenTK;
using OpenTK.Input;

namespace ClashEngine.NET.Internals
{
	using Interfaces;

	/// <summary>
	/// Klasa wejścia obsługująca IWindow.
	/// </summary>
	internal class WindowInput
		: IInput
	{
		#region Private fields
		private bool[] KeyStates = new bool[(int)Key.LastKey];
		private bool[] ButtonStates = new bool[(int)OpenTK.Input.MouseButton.LastButton];
		#endregion

		#region IInput Members
		#region Keyboard
		/// <summary>
		/// Pobiera stan danego klawisza.
		/// </summary>
		/// <param name="index">Indeks klawisza.</param>
		/// <returns>Czy został wciśnięty.</returns>
		public bool this[Key index]
		{
			get { return this.KeyStates[(int)index]; }
			private set { this.KeyStates[(int)index] = value; }
		}

		/// <summary>
		/// Ostatni znak wprowadzony przez użytkownika.
		/// 0, gdy nie wprowadzono nic.
		/// </summary>
		public char LastCharacter { get; set; }
		#endregion

		#region Mouse
		/// <summary>
		/// Pozycja myszki.
		/// </summary>
		public Vector2 MousePosition { get; private set; }

		/// <summary>
		/// Pobiera stan danego przycisku.
		/// </summary>
		/// <param name="index">Przycisk.</param>
		/// <returns>Czy jest wciśnięty</returns>
		public bool this[MouseButton index]
		{
			get { return this.ButtonStates[(int)index]; }
			private set { this.ButtonStates[(int)index] = value; }
		}

		/// <summary>
		/// Położenie kółka myszki.
		/// </summary>
		public float Wheel { get; private set; }
		#endregion

		#region Events
		/// <summary>
		/// Zdarzenie klawiatury - albo naciśnięcie, albo zwolnienie klawisza.
		/// </summary>
		public event EventHandler<KeyEventArgs> KeyChanged;

		/// <summary>
		/// Zdarzenie naciśnięca/zwolnienia przycisku myszy.
		/// </summary>
		public event EventHandler<MouseButtonEventArgs> MouseButton;

		/// <summary>
		/// Zdarzenie zmiany położenia myszki.
		/// </summary>
		public event EventHandler<MouseMoveEventArgs> MouseMove;

		/// <summary>
		/// Zdarzenie zmiany kółka myszy.
		/// </summary>
		public event EventHandler<MouseWheelEventArgs> MouseWheel;
		#endregion
		#endregion

		#region Constructors
		internal WindowInput(IWindow wnd)
		{
			#pragma warning disable 0612
			wnd.InputDriver.Mouse[0].Move         += new EventHandler<MouseMoveEventArgs>(Window_MouseMove);
			wnd.InputDriver.Mouse[0].ButtonDown   += new EventHandler<MouseButtonEventArgs>(Window_ButtonEvent);
			wnd.InputDriver.Mouse[0].ButtonUp     += new EventHandler<MouseButtonEventArgs>(Window_ButtonEvent);
			wnd.InputDriver.Mouse[0].WheelChanged += new EventHandler<MouseWheelEventArgs>(Window_WheelChanged);

			wnd.InputDriver.Keyboard[0].KeyDown   += new EventHandler<KeyboardKeyEventArgs>(Window_KeyDown);
			wnd.InputDriver.Keyboard[0].KeyUp     += new EventHandler<KeyboardKeyEventArgs>(Window_KeyUp);
			wnd.KeyPress                          += new EventHandler<OpenTK.KeyPressEventArgs>(Window_Text);
			#pragma warning restore 0612
		}
		#endregion

		#region Events
		void Window_MouseMove(object sender, MouseMoveEventArgs e)
		{
			this.MousePosition = new Vector2(e.X, e.Y);

			if (this.MouseMove != null)
			{
				this.MouseMove(this, e);
			}
		}

		void Window_ButtonEvent(object sender, MouseButtonEventArgs e)
		{
			this[e.Button] = e.IsPressed;

			if (this.MouseButton != null)
			{
				this.MouseButton(this, e);
			}
		}

		void Window_WheelChanged(object sender, MouseWheelEventArgs e)
		{
			this.Wheel = e.ValuePrecise;

			if (this.MouseWheel != null)
			{
				this.MouseWheel(this, e);
			}
		}

		void Window_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			this[e.Key] = true;

			if (this.KeyChanged != null)
			{
				this.KeyChanged(this, new KeyEventArgs(e.Key, true));
			}
		}

		void Window_KeyUp(object sender, KeyboardKeyEventArgs e)
		{
			this[e.Key] = false;

			if (this.KeyChanged != null)
			{
				this.KeyChanged(this, new KeyEventArgs(e.Key, false));
			}
		}

		void Window_Text(object sender, OpenTK.KeyPressEventArgs e)
		{
			this.LastCharacter = e.KeyChar;
		}
		#endregion
	}
}
