using System;
using OpenTK;
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
		#region Private fields
		private bool[] KeyStates = new bool[(int)Key.LastKey];
		private bool[] ButtonStates = new bool[(int)OpenTK.Input.MouseButton.LastButton];
		#endregion

		#region IInput Members
		/// <summary>
		/// Okno(gra), do którego wejście sie odnosi.
		/// </summary>
		public Game Owner { get; private set; }

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
		internal Input(Game g, OpenTK.GameWindow wnd)
		{
			wnd.Mouse.Move += new EventHandler<MouseMoveEventArgs>(Window_MouseMove);
			wnd.Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(Window_ButtonEvent);
			wnd.Mouse.ButtonUp += new EventHandler<MouseButtonEventArgs>(Window_ButtonEvent);
			wnd.Mouse.WheelChanged += new EventHandler<MouseWheelEventArgs>(Window_WheelChanged);

			wnd.Keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(Window_KeyDown);
			wnd.Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(Window_KeyUp);
			wnd.KeyPress += new EventHandler<OpenTK.KeyPressEventArgs>(Window_Text);

			this.Owner = g;
			//this.WindowSize = new Vector2(wnd.Width, wnd.Height);
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
		
		#region Methods
		/// <summary>
		/// Skaluje pozycję myszki do perpektywy kamery.
		/// </summary>
		/// <param name="camera"></param>
		/// <returns></returns>
		public Vector2 TransformMousePosition(Interfaces.Graphics.ICamera camera)
		{
			if (camera == null)
			{
				return this.MousePosition;
			}
			float x = this.MousePosition.X / this.Owner.Size.X * camera.Size.X;
			float y = this.MousePosition.Y / this.Owner.Size.Y * camera.Size.Y;
			if (camera is Interfaces.Graphics.Cameras.IMovable2DCamera)
			{
				Interfaces.Graphics.Cameras.IMovable2DCamera cam = camera as Interfaces.Graphics.Cameras.IMovable2DCamera;
				x += cam.CurrentPosition.X;
				y += cam.CurrentPosition.Y;
			}
			return new Vector2(x, y);
		}
		#endregion
	}
}
