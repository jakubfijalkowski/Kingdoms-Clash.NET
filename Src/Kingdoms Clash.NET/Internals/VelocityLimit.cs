using FarseerPhysics.Controllers;

namespace Kingdoms_Clash.NET.Internals
{
	using Interfaces.Units;
	using Kingdoms_Clash.NET.Interfaces.Units.Components;

	/// <summary>
	/// Kontroler ograniczający prędkość jednostek.
	/// </summary>
	internal class VelocityLimit
		: Controller
	{
		public VelocityLimit()
			: base(ControllerType.VelocityLimitController)
		{ }

		public override void Update(float dt)
		{
			foreach (var body in this.World.BodyList)
			{
				if (body.UserData is IUnit)
				{
					IUnit unit = body.UserData as IUnit;
					var movable = unit.Description.Components.GetSingle<IMovable>();
					if (movable != null)
					{
						var len = body.LinearVelocity.Length();
						if (len > movable.MaxVelocity)
						{
							body.LinearVelocity *= movable.MaxVelocity / len;
						}
					}
				}
			}
		}
	}
}
