using System.Diagnostics;

namespace ClashEngine.NET.EntitiesManager
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Atrybut encji gry.
	/// </summary>
	[DebuggerDisplay("{Id,nq} = {Value,nq}")]
	public class Attribute
		: IAttribute
	{
		private object _Value = null;

		/// <summary>
		/// Identyfikator atrybutu.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Wartość atrybutu.
		/// </summary>
		public object Value
		{
			get { return this._Value; }
			set
			{
				this._Value = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged(this);
				}
			}
		}

		/// <summary>
		/// Zdarzenie wywoływane przy zmianie wartości atrybutu.
		/// </summary>
		public event ValueChangedDelegate ValueChanged;

		/// <summary>
		/// Inicjalizuje nowy atrybut.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="value">Wartość.</param>
		public Attribute(string id, object value)
		{
			this.Id = id;
			this._Value = value;
		}
	}
}
