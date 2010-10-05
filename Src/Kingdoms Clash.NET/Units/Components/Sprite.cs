using ClashEngine.NET.Resources;
using ClashEngine.NET.ResourcesManager;
using OpenTK;

namespace Kingdoms_Clash.NET.Units.Components
{
	using Interfaces.Units;
	using Interfaces.Units.Components;

	/// <summary>
	/// Jednostka posiada obrazek(sprite).
	/// </summary>
	/// <remarks>
	/// Wymagane atrybuty:
	/// string Image - ścieżka do obrazka jednostki
	/// </remarks>
	public class Sprite
		: ClashEngine.NET.Components.Sprite, ISprite
	{
		public Sprite()
			: base("Sprite")
		{ }

		public override void OnInit()
		{
			string path = (this.Owner as IUnit).Description.Attributes.Get("Image") as string;

			base.Init(ResourcesManager.Instance.Load<Texture>(path));
			this.Owner.Attributes.GetOrCreate<Vector2>("Size").Value = new Vector2(
				(this.Owner as IUnit).Description.Width,
				(this.Owner as IUnit).Description.Height);

			base.OnInit();
		}

		#region ICloneable Members
		public object Clone()
		{
			return new Sprite();
		}
		#endregion
	}
}
