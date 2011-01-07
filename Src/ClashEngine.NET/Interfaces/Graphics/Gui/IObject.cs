namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Bazowy interfejs dla obiektów renderera kontrolki GUI.
	/// </summary>
	public interface IObject
	{
		/// <summary>
		/// Pozycja relatywna - nie uwzględnia pozycji kontrolki.
		/// </summary>
		OpenTK.Vector2 Position { get; set; }

		/// <summary>
		/// Pozycja absolutna - uwzględnia pozycję(absolutną!) kontrolki(<see cref="Owner"/>).
		/// </summary>
		OpenTK.Vector2 AbsolutePosition { get; }

		/// <summary>
		/// Kontrolka-rodzic.
		/// </summary>
		IStylizableControl Owner { get; set; }

		/// <summary>
		/// Czy obiekt jest widoczny.
		/// </summary>
		bool Visible { get; set; }

		/// <summary>
		/// Głębokość, na któej obiekt się znajduje.
		/// </summary>
		float Depth { get; set; }

		/// <summary>
		/// Metoda wywoływana przy dodaniu do kolekcji obiektów.
		/// Służy do np. korekty rozmiarów/pozycji kontrolki.
		/// </summary>
		void OnAdd();

		/// <summary>
		/// Wyświetla obiekt.
		/// </summary>
		void Render();
	}
}
