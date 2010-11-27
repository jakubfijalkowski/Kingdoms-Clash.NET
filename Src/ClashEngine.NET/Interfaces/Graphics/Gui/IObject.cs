namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Bazowy interfejs dla obiektów renderera kontrolki GUI.
	/// </summary>
	public interface IObject
		: Graphics.IObject
	{
		/// <summary>
		/// Kontrolka-rodzic.
		/// </summary>
		IControl ParentControl { get; set; }

		/// <summary>
		/// Czy obiekt jest widoczny.
		/// </summary>
		bool Visible { get; set; }

		/// <summary>
		/// Metoda, w której wszystko już ma prawidłowe wartości.
		/// Służy do np. korekty rozmiarów kontrolki lub jej pozycji.
		/// </summary>
		void Finish();
	}
}
