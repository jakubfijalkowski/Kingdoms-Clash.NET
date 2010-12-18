using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Markup;

namespace ClashEngine.NET.Graphics.Gui.Conditions
{
	using Interfaces.Graphics.Gui.Conditions;

	/// <summary>
	/// Parametr dla Call.
	/// </summary>
	[ContentProperty("Value")]
	public class Parameter
		: IParameter
	{
		#region Proivate fields
		private object _Value = null;
		#endregion

		#region IParameter Members
		/// <summary>
		/// Wartość parametru.
		/// </summary>
		public object Value
		{
			get { return this._Value; }
			set
			{
				if (this._Value != value)
				{
					this._Value = value;
					if (this.PropertyChanged != null)
					{
						this.PropertyChanged(this, new PropertyChangedEventArgs("Value"));
					}
				}
			}
		} 
		#endregion

		#region INotifyPropertyChanged Members
		/// <summary>
		/// Wywoływane przy zmianie wartości Value.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}

	/// <summary>
	/// Kolekcja parametrów.
	/// </summary>
	public class ParametersCollection
		: IParametersCollection
	{
		private List<IParameter> Parameters = new List<IParameter>();

		#region IParametersCollection Members
		/// <summary>
		/// Pobiera parametr o wskazanym indeksie.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public IParameter this[int index] { get { return this.Parameters[index]; } }
		#endregion

		#region ICollection<IParameter> Members
		/// <summary>
		/// Dodaje element do kolekcji.
		/// </summary>
		/// <param name="item"></param>
		public void Add(IParameter item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.Parameters.Add(item);
		}

		/// <summary>
		/// Usuwa element z kolekcji.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(IParameter item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.Parameters.Remove(item);
		}

		/// <summary>
		/// Czyści kolekcję.
		/// </summary>
		public void Clear()
		{
			this.Parameters.Clear();
		}

		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się wskazany element.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(IParameter item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.Parameters.Contains(item);
		}

		/// <summary>
		/// Kopiuje kolekcję do tablicy.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		void ICollection<IParameter>.CopyTo(IParameter[] array, int arrayIndex)
		{
			this.Parameters.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Pobiera ilość elementów.
		/// </summary>
		public int Count
		{
			get { return this.Parameters.Count; }
		}

		/// <summary>
		/// Czy kolekcja jest tylko do odczytu - zawsze false.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region IEnumerable<IParameter> Members
		public IEnumerator<IParameter> GetEnumerator()
		{
			return this.Parameters.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Parameters.GetEnumerator();
		}
		#endregion
	}
}
