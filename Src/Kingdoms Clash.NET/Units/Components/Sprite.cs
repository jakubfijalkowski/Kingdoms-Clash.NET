using System.Diagnostics;
using ClashEngine.NET.Resources;
using ClashEngine.NET.ResourcesManager;
using OpenTK;

namespace Kingdoms_Clash.NET.Units.Components
{
	using Interfaces.Units;
	using Interfaces.Units.Components;

	/// <summary>
	/// Opis komponentu dla jednostki posiadającej obrazek.
	/// </summary>
	[DebuggerDisplay("Sprite, Image = {ImagePath,nq}")]
	public class Sprite
		: ISprite
	{
		#region ISprite Members
		/// <summary>
		/// Ścieżka do obrazka
		/// </summary>
		public string ImagePath { get; private set; }
		#endregion

		#region IUnitComponentDescription Members
		/// <summary>
		/// Tworzy komponent na podstawie opisu.
		/// </summary>
		/// <returns>Komponent.</returns>
		public IUnitComponent Create()
		{
			return new SpriteComponent() { Description = this };
		}
		#endregion

		#region IXmlSerializable Members
		public void Serialize(System.Xml.XmlElement element)
		{
			element.SetAttribute("image", this.ImagePath);
		}

		public void Deserialize(System.Xml.XmlElement element)
		{
			if (element.HasAttribute("image"))
			{
				this.ImagePath = element.GetAttribute("image");
			}
			else
			{
				throw new System.Xml.XmlException("Insufficient data: image");
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje opis.
		/// </summary>
		/// <param name="image">Ścieżka do obrazka.</param>
		public Sprite(string image)
		{
			this.ImagePath = image;
		}

		/// <summary>
		/// Inicjalizuje opis domyślnymi wartościami.
		/// </summary>
		public Sprite()
		{
			this.ImagePath = string.Empty;
		}
		#endregion

		#region Component
		/// <summary>
		/// Klasa właściwego komponentu - nie musi być widoczna publicznie.
		/// </summary>
		private class SpriteComponent
			: ClashEngine.NET.Components.Sprite, IUnitComponent
		{
			#region IUnitComponent Members
			/// <summary>
			/// Opis komponentu.
			/// </summary>
			public IUnitComponentDescription Description { get; set; }
			#endregion

			#region Sprite Members
			public override void OnInit()
			{
				base.Init(ResourcesManager.Instance.Load<Texture>((this.Description as ISprite).ImagePath));
				this.Owner.Attributes.GetOrCreate<Vector2>("Size").Value = new Vector2(
					(this.Owner as IUnit).Description.Width,
					(this.Owner as IUnit).Description.Height);

				base.OnInit();
			}
			#endregion

			public SpriteComponent()
				: base("UnitSprite")
			{ }
		}
		#endregion
	}
}
