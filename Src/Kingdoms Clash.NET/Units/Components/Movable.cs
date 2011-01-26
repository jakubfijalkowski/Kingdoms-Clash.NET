using System.Diagnostics;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Extensions;
using ClashEngine.NET.Interfaces.EntitiesManager;
using FarseerPhysics.Dynamics;

namespace Kingdoms_Clash.NET.Units.Components
{
	using Interfaces.Units;
	using Interfaces.Units.Components;

	/// <summary>
	/// Opis komponentu jednostki określający, że jednostka potrafi się poruszać.
	/// </summary>
	/// <remarks>
	/// Komponent używa atrybutu VelocityMultiplier do określenia kierunku(i mnożnika - domyślnie 1) prędkości jednostki.
	/// </remarks>
	[DebuggerDisplay("Movable, Velocity = {Velocity}")]
	public class Movable
		: IMovable
	{
		#region IMovable Members
		/// <summary>
		/// Maksymalna prędkość jednostki.
		/// </summary>
		public float MaxVelocity { get; set; }

		/// <summary>
		/// Siła, jaka działa na jednostkę.
		/// </summary>
		public float Force { get; set; }
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
		/// <param name="maxVelocity">Prędkość jednostki.</param>
		/// <param name="force">Siła, jaka działa na jednostkę.</param>
		public Movable(float maxVelocity, float force)
		{
			this.MaxVelocity = maxVelocity;
			this.Force = force;
		}

		/// <summary>
		/// Inicjalizuje opis domyślnymi wartościami.
		/// </summary>
		public Movable()
		{ }
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
				var f = this.Body.Value.GetWorldVector(new Microsoft.Xna.Framework.Vector2((this.Description as IMovable).Force * this.VelocityMultiplier.Value, 0f));
				this.Body.Value.ApplyForce(ref f);
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
