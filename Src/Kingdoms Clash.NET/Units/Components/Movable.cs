using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.Utilities;
using FarseerPhysics.Dynamics;

namespace Kingdoms_Clash.NET.Units.Components
{
	using Interfaces.Units;
	using Interfaces.Units.Components;

	/// <summary>
	/// Opis komponentu jednostki określający, że jednostka potrafi się poruszać.
	/// </summary>
	public class Movable
		: IMovable
	{
		#region IMovable Members
		/// <summary>
		/// Prędkość jednostki.
		/// </summary>
		public OpenTK.Vector2 Velocity { get; private set; }
		#endregion

		#region IUnitComponentDescription Members
		/// <summary>
		/// Tworzy komponent na podstawie opisu.
		/// </summary>
		/// <returns>Komponent.</returns>
		public IUnitComponent Create()
		{
			return new MovableComponent() { Description = this };
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje opis.
		/// </summary>
		/// <param name="velocity">Prędkość jednostki.</param>
		public Movable(OpenTK.Vector2 velocity)
		{
			this.Velocity = velocity;
		}
		#endregion

		#region Component
		/// <summary>
		/// Klasa właściwego komponentu - nie musi być widoczna publicznie.
		/// </summary>
		private class MovableComponent
			: Component, IUnitComponent
		{
			private IAttribute<Body> Body;
			private IAttribute<float> VelocityMultiplier;

			#region IUnitComponent Members
			/// <summary>
			/// Opis komponentu.
			/// </summary>
			public IUnitComponentDescription Description { get; set; }
			#endregion

			#region Component Members
			/// <summary>
			/// Inicjalizuje komponent pobierając atrybuty i ustawiając VelocityMultiplier.
			/// </summary>
			public override void OnInit()
			{
				this.VelocityMultiplier = this.Owner.Attributes.GetOrCreate<float>("VelocityMultiplier");
				if (this.VelocityMultiplier.Value == 0f) //Jeśli nie mamy jeszcze tego atrybutu...
				{
					this.VelocityMultiplier.Value = 1f;
				}
				if ((this.Owner as IUnit).Owner.Type == Interfaces.Player.PlayerType.Second) //Jeśli to drugi gracz musimy mu ustawić prędkość w drugą stronę!
				{
					this.VelocityMultiplier.Value *= -1f;
				}

				this.Body = this.Owner.Attributes.Get<Body>("Body");
			}

			/// <summary>
			/// Przemieszcza jednostkę.
			/// </summary>
			/// <param name="delta"></param>
			public override void Update(double delta)
			{
				this.Body.Value.Position += ((float)delta * (this.Description as IMovable).Velocity * this.VelocityMultiplier.Value).ToXNA();
			}
			#endregion

			#region Constructors
			public MovableComponent()
				: base("Movable")
			{ }
			#endregion
		}
		#endregion
	}
}
