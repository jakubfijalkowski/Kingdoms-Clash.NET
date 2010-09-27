using ClashEngine.NET.Components;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.Resources;
using ClashEngine.NET.ResourcesManager;

namespace Kingdoms_Clash.NET.Units.Components
{
	using Interfaces.Units;
	using Interfaces.Units.Components;

	/// <summary>
	/// Jednostka posiada statyczny obrazek(sprite).
	/// </summary>
	/// <remarks>
	/// Użyte atrybuty opisu jednostki:
	/// string ImagePath
	/// float ImageWidth
	/// float ImageHeight
	/// </remarks>
	public class StaticImage
		: Sprite, IStaticImage
	{
		public StaticImage()
			: base("StaticImage")
		{ }

		/// <summary>
		/// Inicjalizuje duszka jednostki - jego teksturę i rozmiar.
		/// </summary>
		/// <param name="owner"></param>
		public override void Init(IGameEntity owner)
		{
			base.Init(owner);

			float width = (this.Owner as IUnit).Description.GetAttribute<float>("ImageWidth");
			float height = (this.Owner as IUnit).Description.GetAttribute<float>("ImageHeight");
			string imagePath = (this.Owner as IUnit).Description.GetAttribute<string>("ImagePath");

			base.Init(ResourcesManager.Instance.Load<Texture>(imagePath));
			base.Size = new OpenTK.Vector2(width, height);
		}

		#region ICloneable Members
		object System.ICloneable.Clone()
		{
			return new StaticImage();
		}
		#endregion
	}
}
