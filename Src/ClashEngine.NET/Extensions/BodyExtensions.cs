using FarseerPhysics.Dynamics;

namespace ClashEngine.NET.Extensions
{
	/// <summary>
	/// Metody rozszerzające dla ciała fizycznego.
	/// </summary>
	public static class BodyExtensions
	{
		#region Collision categories
		/// <summary>
		/// Pobiera kategorie kolizji dla ciała(z pierwszego fixture).
		/// </summary>
		/// <param name="body">this</param>
		/// <returns>Kategorie kolizji lub All, gdy nie dodano jeszcze żadnej fixture.</returns>
		public static CollisionCategory GetCollisionCategories(this Body body)
		{
			if (body.FixtureList.Count > 0)
			{
				return body.FixtureList[0].CollisionCategories;
			}
			return CollisionCategory.All;
		}

		/// <summary>
		/// Ustawia kategorię kolizji dla wszystkich fixture.
		/// </summary>
		/// <param name="body">this</param>
		/// <param name="categories">Kategorie.</param>
		public static void SetCollisionCategories(this Body body, CollisionCategory categories)
		{
			foreach (var f in body.FixtureList)
			{
				f.CollisionCategories = categories;
			}
		}

		/// <summary>
		/// Dodaje kategorię kolizji dla wszystkich fixture.
		/// </summary>
		/// <param name="body">this</param>
		/// <param name="categories">Kategorie do dodania.</param>
		public static void AddCollisionCategories(this Body body, CollisionCategory categories)
		{
			foreach (var f in body.FixtureList)
			{
				f.CollisionCategories |= categories;
			}
		}

		/// <summary>
		/// Usuwa kategorie kolizji ze wszystkich fixture.
		/// </summary>
		/// <param name="body">this</param>
		/// <param name="categories">Kategorie do usunięcia.</param>
		public static void RemoveCollisionCategories(this Body body, CollisionCategory categories)
		{
			foreach (var f in body.FixtureList)
			{
				f.CollisionCategories &= ~categories;
			}
		}
		#endregion

		#region Collides with
		/// <summary>
		/// Pobiera kategorie z którymi koliduje ciało(z pierwszego fixture).
		/// </summary>
		/// <param name="body"></param>
		/// <returns>Kategorie, z którymi koliduje.</returns>
		public static CollisionCategory GetCollidesWith(this Body body)
		{
			if (body.FixtureList.Count > 0)
			{
				return body.FixtureList[0].CollidesWith;
			}
			return CollisionCategory.All;
		}

		/// <summary>
		/// Ustawia kategorię kolizji z którymi koliduje ciało.
		/// </summary>
		/// <param name="body">this</param>
		/// <param name="categories">Kategorie do ustawienia.</param>
		public static void SetCollidesWith(this Body body, CollisionCategory categories)
		{
			foreach (var f in body.FixtureList)
			{
				f.CollidesWith = categories;
			}
		}

		/// <summary>
		/// Dodaje kategorię kolizji z którymi koliduje ciało.
		/// </summary>
		/// <param name="body">this</param>
		/// <param name="categories">Kategorie do dodania.</param>
		public static void AddCollidesWith(this Body body, CollisionCategory categories)
		{
			foreach (var f in body.FixtureList)
			{
				f.CollidesWith |= categories;
			}
		}

		/// <summary>
		/// Usuwa kategorię kolizji z którymi koliduje ciało.
		/// </summary>
		/// <param name="body">this</param>
		/// <param name="categories">Kategorie do usunięcia.</param>
		public static void RemoveCollidesWith(this Body body, CollisionCategory categories)
		{
			foreach (var f in body.FixtureList)
			{
				f.CollidesWith &= ~categories;
			}
		}
		#endregion

		#region Collision group
		/// <summary>
		/// Pobiera grupę kolizji dla ciała(z pierwszego fixture).
		/// </summary>
		/// <param name="body">this</param>
		/// <returns>Grupa.</returns>
		public static short GetCollisionGroup(this Body body)
		{
			if (body.FixtureList.Count > 0)
			{
				return body.FixtureList[0].CollisionGroup;
			}
			return 0;
		}

		/// <summary>
		/// Ustawia grupę kolizji ciała.
		/// </summary>
		/// <param name="body">this</param>
		/// <param name="group">Grupa.</param>
		public static void SetCollisionGroup(this Body body, short group)
		{
			foreach (var f in body.FixtureList)
			{
				f.CollisionGroup = group;
			}
		}
		#endregion

		#region Collision events
		/// <summary>
		/// Ustawia zdarzenie kolizji dla danego ciała.
		/// </summary>
		/// <param name="body">this</param>
		/// <param name="eventHandler">Metoda obsługująca to zdarzenie.</param>
		public static void SetCollisionEvent(this Body body, CollisionEventHandler eventHandler)
		{
			foreach (var f in body.FixtureList)
			{
				f.OnCollision = eventHandler;
			}
		}
		#endregion
	}
}
