using OpenTK.Input;

namespace ClashEngine.NET.Gui
{
	using Interfaces;
	using Interfaces.Gui;
	using Interfaces.ScreensManager;

	/// <summary>
	/// Ekran jako kontener na kontrolki(i nic więcej).
	/// </summary>
	public abstract class GuiScreen
		: GuiContainer, IGuiScreen
	{
		private ScreenState _State = ScreenState.Deactivated;

		#region IScreen Members
		#region Properties
		/// <summary>
		/// Identyfikator ekranu.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Manager - rodzic.
		/// </summary>
		public IScreensManager Manager { get; set; }

		/// <summary>
		/// Typ ekranu.
		/// </summary>
		public ScreenType Type { get; private set; }

		/// <summary>
		/// Aktualny stan ekranu.
		/// </summary>
		public ScreenState State
		{
			get { return this._State; }
			set
			{
				if (value != this._State)
				{
					var oldState = this._State;
					this._State = value;
					this.StateChanged(oldState);
				}
			}
		}
		#endregion

		#region Events
		/// <summary>
		/// Zdarzenie wywoływane przy inicjalizacji ekranu(dodaniu do managera).
		/// </summary>
		public virtual void OnInit()
		{ }

		/// <summary>
		/// Zdarzenie wywoływane przy deinicjalizacji ekranu(usunięcie z managera).
		/// </summary>
		public virtual void OnDeinit()
		{ }

		/// <summary>
		/// Zmienił się stan ekranu.
		/// </summary>
		/// <param name="oldState">Stan sprzed zmiany.</param>
		public virtual void StateChanged(ScreenState oldState)
		{ }

		/// <summary>
		/// Uaktualnienie.
		/// </summary>
		/// <param name="delta">Czas od ostatniego uaktualnienia.</param>
		void IScreen.Update(double delta)
		{
			this.Update(delta);
			this.CheckControls();
		}

		/// <summary>
		/// Ekran ma się odrysować.
		/// </summary>
		void IScreen.Render()
		{
			this.Render();
		}

		#region Keyboard
		/// <summary>
		/// Zdarzenie naciśnięcia/zwolnienia klawisza.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zdarzenie zostało obsłużone.</returns>
		public virtual bool KeyChanged(KeyEventArgs e)
		{ return false; }
		#endregion

		#region Mouse
		/// <summary>
		/// Zdarzenie naciśnięcia/zwolnienia przycisku myszy.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zdarzenie zostało obsłużone.</returns>
		public virtual bool MouseButton(MouseButtonEventArgs e)
		{ return false; }

		/// <summary>
		/// Zdarzenie poruszenia myszy.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zostało obsłużonę.</returns>
		public virtual bool MouseMove(MouseMoveEventArgs e)
		{ return false; }

		/// <summary>
		/// Zdarzenie "przekręcenia" kółka myszy.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zostało obsłużonę.</returns>
		public virtual bool MouseWheel(MouseWheelEventArgs e)
		{ return false; }
		#endregion
		#endregion
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje nowy ekran.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="type">Typ.</param>
		public GuiScreen(string id, ScreenType type)
		{
			this.Id = id;
			this.Type = type;
		}
		#endregion

		#region Others
		/// <summary>
		/// Metoda-zdarzenie służąca do sprawdzenia kontrolek.
		/// </summary>
		public abstract void CheckControls();
		#endregion
	}
}
