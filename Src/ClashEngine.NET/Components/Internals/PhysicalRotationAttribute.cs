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
	class PhysicalRotationAttribute
		: IAttribute<float>
	{
		private Body Body;

		#region IAttribute<float> Members
		public float Value
		{
			get
			{
				return this.Body.Rotation;
			}
			set
			{
				this.Body.Rotation = value;
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
				if (!(value is float))
				{
					throw new ArgumentException("Must be of type float", "value");
				}
				this.Body.Rotation = (float)value;
				this.PropertyChanged.Raise(this, () => Value);
			}
		}
		#endregion

		#region INotifyPropertyChanged Members
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		#endregion

		public PhysicalRotationAttribute(string id, Body body)
		{
			this.Id = id;
			this.Body = body;
		}
	}
}
