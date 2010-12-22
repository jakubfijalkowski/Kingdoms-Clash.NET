using System;
using System.Collections.Generic;

namespace ClashEngine.NET.Graphics.Gui.Conditions
{
	using Extensions;
	using Interfaces.Graphics.Gui.Conditions;

	/// <summary>
	/// Podwarunek zbiorczy dla IMultiIf.
	/// </summary>
	public class Conditions
		: IMultiIfConditionsCollection
	{
		#region Private fields
		private List<IMultiIfCondition> ConditionsList = new List<IMultiIfCondition>();
		private int TruesCount = 0;
		private bool _Value = false;
		#endregion

		#region IMultiIfCondition Members
		/// <summary>
		/// Aktualna wartość wyrażenia.
		/// </summary>
		public bool Value
		{
			get { return this._Value; }
			set
			{
				if (value != this._Value)
				{
					this._Value = value;
					this.PropertyChanged.Raise(this, () => Value);
				}
			}
		}
		#endregion

		#region INotifyPropertyChanged Members
		/// <summary>
		/// Wywoływane przy zmianie wartości Value.
		/// </summary>
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region IMultiIfConditionsCollection Members
		/// <summary>
		/// Typ łączenia warunków.
		/// </summary>
		public MultiIfConditionType Type { get; set; }
		#endregion

		#region ICollection<IMultiIfCondition> Members
		/// <summary>
		/// Dodaje element do kolekcji.
		/// </summary>
		/// <param name="item"></param>
		public void Add(IMultiIfCondition item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			item.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ItemValueChanged);
			this.ConditionsList.Add(item);
		}

		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się podany element.
		/// </summary>
		/// <param name="item">Element.</param>
		/// <returns></returns>
		public bool Contains(IMultiIfCondition item)
		{
			return this.ConditionsList.Contains(item);
		}

		/// <summary>
		/// Kopiuje kolekcje do tablicy.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(IMultiIfCondition[] array, int arrayIndex)
		{
			this.ConditionsList.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Liczba elementów w kolekcji.
		/// </summary>
		public int Count
		{
			get { return this.ConditionsList.Count; }
		}

		/// <summary>
		/// Nieużywane - zawsze false.
		/// </summary>
		bool ICollection<IMultiIfCondition>.IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Niewspierane.
		/// </summary>
		public void Clear()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Niewspierane.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(IMultiIfCondition item)
		{
			throw new NotSupportedException();
		}
		#endregion

		#region IEnumerable<IMultiIfCondition> Members
		public IEnumerator<IMultiIfCondition> GetEnumerator()
		{
			return this.ConditionsList.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.ConditionsList.GetEnumerator();
		}
		#endregion

		#region Private methods
		private void ItemValueChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Value")
			{
				if ((sender as IMultiIfCondition).Value)
				{
					++this.TruesCount;
				}
				else
				{
					--this.TruesCount;
				}

				switch (this.Type)
				{
				case MultiIfConditionType.And:
					this.Value = this.TruesCount == this.ConditionsList.Count;
					break;
				case MultiIfConditionType.Or:
					this.Value = this.TruesCount > 0;
					break;
				}
			}
		}
		#endregion
	}
}
