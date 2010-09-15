using System;
using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Units.Sample
{
	using Interfaces.Units;

	/// <summary>
	/// Przykładowy pracownik.
	/// </summary>
	public class SampleWorkerDescription
		: IWorkerDescription
	{
		#region IWorkerDescription Members

		public int MaxCargoSize
		{
			get { return 10; }
		}

		#endregion

		#region IUnitDescription Members

		public string Id
		{
			get { return "Sample worker"; }
		}

		public IList<Interfaces.Resources.IResource> Costs
		{
			get { throw new NotImplementedException(); }
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
