using Kingdoms_Clash.NET.Interfaces.Resources;
using Kingdoms_Clash.NET.Interfaces.Units;
using Kingdoms_Clash.NET.Units;
using NUnit.Framework;

namespace Kingdoms_Clash.NET.Tests
{

	[TestFixture(Description = "Testy dla klasy Worker")]
	public class WorkerTests
	{
		private IWorkerDescription Description;
		private Worker Worker;

		[SetUp]
		public void SetUp()
		{
			this.Description = new WorkerDescription();
			this.Worker = new Worker(this.Description, null);
			this.Worker.Init(null);
		}

		[Test]
		public void WorkerHasSameAttributesAsDescription()
		{

			Assert.AreEqual(this.Description.Health, this.Worker.Health);
			Assert.AreEqual(this.Description.Speed, this.Worker.Speed);
			Assert.AreEqual(this.Description.MaxCargoSize, this.Worker.MaxCargoSize);
		}
	}

	internal class WorkerDescription
		: IWorkerDescription
	{
		#region IWorkerDescription Members

		public int MaxCargoSize
		{
			get { return 30; }
		}

		#endregion

		#region IUnitDescription Members

		public string Id
		{
			get { return "TestWorker"; }
		}

		public System.Collections.Generic.IList<IResource> Costs
		{
			get { throw new System.NotImplementedException(); }
		}

		public UnitType Type
		{
			get { return UnitType.Worker; }
		}

		public int Health
		{
			get { return 100; }
		}

		public int Speed
		{
			get { return 5; }
		}

		#endregion
	}
}
