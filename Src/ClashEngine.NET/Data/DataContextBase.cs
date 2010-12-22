using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Markup;

namespace ClashEngine.NET.Data
{
	using Extensions;
	using Interfaces.Data;

	/// <summary>
	/// Bazowa klasa dla klas z kontekstami danych.
	/// </summary>
	public abstract class DataContextBase
		: IDataContext
	{
		#region Private fields
		private object _DataContext = null;
		#endregion

		#region IDataContext Members
		/// <summary>
		/// Kontekst danych.
		/// </summary>
		[TypeConverter(typeof(NameReferenceConverter))]
		public object DataContext 
		{
			get { return this._DataContext; }
			set
			{
				if (value != this._DataContext)
				{
					this._DataContext = value;
					this.RaisePropertyChanged(() => DataContext);
				}
			}
		}
		#endregion

		#region INotifyPropertyChanged Members
		/// <summary>
		/// Wywoływane przy zmianie którejś z właściwości.
		/// </summary>
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Raising events
		/// <summary>
		/// Wysyła zdarzenie PropertyChanged.
		/// </summary>
		/// <param name="propertyExpression"></param>
		protected void RaisePropertyChanged(Expression<Func<object>> propertyExpression)
		{
			this.PropertyChanged.Raise(this, propertyExpression);
		}
		#endregion
	}
}
