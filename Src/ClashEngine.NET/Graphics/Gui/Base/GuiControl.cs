namespace ClashEngine.NET.Gui.Base
{
	using Interfaces.Gui;

	/// <summary>
	/// Bazowa klasa dla kontrolek.
	/// Implementuje część funkcjonalności <see cref="IGuiControl"/>, tak, by nie trzeba było tego za każdym razem implementować.
	/// </summary>
	public abstract class GuiControl
		: IGuiControl
	{
		#region IGuiControl Members
		/// <summary>
		/// Identyfikator kontrolki.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Dane UI.
		/// </summary>
		public IUIData Data { get; set; }
		
		/// <summary>
		/// Czy kontrolka ma być "permanentnie" aktywna, tzn. czy po puszczeniu przycisku myszy przestaje być aktywna.
		/// </summary>
		public abstract bool PermanentActive { get; }

		/// <summary>
		/// Sprawdza, czy myszka znajduje się nad kontrolką.
		/// </summary>
		/// <returns>Prawda, gdy myszka jest nad kontrolką. W przeciwnym razie fałsz.</returns>
		public abstract bool ContainsMouse();

		/// <summary>
		/// Uaktualnia kontrolkę.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		public abstract void Update(double delta);

		/// <summary>
		/// Renderuje kontrolkę.
		/// </summary>
		public abstract void Render();

		/// <summary>
		/// Sprawdza, czy zaszła jakaś akcja kontrolki(np. czy przycisk został wciśnięty).
		/// </summary>
		/// <returns>Nr akcji lub 0, gdy takowa nie zaszła.</returns>
		public abstract int Check();
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje kontrolkę.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		public GuiControl(string id)
		{
			this.Id = id;
		}
		#endregion
	}
}
