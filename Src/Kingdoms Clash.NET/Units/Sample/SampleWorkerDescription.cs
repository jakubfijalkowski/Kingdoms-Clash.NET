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
		IList<IUnitComponent> Components_ = new List<IUnitComponent>(new IUnitComponent[]
		{
			new Kingdoms_Clash.NET.Units.Components.Movable(),
			new Kingdoms_Clash.NET.Units.Components.StaticImage()
		});

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
			get { return this.Components_; }
		}

		public int Health
		{
			get { return 100; }
		}

		public T GetAttribute<T>(string name)
			where T : IConvertible
		{
			switch (name)
			{
			case "Speed":
				return (T)(IConvertible)0.2f;

			case "ImageHeight":
			case "ImageWidth":
				return (T)(IConvertible)0.05f;

			case "ImagePath":
				return (T)(IConvertible)"SampleSprite.png";
			}
			return default(T);
		}
		#endregion
	}
}
