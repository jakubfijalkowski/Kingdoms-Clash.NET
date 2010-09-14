namespace Kingdoms_Clash.NET.Interfaces.Controllers
{
	/// <summary>
	/// Bazowy interfejs dla kontrolera gry.
	/// </summary>
	public interface IGameController
	{
		/// <summary>
		/// Gra.
		/// </summary>
		IGame Game { get; }
	}
}
