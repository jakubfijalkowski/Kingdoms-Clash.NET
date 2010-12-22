using System;
using System.Diagnostics;
using FarseerPhysics.Dynamics;
using OpenTK;

namespace ClashEngine.NET.Components.Internals
{
	using Extensions;
	using Interfaces.EntitiesManager;
	using Utilities;

	/// <summary>
	/// Wewnętrzna klasa zamieniająca normalne atrybuty w atrybuty "fizyczne" - pobierane z obiektów fizycznych.
	/// Pobiera atrybut z FarseerPhysics.Dynamics.Body.
	/// </summary>
	[DebuggerDisplay("{Id,nq} = {Value}")]
	class PhysicalPositionAttribute
		: IAttribute<Vector2>
	{
		private Body Body;

		#region IAttribute<Vector2> Members
		public Vector2 Value
		{
			get
			{
				return this.Body.Position.ToOpenTK();
			}
			set
			{
				this.Body.Position = value.ToXNA();
				this.PropertyChanged.Raise(this, () => Value);
			}
		}
		#endregion

		#region IAttribute Members
		public string Id { get; private set; }

		object IAttribute.Value
		{
			get
			{
				return this.Body.Position;
			}
			set
			{
				if (!(value is Microsoft.Xna.Framework.Vector2))
				{
					throw new ArgumentException("Must be of type Microsoft.Xna.Framework.Vector2", "value");
				}
				this.Body.Position = (Microsoft.Xna.Framework.Vector2)value;
				this.PropertyChanged.Raise(this, () => Value);
			}
		}
		#endregion

		#region INotifyPropertyChanged Members
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		#endregion

		public PhysicalPositionAttribute(string id, Body body)
		{
			this.Id = id;
			this.Body = body;
		}
	}
}
