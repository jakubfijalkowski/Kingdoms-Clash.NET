using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Units.Components
{
	using Interfaces.Units;
	using Interfaces.Units.Components;

	/// <summary>
	/// Komponent określający, czy jednostka potrafi zbierać zasoby.
	/// </summary>
	/// <remarks>
	/// Używa atrybutu VelocityMultiplier do zawracania jednostki.
	/// </remarks>
	public class Collector
		: ICollector
	{
		#region ICollector Members
		/// <summary>
		/// Maksymalny rozmiar ładunku, który może unieść jednostka.
		/// </summary>
		public uint MaxCargoSize { get; private set; }
		#endregion

		#region IUnitComponentDescription Members
		/// <summary>
		/// Tworzy komponent na podstawie opisu.
		/// </summary>
		/// <returns>Komponent.</returns>
		public IUnitComponent Create()
		{
			return new CollectorComponent() { Description = this };
		}
		#endregion

		#region IXmlSerializable Members
		public void Serialize(System.Xml.XmlElement element)
		{
			element.SetAttribute("MaxCargoSize", this.MaxCargoSize.ToString());
		}

		public void Deserialize(System.Xml.XmlElement element)
		{
			if (element.HasAttribute("maxcargosize"))
			{
				try
				{
					this.MaxCargoSize = uint.Parse(element.GetAttribute("maxcargosize"));
				}
				catch (System.Exception ex)
				{
					new System.Xml.XmlException("Parsing error", ex);
				}
			}
			else
			{
				throw new System.Xml.XmlException("Insufficient data: maxcargosize");
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje opis.
		/// </summary>
		/// <param name="maxCargoSize">Maksymalny rozmiar ładunku.</param>
		public Collector(uint maxCargoSize)
		{
			this.MaxCargoSize = maxCargoSize;
		}

		/// <summary>
		/// Inicjalizuje opis.
		/// </summary>
		public Collector()
		{
			this.MaxCargoSize = 0;
		}
		#endregion

		#region Component
		private class CollectorComponent
			: Component, IUnitComponent
		{
			#region Private fields
			private IAttribute<float> VelocityMultiplier;
			private Interfaces.Map.ResourceOnMap CarriedResource = null;
			#endregion

			#region IUnitComponent Members
			public IUnitComponentDescription Description { get; set; }
			#endregion

			#region Constructors
			public CollectorComponent()
				: base("Collector")
			{ }
			#endregion

			#region Component Members
			public override void OnInit()
			{
				this.VelocityMultiplier = this.Owner.Attributes.GetOrCreate<float>("VelocityMultiplier");

				(this.Owner as IUnit).Owner.GameState.Map.Collide += new Interfaces.Map.CollisionWithResourceEventHandler(CollisionWithResource);
				(this.Owner as IUnit).Owner.Collide += new Interfaces.Player.UnitCollideWithPlayerEventHandler(CollisionWithPlayer);
			}

			public override void Update(double delta)
			{ }
			#endregion

			/// <summary>
			/// Wywoływane przy kolizji z zasobem na mapie.
			/// </summary>
			/// <param name="unit"></param>
			/// <param name="resource"></param>
			/// <returns></returns>
			bool CollisionWithResource(IUnit unit, Interfaces.Map.ResourceOnMap resource)
			{
				if (unit == this.Owner)
				{
					this.CarriedResource = resource;
					if (resource.Value > (this.Description as ICollector).MaxCargoSize)
					{
						this.CarriedResource.Value = (this.Description as ICollector).MaxCargoSize;
					}
					this.VelocityMultiplier.Value *= -1f;
					return true;
				}
				return false;
			}

			/// <summary>
			/// Wywoływane przy kolizji gracza z zamkiem.
			/// </summary>
			/// <param name="unit"></param>
			/// <param name="player"></param>
			void CollisionWithPlayer(IUnit unit, Interfaces.Player.IPlayer player)
			{
				//Tylko TA jednostka, tylko dla własnego zamku i tylko, gdy niesiemy jakiś zasób.
				if (unit == this.Owner && this.CarriedResource != null)
				{
					player.Resources.Add(this.CarriedResource.Id, this.CarriedResource.Value);
				}
			}
		}
		#endregion
	}
}
