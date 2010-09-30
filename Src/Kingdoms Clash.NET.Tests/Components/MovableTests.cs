using System;
using FarseerPhysics.Dynamics;
using Kingdoms_Clash.NET.Interfaces.Units;
using Kingdoms_Clash.NET.Interfaces.Units.Components;
using Kingdoms_Clash.NET.Units;
using Kingdoms_Clash.NET.Units.Components;
using NUnit.Framework;

namespace Kingdoms_Clash.NET.Tests.Components
{
	[TestFixture(Description = "Testy komponentu Movable BEZ aktualizacji fizyki")]
	[Category("Components")]
	public class MovableTests
	{
		private const float TimeStep = 0.3f;
		private const int Iterations = 10;
		private const float Velocity = 10f;
		private const float PositionDelta = Velocity * TimeStep;

		private IMovable Component;
		private IUnit Unit;
		private Body Body;

		[SetUp]
		public void SetUp()
		{
			this.Component = new Movable();

			UnitDescription desc = new UnitDescription("TestUnit", 100, 5f, 5f);
			(desc.Attributes as UnitAttributesCollection).Add(new UnitAttribute<float>("Velocity", Velocity));
			desc.Components.Add(this.Component);
			this.Unit = new Unit(desc, null);
			this.Unit.Init(null);

			this.Body = this.Unit.Attributes.Get<Body>("Body").Value; //Na pewno istnieje
		}

		[Test]
		public void UnitUpdates()
		{
			var oldPosition = this.Body.Position;
			for (int i = 0; i < Iterations; i++)
			{
				this.Unit.Update(TimeStep);

				Assert.Less(Math.Abs(this.Body.Position.X - oldPosition.X - PositionDelta), float.Epsilon);
				oldPosition = this.Body.Position;
			}
		}
	}
}
