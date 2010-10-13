using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.Utilities;
using FarseerPhysics.Dynamics;

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
		/// <summary>
		/// Klasa właściwego komponentu - nie musi być widoczna publicznie.
		/// </summary>
		private class CollectorComponent
			: Component, IUnitComponent
		{
			#region Private fields
			private IAttribute<float> VelocityMultiplier;
			private Interfaces.Map.IResourceOnMap CarriedResource = null;
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

				(this.Owner as IUnit).CollisionWithResource += new CollisionWithResourceEventHandler(CollisionWithResource);
				(this.Owner as IUnit).CollisionWithPlayer += new CollisionWithPlayerEventHandler(CollisionWithPlayer);

				var body = this.Owner.Attributes.Get<Body>("Body");
				if (body == null || body.Value == null || body.Value.FixtureList.Count == 0)
				{
					throw new ClashEngine.NET.Exceptions.NotFoundException("Body");
				}
				body.Value.AddCollidesWith(CollisionCategory.Cat10);
			}

			public override void Update(double delta)
			{ }
			#endregion

			#region Events
			/// <summary>
			/// Wywoływane przy kolizji z zasobem na mapie.
			/// </summary>
			/// <param name="unit"></param>
			/// <param name="resource"></param>
			/// <returns></returns>
			bool CollisionWithResource(IUnit unit, Interfaces.Map.IResourceOnMap resource)
			{
				this.CarriedResource = resource;
				if (resource.Value > (this.Description as ICollector).MaxCargoSize)
				{
					this.CarriedResource.Value = (this.Description as ICollector).MaxCargoSize;
				}
				this.VelocityMultiplier.Value *= -1f;
				return true;
			}

			/// <summary>
			/// Wywoływane przy kolizji gracza z zamkiem.
			/// </summary>
			/// <param name="unit"></param>
			/// <param name="player"></param>
			void CollisionWithPlayer(IUnit unit, Interfaces.Player.IPlayer player)
			{
				//Tylko dla własnego zamku i tylko, gdy niesiemy jakiś zasób.
				if ((this.Owner as IUnit).Owner == player && this.CarriedResource != null)
				{
					player.Resources.Add(this.CarriedResource.Id, this.CarriedResource.Value);
				}
			}
			#endregion
		}
		#endregion
	}
}
