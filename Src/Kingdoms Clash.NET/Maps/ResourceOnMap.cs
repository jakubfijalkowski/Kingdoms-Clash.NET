using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Extensions;
using ClashEngine.NET.Graphics.Resources;
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
			this.Body.SetCollidesWith(Category.None);
			this.Body.SetCollisionCategories(Category.None);

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
			ClashEngine.NET.PhysicsManager.Instance.World.RayCast((fixture, point, n, f) =>
				{
					if (fixture.Body.UserData is ClashEngine.NET.Interfaces.Graphics.Components.ITerrain)
					{
						this.Position.Y = point.Y - desc.Size.Y;
						return 0;
					}
					return -1;
				}, new Microsoft.Xna.Framework.Vector2(this.Position.X + desc.Size.X / 2f, 0),
				new Microsoft.Xna.Framework.Vector2(this.Position.X + desc.Size.X / 2f, float.MaxValue));
			
			//Fizyka
			var pObj = new ClashEngine.NET.Components.PhysicalObject();
			this.Components.Add(pObj);
			this.Body = pObj.Body;

			//Tworzymy figurę dla zasobu
			FarseerPhysics.Common.Vertices verts = new FarseerPhysics.Common.Vertices();
			foreach (var point in desc.Polygon)
			{
				verts.Add(point.ToXNA());
			}
			FarseerPhysics.Factories.FixtureFactory.CreatePolygon(verts, 1, this.Body);

			this.Body.Position = this.Position.ToXNA();
			this.Body.SetCollidesWith(Category.Cat11 | Category.Cat12);
			this.Body.SetCollisionCategories(Category.Cat10);
			this.Body.UserData = this;
			this.Body.FixedRotation = true;


			this.Body.BodyType = BodyType.Dynamic;
			this.Body.FixtureList[0].AfterCollision = (a, b, c) =>
				{
					if (b.Body.UserData is ClashEngine.NET.Interfaces.Graphics.Components.ITerrain)
					{
						this.Body.IsStatic = true;
						this.Components.Add(new ClashEngine.NET.Graphics.Components.Sprite(this.Id, this.GameInfo.Content.Load<Texture>(desc.Image)));
					}
				};

			//Wygląd
			this.Attributes.GetOrCreate<OpenTK.Vector2>("Size").Value = desc.Size;
		}
		#endregion
	}
}
