using Kingdoms_Clash.NET.Controllers;
using Kingdoms_Clash.NET.Interfaces.Controllers;
using Kingdoms_Clash.NET.Interfaces.Units;
using NUnit.Framework;

namespace Kingdoms_Clash.NET.Tests
{
	using TestObjects;

	[TestFixture(Description = "Testy dla kontrolera gry: ClassicGame(+ UnitQueue)")]
	public class ClassicGameTests
	{
		private GameState State = null;
		private IGameController Controller = null;

		[SetUp]
		public void SetUp()
		{
			this.State = new TestObjects.GameState();
			this.Controller = new ClassicGame();
			this.Controller.GameState = this.State;
			this.State.Controller = this.Controller;
			this.Controller.GameStarted();
		}

		[TearDown]
		public void TearDown()
		{
			this.Controller.Reset();
		}

		[Test]
		public void RequestUnitWorks()
		{
			var token = this.Controller[this.State.Players[0]].Request("Unit1");
			Assert.IsNotNull(token);
			Assert.IsTrue(token.IsValidToken);
			Assert.IsFalse(token.IsPaused);
			Assert.IsFalse(token.IsCompleted);
			Assert.AreEqual(token.TimeLeft, GameState.TestUnit.CreationTime);

			var stats = this.Controller[this.State.Players[0]]["Unit1"];
			Assert.AreEqual(token, stats.CurrentToken);
			Assert.AreEqual(1, stats.QueueLength);
		}

		[Test]
		public void TestUnitTokens()
		{
			var tokensP1 = new IUnitRequestToken[]
			{
				this.Controller[this.State.Players[0]].Request("Unit1"),
				this.Controller[this.State.Players[0]].Request("Unit1"),
				this.Controller[this.State.Players[0]].Request("Unit1")
			};

			var tokensP2 = new IUnitRequestToken[]
			{
				this.Controller[this.State.Players[1]].Request("Unit1"),
				this.Controller[this.State.Players[1]].Request("Unit1"),
				this.Controller[this.State.Players[1]].Request("Unit1")
			};

			AreNotCompleted(tokensP1);
			AreNotCompleted(tokensP2);

			//Żadna jednostka nie jest stworzona.
			this.Controller.Update(GameState.TestUnit.CreationTime / 2);
			AreNotCompleted(tokensP1);
			AreNotCompleted(tokensP2);
			Assert.AreEqual(0, this.State.Units.Count);
			Assert.AreEqual(0, this.State.Players[0].Units.Count);
			Assert.AreEqual(0, this.State.Players[1].Units.Count);

			//Statystyki
			var stats = this.Controller[this.State.Players[0]]["Unit1"];
			Assert.AreEqual(tokensP1[0], stats.CurrentToken);
			Assert.AreEqual(3, stats.QueueLength);

			//Tu już po jednej jednostce powinno być stworzone.
			this.Controller.Update(GameState.TestUnit.CreationTime / 2);
			AreNotCompleted(tokensP1, 1);
			AreNotCompleted(tokensP2, 1);
			Assert.AreEqual(2, this.State.Units.Count);
			Assert.AreEqual(1, this.State.Players[0].Units.Count);
			Assert.AreEqual(1, this.State.Players[1].Units.Count);

			//Statystyki
			stats = this.Controller[this.State.Players[0]]["Unit1"];
			Assert.AreEqual(tokensP1[1], stats.CurrentToken);
			Assert.AreEqual(2, stats.QueueLength);

			//Tu już powinny być wszystkie jednostki stworzone
			this.Controller.Update(GameState.TestUnit.CreationTime);
			this.Controller.Update(GameState.TestUnit.CreationTime);
			AreCompleted(tokensP1);
			AreCompleted(tokensP2);
			Assert.AreEqual(6, this.State.Units.Count);
			Assert.AreEqual(3, this.State.Players[0].Units.Count);
			Assert.AreEqual(3, this.State.Players[1].Units.Count);
		}

		#region Helpers
		private static void AreNotCompleted(IUnitRequestToken[] tokens, int start = 0, int end = -1)
		{
			if (end == -1)
				end = tokens.Length;

			for (int i = start; i < end; i++)
			{
				Assert.IsFalse(tokens[i].IsCompleted, "Token: {0}", i);
			}
		}

		private static void AreCompleted(IUnitRequestToken[] tokens, int start = 0, int end = -1)
		{
			if (end == -1)
				end = tokens.Length;

			for (int i = start; i < end; i++)
			{
				Assert.IsTrue(tokens[i].IsCompleted, "Token: {0}", i);
			}
		}
		#endregion
	}
}
