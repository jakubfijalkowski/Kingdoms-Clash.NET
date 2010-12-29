using System.Diagnostics;

namespace ClashEngine.NET
{
	using Interfaces;

	/// <summary>
	/// Bazowa klasa dla "ekranów"(np. menu, plansza).
	/// UWAGA! Właściwość Entites ustawiana jest w metodzie OnInit!
	/// </summary>
	/// <seealso cref="IScreen"/>
	/// <seealso cref="IScreensManager"/>
	/// <seealso cref="ScreensManager"/>
	[DebuggerDisplay("Id = {Id,nq}, State = {State}")]
	public abstract class Screen
		: IScreen
	{
		#region Private fields
		private ScreenState _State = ScreenState.Deactivated;
		private EntitiesManager.EntitiesManager _Entities;
		#endregion

		#region Properties
		/// <summary>
		/// Identyfikator ekranu.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Manager - rodzic.
		/// </summary>
		public IScreensManager OwnerManager { get; set; }

		/// <summary>
		/// Informacje o grze.
		/// Ustawiane przez właściciela.
		/// </summary>
		public IGameInfo GameInfo { get; set; }

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
		protected EntitiesManager.EntitiesManager Entities
		{
			get { return this._Entities; }
		}

		/// <summary>
		/// Kamera dla ekranu.
		/// </summary>
		protected Interfaces.Graphics.ICamera Camera { get; set; }
		#endregion

		#region Events
		/// <summary>
		/// Zdarzenie wywoływane przy inicjalizacji ekranu(dodaniu do managera).
		/// </summary>
		public virtual void OnInit()
		{
			this._Entities = new EntitiesManager.EntitiesManager(this.GameInfo);
		}

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
			this._Entities.Update(delta);
		}

		/// <summary>
		/// Ekran ma się odrysować.
		/// </summary>
		public virtual void Render()
		{
			if (this.Camera != null)
			{
				this.GameInfo.Renderer.Camera = this.Camera;
			}
			this._Entities.Render();
		}
		#endregion
		
		#region Utilities
		/// <summary>
		/// Odpowiednik <see cref="IScreensManager.Activate(IScreen)"/> dla tego ekranu.
		/// </summary>
		public void Activate()
		{
			this.OwnerManager.Activate(this);
		}

		/// <summary>
		/// Odpowiednik <see cref="IScreensManager.Deactivate(IScreen)"/> dla tego ekranu.
		/// </summary>
		public void Deactivate()
		{
			this.OwnerManager.Deactivate(this);
		}

		/// <summary>
		/// Odpowiednik <see cref="IScreensManager.MoveTo(IScreen, int)"/> dla tego ekranu.
		/// </summary>
		public void MoveTo(int position)
		{
			this.OwnerManager.MoveTo(this, position);
		}

		/// <summary>
		/// Odpowiednik <see cref="IScreensManager.MoveToFront(IScreen)"/> dla tego ekranu.
		/// </summary>
		public void MoveToFront()
		{
			this.OwnerManager.MoveToFront(this);
		}
		#endregion

		#region Constructors
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
		#endregion
	}
}
