using System.ComponentModel;
using System.Diagnostics;

namespace ClashEngine.NET.EntitiesManager
{
	using Extensions;
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Atrybut encji gry.
	/// </summary>
	[DebuggerDisplay("{Id,nq} = {Value,nq}")]
	public class Attribute
		: IAttribute
	{
		#region Private fields
		private object _Value = null;
		#endregion

		#region IAttribute Members
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
				this.PropertyChanged.Raise(this, () => Value);
			}
		}
		#endregion

		#region INotifyPropertyChanged Members
		/// <summary>
		/// Wywoływane przy zmianie Value.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Constructors
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
		#endregion
	}
}
