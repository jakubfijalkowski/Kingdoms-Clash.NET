using System.Diagnostics;

namespace Kingdoms_Clash.NET.Player
{
	using Interfaces.Player;
	using Interfaces.Units;

	/// <summary>
	/// Gracz - człowiek.
	/// </summary>
	[DebuggerDisplay("{this,nq}")]
	public class Human
		: Castle, IHuman
	{
		/// <summary>
		/// Inicjalizuje nowego gracza.
		/// </summary>
		/// <param name="name">Nazwa.</param>
		/// <param name="nation">Jego nacja.</param>
		public Human(string name, INation nation/*, IEnumerable<IResource> startResources*/)
			: base("Player.Human." + name, name, nation)
		{ }

		public override string ToString()
		{
			return string.Format("Human player called {0}", this.Name);
		}
	}
}
