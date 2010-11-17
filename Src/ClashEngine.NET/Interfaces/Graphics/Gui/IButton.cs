namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Przycisk.
	/// </summary>
	public interface IButton
		: IControl
	{
		/// <summary>
		/// Pozycja kontrolki.
		/// </summary>
		[System.ComponentModel.TypeConverter(typeof(Converters.Vector2Converter))]
		OpenTK.Vector2 Size { get; set; }

		/// <summary>
		/// Czy przycisk jest wciśnięty.
		/// </summary>
		bool Clicked { get; }
	}
}
