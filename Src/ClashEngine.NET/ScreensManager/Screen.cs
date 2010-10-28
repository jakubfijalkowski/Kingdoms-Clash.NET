using System.Diagnostics;
using OpenTK.Input;

namespace ClashEngine.NET.ScreensManager
{
	using Interfaces;
	using Interfaces.ScreensManager;

	/// <summary>
	/// Bazowa klasa dla "ekranów"(np. menu, plansza).
	/// </summary>
	/// <seealso cref="IScreen"/>
	/// <seealso cref="IScreensManager"/>
	/// <seealso cref="ScreensManager"/>
	[DebuggerDisplay("Id = {Id,nq}, State = {State}")]
	public abstract class Screen
		: IScreen
	{
		private ScreenState _State = ScreenState.Deactivated;
		private EntitiesManager.EntitiesManager _Entites = new EntitiesManager.EntitiesManager();

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

		/// <summary>
		/// Manager encji ekranu.
		/// </summary>
		public EntitiesManager.EntitiesManager Entities
		{
			get { return this._Entites; }
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
		public virtual void Update(double delta)
		{
			this._Entites.Update(delta);
		}

		/// <summary>
		/// Ekran ma się odrysować.
		/// </summary>
		public virtual void Render()
		{
			this._Entites.Render();
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
		
		#region Utilities
		/// <summary>
		/// Odpowiednik <see cref="IScreensManager.Activate(IScreen)"/> dla tego ekranu.
		/// </summary>
		public void Activate()
		{
			this.Manager.Activate(this);
		}

		/// <summary>
		/// Odpowiednik <see cref="IScreensManager.Deactivate(IScreen)"/> dla tego ekranu.
		/// </summary>
		public void Deactivate()
		{
			this.Manager.Deactivate(this);
		}

		/// <summary>
		/// Odpowiednik <see cref="IScreensManager.MoveTo(IScreen, int)"/> dla tego ekranu.
		/// </summary>
		public void MoveTo(int position)
		{
			this.Manager.MoveTo(this, position);
		}

		/// <summary>
		/// Odpowiednik <see cref="IScreensManager.MoveToFront(IScreen)"/> dla tego ekranu.
		/// </summary>
		public void MoveToFront()
		{
			this.Manager.MoveToFront(this);
		}
		#endregion

		/// <summary>
		/// Inicjalizuje nowy ekran.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="type">Typ.</param>
		public Screen(string id, ScreenType type)
		{
			this.Id = id;
			this.Type = type;
		}
	}
}
