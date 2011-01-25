using System;
using FarseerPhysics.Dynamics;
using Kingdoms_Clash.NET.Interfaces.Units;
using Kingdoms_Clash.NET.Interfaces.Units.Components;
using Kingdoms_Clash.NET.Units;
using Kingdoms_Clash.NET.Units.Components;
using NUnit.Framework;
using Moq;

namespace Kingdoms_Clash.NET.Tests.Components
{
	[TestFixture(Description = "Testy komponentu Movable BEZ aktualizacji fizyki")]
	[Category("Components")]
	public class MovableTests
	{
		private const float TimeStep = 0.3f;
		private const int Iterations = 10;
		private const float Velocity = 10f;
		private const float Force = 1300f;
		private const float PositionDelta = Velocity * TimeStep;

		private IUnitComponentDescription Component;
		private IUnit Unit;
		private Body Body;

		private Mock<UnitTests.TestPlayer> Player;

		[SetUp]
		public void SetUp()
		{
			this.Player = new Mock<UnitTests.TestPlayer>();
			this.Player.SetupAllProperties();

			this.Component = new Movable(Velocity, Force);

			UnitDescription desc = new UnitDescription("TestUnit", 100, 1f, 5f, 5f);
			desc.Components.Add(this.Component);
			//(desc.Attributes as UnitAttributesCollection).Add(new UnitAttribute<float>("Velocity", Velocity));
			//desc.Components.Add(this.Component);
			this.Unit = new Unit(desc, this.Player.Object);
			//this.Unit.Owner = null;
			this.Unit.OnInit();

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
