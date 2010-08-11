using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClashEngine.NET.EntitiesManager
{
	public class EntitiesManager
	{
		#region Properties
		private List<GameEntity> Entities_;

		/// <summary>
		/// Lista encji.
		/// </summary>
		public ReadOnlyCollection<GameEntity> Entities
		{
			get { return this.Entities_.AsReadOnly(); }
		}
		#endregion

		#region Methods
		public void AddEntity(GameEntity entity)
		{ }

		public void RemoveEntity(GameEntity entity)
		{ }

		public void Update(double delta)
		{ }

		public void Render()
		{ }
		#endregion
	}
}
