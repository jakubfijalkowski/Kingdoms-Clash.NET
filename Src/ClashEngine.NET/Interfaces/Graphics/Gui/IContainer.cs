using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Kontener na kontrolki.
	/// </summary>
	/// <remarks>
	/// Kontrolki układają się w hierarchię, której korzeniem jest IContainer i jej kontrolka przechowywana we właściwości Root.
	/// Każda kontrolka IContainerControl, która jest kontenerem na inne kontrolki, obsługuje swoje kontrolki(renderowanie, aktualizacja).
	/// Jeśli IContainerControl ma rodzica, który też jest IContainerControl to jest ona zobowiązana do dodanie swojej kontrolki-dziecka do niego
	/// używając metody <see cref="IControlsCollection.AddChild"/>.
	/// </remarks>
	public interface IContainer
		: ISupportInitialize
	{
		/// <summary>
		/// Informacje o grze.
		/// </summary>
		IGameInfo GameInfo { get; }

		/// <summary>
		/// Główna kontrolka.
		/// </summary>
		IContainerControl Root { get; }

		/// <summary>
		/// Uaktualnia wszystkie kontrolki w kontenerze.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		void Update(double delta);

		/// <summary>
		/// Renderuje wszystkie kontrolki.
		/// </summary>
		void Render();
	}
}
