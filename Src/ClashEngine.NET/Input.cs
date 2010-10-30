using System;
using OpenTK.Input;

namespace ClashEngine.NET
{
	using Interfaces;
	using OpenTK;

	/// <summary>
	/// Klasa wejścia.
	/// </summary>
	public class Input
		: IInput
	{
		private bool[] KeyStates = new bool[(int)Key.LastKey];
		private bool[] ButtonStates = new bool[(int)OpenTK.Input.MouseButton.LastButton];
		private System.Drawing.RectangleF _MouseTransformation = System.Drawing.RectangleF.Empty;
		private Vector2 _MousePosition = Vector2.Zero;

		#region Singleton
		private static Input _Instance;

		/// <summary>
		/// Instancja.
		/// </summary>
		public static Input Instance
		{
			get
			{
				return _Instance;
			}
		}
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
		public Vector2 MousePosition
		{
			get
			{
				return this._MousePosition;
			}
			private set
			{
				this._MousePosition = value;
				this.TransformMousePosition();
			}
		}

		/// <summary>
		/// "Transformacja" myszki. Służy skalowania pozycji myszki do, np., aktualnej kamery.
		/// </summary>
		public System.Drawing.RectangleF MouseTransformation
		{
			get
			{
				return this._MouseTransformation;
			}
			set
			{
				this._MouseTransformation = value;
				this.TransformMousePosition();
			}
		}

		/// <summary>
		/// Rozmiar okna.
		/// Nie jest to stricte związane z wejściem, ale jest wymagane, by poprawnie przekształcić pozycję myszki.
		/// </summary>
		public Vector2 WindowSize { get; set; }

		/// <summary>
		/// Pozycja myszki przekształcona przez <see cref="MouseTransformation"/>.
		/// </summary>
		public Vector2 TransformedMousePosition { get; private set; }

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
		internal Input(OpenTK.GameWindow wnd, bool isMainInput)
		{
			wnd.Mouse.Move += new EventHandler<MouseMoveEventArgs>(Window_MouseMove);
			wnd.Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(Window_ButtonEvent);
			wnd.Mouse.ButtonUp += new EventHandler<MouseButtonEventArgs>(Window_ButtonEvent);
			wnd.Mouse.WheelChanged += new EventHandler<MouseWheelEventArgs>(Window_WheelChanged);

			wnd.Keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(Window_KeyDown);
			wnd.Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(Window_KeyUp);
			wnd.KeyPress += new EventHandler<OpenTK.KeyPressEventArgs>(Window_Text);

			this.WindowSize = new Vector2(wnd.Size.Width, wnd.Size.Height);

			if (isMainInput)
			{
				_Instance = this;
			}
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

		#region Private methods
		/// <summary>
		/// Przekształca pozycje myszki.
		/// </summary>
		private void TransformMousePosition()
		{
			if (!this.MouseTransformation.IsEmpty)
			{
				this.MousePosition = new Vector2(
					this.MouseTransformation.Left + this.MousePosition.X / this.WindowSize.X * this.MouseTransformation.Width,
					this.MouseTransformation.Top + this.MousePosition.Y / this.WindowSize.Y * this.MouseTransformation.Height);
			}
			else
			{
				this.TransformedMousePosition = this.MousePosition;
			}
		}
		#endregion
	}
}
