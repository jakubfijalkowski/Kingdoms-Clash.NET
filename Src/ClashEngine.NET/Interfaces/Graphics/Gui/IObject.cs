using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Bazowy interfejs dla obiektów renderera kontrolki GUI.
	/// </summary>
	public interface IObject
		: Graphics.IObject
	{
		/// <summary>
		/// Pozycja relatywna - nie uwzględnia pozycji kontrolki.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector2Converter))]
		OpenTK.Vector2 Position { get; set; }

		/// <summary>
		/// Pozycja absolutna - uwzględnia pozycję(absolutną!) kontrolki(<see cref="ParentControl"/>).
		/// </summary>
		OpenTK.Vector2 AbsolutePosition { get; }

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
