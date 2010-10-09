using ClashEngine.NET.EntitiesManager;

namespace Kingdoms_Clash.NET.Units.Components
{
	using Interfaces.Units;
	using Interfaces.Units.Components;

	/// <summary>
	/// Opis komponentu dla jednostki walczącej wręcz.
	/// </summary>
	/// <remarks>
	/// Tak szczerze powiedziawszy, to bez sensu jest to jako komponent, ale trzymam się wytycznych ;)
	/// </remarks>
	public class ContactSoldier
		: IContactSoldier
	{
		#region IContactSoldier Members
		/// <summary>
		/// Siła jednostki.
		/// </summary>
		public int Strength { get; private set; }
		#endregion

		#region IUnitComponentDescription Members
		/// <summary>
		/// Tworzy komponent na podstawie opisu.
		/// </summary>
		/// <returns>Komponent.</returns>
		public IUnitComponent Create()
		{
			return new ContactSoldierComponent() { Description = this };
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje opis.
		/// </summary>
		/// <param name="strength">Siła jednostki.</param>
		public ContactSoldier(int strength)
		{
			this.Strength = strength;
		}
		#endregion

		#region Component
		/// <summary>
		/// Klasa właściwego komponentu - nie musi być widoczna publicznie.
		/// </summary>
		private class ContactSoldierComponent
			: Component, IUnitComponent
		{
			#region IUnitComponent Members
			/// <summary>
			/// Opis komponentu.
			/// </summary>
			public IUnitComponentDescription Description { get; set; }
			#endregion

			#region Component Members
			public override void Update(double delta)
			{ }
			#endregion

			#region Constructors
			public ContactSoldierComponent()
				: base("ContactSoldier")
			{ }
			#endregion
		}
		#endregion
	}
}
