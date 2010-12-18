namespace Kingdoms_Clash.NET.Interfaces.Player.Controllers
{
	/// <summary>
	/// Fabryka dla kontrolerów GUI graczy.
	/// </summary>
	/// <remarks>
	/// <see cref="Produce"/> zwraca obiekty, które implementują IGuiController.
	/// </remarks>
	public interface IGuiControllerFactory
		: Factories.IPlayerControllerFactory
	{ }
}
