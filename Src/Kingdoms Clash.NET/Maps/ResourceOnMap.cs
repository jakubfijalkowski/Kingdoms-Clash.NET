﻿using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Resources;
using ClashEngine.NET.ResourcesManager;
using ClashEngine.NET.Utilities;
using FarseerPhysics.Dynamics;

namespace Kingdoms_Clash.NET.Maps
{
	using Interfaces.Map;
	using Resources;

	/// <summary>
	/// Klasa implementująca interfejs "zasobu na mapie".
	/// Zasób wyświetlany jest jako duszek.
	/// </summary>
	public class ResourceOnMap
		: GameEntity, IResourceOnMap
	{
		#region Private Fields
		/// <summary>
		/// Pozycja zasobu.
		/// </summary>
		private OpenTK.Vector2 Position;

		/// <summary>
		/// Ciało fizyczne zasobu.
		/// </summary>
		private Body Body;
		#endregion

		#region IResourceOnMap Members
		/// <summary>
		/// Stan gry, do którego przynależy zasób.
		/// </summary>
		public Interfaces.IGameState GameState { get; set; }

		/// <summary>
		/// Wartość.
		/// </summary>
		public uint Value { get; set; }

		/// <summary>
		/// Zbiera zasób z mapy.
		/// </summary>
		public void Gather()
		{
			//Usuwamy kolizję, zabezpieczy to nas przed podwójnym zebraniem
			this.Body.SetCollidesWith(CollisionCategory.None);
			this.Body.SetCollisionCategories(CollisionCategory.None);

			//Usuwamy z gry
			this.GameState.Remove(this);
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje zasób.
		/// </summary>
		/// <param name="id">Identyfikator zasobu(np. wood).</param>
		/// <param name="value">Jego wartość.</param>
		/// <param name="x">Pozycja(lewa krawędź).</param>
		public ResourceOnMap(string id, uint value, float x)
			: base(id)
		{
			this.Value = value;
			this.Position = new OpenTK.Vector2(x, 0f);
		}
		#endregion

		#region GameEntity Members
		public override void OnInit()
		{
			var desc = ResourcesList.Instance[this.Id];
			this.Position.Y = this.GameState.Map.GetHeight(this.Position.X, this.Position.X + desc.Size.X) - desc.Size.Y;

			//Fizyka
			var pObj = new ClashEngine.NET.PhysicsManager.PhysicalObject();
			this.Components.Add(pObj);
			this.Components.Add(new ClashEngine.NET.Components.Physical.BoundingBox(desc.Size));
			this.Body = pObj.Body;

			pObj.Body.Position = this.Position.ToXNA();
			pObj.Body.SetCollidesWith(CollisionCategory.Cat11 | CollisionCategory.Cat12);
			pObj.Body.SetCollisionCategories(CollisionCategory.Cat10);
			pObj.Body.UserData = this;

			//Wygląd
			this.Components.Add(new ClashEngine.NET.Components.Sprite(this.Id, ResourcesManager.Instance.Load<Texture>(desc.Image)));
			this.Attributes.GetOrCreate<OpenTK.Vector2>("Size").Value = desc.Size;
		}
		#endregion
	}
}