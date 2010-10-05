using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Units.Components
{
	using Interfaces.Units;
	using Interfaces.Units.Components;

	/// <summary>
	/// Interfejs dla komponentów jednostek które walczą wręcz.
	/// </summary>
	public class ContactSoldier
		: Component, IContactSoldier
	{
		private IAttribute<int> Strength_;

		#region IContactSoldier Members
		/// <summary>
		/// Siła jednostki.
		/// </summary>
		public int Strength
		{
			get { return this.Strength_.Value; }
			private set { this.Strength_.Value = value; }
		}
		#endregion

		#region Component Members
		public override void OnInit()
		{
			this.Strength_ = this.Owner.Attributes.GetOrCreate<int>("Strength");
			this.Strength = (this.Owner as IUnit).Description.Attributes.Get<int>("Strength");
		}

		public override void Update(double delta)
		{ }
		#endregion

		#region ICloneable Members
		public object Clone()
		{
			return new ContactSoldier();
		}
		#endregion

		public ContactSoldier()
			: base("ContactSoldier")
		{ }
	}
}
