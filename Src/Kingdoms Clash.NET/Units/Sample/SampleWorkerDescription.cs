using System;
using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Units.Sample
{
	using Interfaces.Units;

	/// <summary>
	/// Przykładowy pracownik.
	/// </summary>
	public class SampleWorkerDescription
		: IUnitDescription
	{
		#region IUnitDescription Members
		public string Id
		{
			get { return "Sample worker"; }
		}

		public IList<Interfaces.Resources.IResource> Costs
		{
			get { throw new NotImplementedException(); }
		}

		public IList<IUnitComponent> Components
		{
			get { return new List<IUnitComponent>(); }
		}

		public int Health
		{
			get { return 100; }
		}
		#endregion
	}
}
