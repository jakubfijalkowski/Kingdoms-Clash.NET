using System;

namespace ClashEngine.NET.ScreensManager
{
	/// <summary>
	/// Stan ekranu.
	/// </summary>
	public enum ScreenState
	{
		/// <summary>
		/// Aktywny.
		/// </summary>
		Active,

		/// <summary>
		/// Nieaktywny(np. zasłonięty przez inny nie-popup.
		/// </summary>
		Inactive,

		/// <summary>
		/// Zamknięty(nieodrysowywany ani nieuaktualniany, ale pozostający w managerze).
		/// </summary>
		Closed
	}

	/// <summary>
	/// Bazowa klasa dla "ekranów"(np. menu, plansza).
	/// Tylko jeden ekran może być aktywny, tzn. jeśli zostanie zasłonięty innym nie będzie aktualizowany.
	/// To zachowanie można zmienić ustawiając właściwość IsPopup na true, wtedy ekran "pod" nadal będzie aktualizowany.
	/// Jeśli ekran ma ustawioną właściwość IsFullscreen na true ekrany pod nie są odrysowywane - względy wydajnościowe.
	/// </summary>
	public abstract class Screen
	{
		private bool _IsPopup = false;
		private bool _IsFullscreen = false;
		private ScreenState State_ = ScreenState.Closed;

		#region Properties
		/// <summary>
		/// Manager - rodzic.
		/// </summary>
		public ScreensManager Manager { get; internal set; }

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
			internal set { this.State_ = value; }
		}

		#endregion

		#region Events
		/// <summary>
		/// Uaktualnienie.
		/// </summary>
		/// <param name="delta">Czas od ostatniego uaktualnienia.</param>
		public virtual void Update(double delta)
		{ }

		/// <summary>
		/// Ekran ma się odrysować.
		/// </summary>
		public virtual void Render()
		{ }

		/// <summary>
		/// Zmienił się stan ekranu.
		/// </summary>
		public virtual void StateChanged()
		{ }
		#endregion
	}
}
