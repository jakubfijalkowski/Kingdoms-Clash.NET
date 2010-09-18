namespace Kingdoms_Clash.NET.Interfaces.Controllers
{
	/// <summary>
	/// Bazowy interfejs dla kontrolera gry.
	/// </summary>
	public interface IGameController
	{
		/// <summary>
		/// Resetuje stan gry(zaczynamy ją od nowa).
		/// </summary>
		void Reset();
	}
}
