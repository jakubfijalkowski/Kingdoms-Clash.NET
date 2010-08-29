using System;
using OpenTK.Input;

namespace ClashEngine.NET.ScreensManager
{
	using Interfaces.ScreensManager;

	/// <summary>
	/// Bazowa klasa dla "ekranów"(np. menu, plansza).
	/// Tylko jeden ekran może być aktywny, tzn. jeśli zostanie zasłonięty innym nie będzie aktualizowany.
	/// To zachowanie można zmienić ustawiając właściwość IsPopup na true, wtedy ekran "pod" nadal będzie aktualizowany.
	/// Jeśli ekran ma ustawioną właściwość IsFullscreen na true ekrany pod nie są odrysowywane - względy wydajnościowe.
	/// </summary>
	public abstract class Screen
		: IScreen
	{
		private bool _IsPopup = false;
		private bool _IsFullscreen = false;
		private ScreenState State_ = ScreenState.Closed;
		private EntitiesManager.EntitiesManager Entites_ = new EntitiesManager.EntitiesManager();

		#region Properties
		/// <summary>
		/// Manager - rodzic.
		/// </summary>
		public IScreensManager Manager { get; private set; }

		/// <summary>
		/// Czy ekran jest ekranem "wyskakującym", czyli nie zasłania reszty a co za tym idzie pozwala na ich aktualizację.
		/// Nie może być true jeśli IsFullscreen == true.
		/// </summary>
		public bool IsPopup
		{
			get { return this._IsPopup; }
			set
			{
				if (value && this.IsFullscreen)
				{
					throw new ArgumentException("IsFullscreen is true");
				}
				this._IsPopup = value;
			}
		}

		/// <summary>
		/// Czy ekran jest pełnoekranowy(zasłania wszystko co pod).
		/// Nie może być true jeśli IsPopup == true;
		/// </summary>
		public bool IsFullscreen
		{
			get { return this._IsFullscreen; }
			set
			{
				if (value && this.IsPopup)
				{
					throw new ArgumentException("IsPopup is true");
				}
				this._IsFullscreen = value;
			}
		}

		/// <summary>
		/// Aktualny stan ekranu.
		/// </summary>
		public ScreenState State
		{
			get { return this.State_; }
			private set { this.State_ = value; }
		}

		/// <summary>
		/// Manager encji ekranu.
		/// </summary>
		public EntitiesManager.EntitiesManager Entities
		{
			get { return this.Entites_; }
		}
		#endregion

		#region Object management
		/// <summary>
		/// Inicjalizuje obiekt.
		/// </summary>
		/// <param name="screensManager">Manager ekranów.</param>
		public void Init(IScreensManager screensManager)
		{
			this.Manager = screensManager;
			this.State = ScreenState.Closed;
		}

		/// <summary>
		/// Zmienia stan ekranu.
		/// </summary>
		/// <param name="state">Nowy stan.</param>
		public void ChangeState(ScreenState state)
		{
			this.State = state;
			this.StateChanged();
		}
		#endregion

		#region Events
		/// <summary>
		/// Uaktualnienie.
		/// </summary>
		/// <param name="delta">Czas od ostatniego uaktualnienia.</param>
		public virtual void Update(double delta)
		{
			this.Entites_.Update(delta);
		}

		/// <summary>
		/// Ekran ma się odrysować.
		/// </summary>
		public virtual void Render()
		{
			this.Entites_.Render();
		}

		/// <summary>
		/// Zmienił się stan ekranu.
		/// </summary>
		public virtual void StateChanged()
		{ }

		#region Keyboard
		/// <summary>
		/// Zdarzenie naciśnięcia klawisza.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zdarzenie zostało obsłużone.</returns>
		public bool KeyDown(KeyboardKeyEventArgs e)
		{ return false; }

		/// <summary>
		/// Zdarzenie zwolnienia klawisza.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zdarzenie zostało obsłużone.</returns>
		public bool KeyUp(KeyboardKeyEventArgs e)
		{ return false; }
		#endregion

		#region Mouse
		/// <summary>
		/// Zdarzenie naciśnięcia przycisku myszy.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zdarzenie zostało obsłużone.</returns>
		public bool MouseButtonDown(MouseButtonEventArgs e)
		{ return false; }

		/// <summary>
		/// Zdarzenie zwolnienia przycisku myszy.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zdarzenie zostało obsłużone.</returns>
		public bool MouseButtonUp(MouseButtonEventArgs e)
		{ return false; }

		/// <summary>
		/// Zdarzenie poruszenia myszy.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zostało obsłużonę.</returns>
		public bool MouseMove(MouseMoveEventArgs e)
		{ return false; }

		/// <summary>
		/// Zdarzenie "przekręcenia" kółka myszy.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zostało obsłużonę.</returns>
		public bool MouseWheelChanged(MouseWheelEventArgs e)
		{ return false; }
		#endregion
		#endregion
	}
}
