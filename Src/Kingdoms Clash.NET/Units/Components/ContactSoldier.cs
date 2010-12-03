using System.Diagnostics;
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
	[DebuggerDisplay("ContactSoldier, Strength = {Strength}")]
	public class ContactSoldier
		: IContactSoldier
	{
		#region IContactSoldier Members
		/// <summary>
		/// Siła jednostki.
		/// </summary>
		public int Strength { get; set; }
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

		#region IXmlSerializable Members
		public void Serialize(System.Xml.XmlElement element)
		{
			element.SetAttribute("strength", this.Strength.ToString());
		}

		public void Deserialize(System.Xml.XmlElement element)
		{
			if (element.HasAttribute("strength"))
			{
				try
				{
					this.Strength = int.Parse(element.GetAttribute("strength"));
				}
				catch (System.Exception ex)
				{
					new System.Xml.XmlException("Parsing error", ex);
				}
			}
			else
			{
				throw new System.Xml.XmlException("Insufficient data: strength");
			}
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

		public ContactSoldier()
		{ }
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
