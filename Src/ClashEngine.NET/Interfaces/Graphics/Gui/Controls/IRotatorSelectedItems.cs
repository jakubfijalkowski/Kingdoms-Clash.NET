namespace ClashEngine.NET.Interfaces.Graphics.Gui.Controls
{
	public interface IRotatorSelectedItems
	{
		/// <summary>
		/// Odpowiada <see cref="IRotator.Items"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		object this[int index] { get; }
	}
}
