namespace Kingdoms_Clash.NET.Controllers
{
	using Interfaces;

	/// <summary>
	/// Domyślna klasa ustawień dla zasobów.
	/// </summary>
	public class DefaultGameplaySettings
		: IGameplaySettings
	{
		#region IGameplaySettings Members
		/// <summary>
		/// Czas pomiędzy poszczególnymi odnowieniami zasobów.
		/// </summary>
		public float ResourceRenewalTime { get; set; }

		/// <summary>
		/// Wartość zasobu.
		/// </summary>
		public uint ResourceRenewalValue { get; set; }

		/// <summary>
		/// Ilość zasobów na start.
		/// </summary>
		public uint StartResources { get; set; }
		#endregion

		#region Constructors
		public DefaultGameplaySettings()
		{
			this.ResourceRenewalTime = 8;
			this.ResourceRenewalValue = 10;
			this.StartResources = 50;
		}
		#endregion
	}
}
