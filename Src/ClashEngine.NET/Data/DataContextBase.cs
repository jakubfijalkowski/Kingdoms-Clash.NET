using System.ComponentModel;
using System.Windows.Markup;

namespace ClashEngine.NET.Data
{
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
					this.SendPropertyChanged("DataContext");
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

		/// <summary>
		/// Jako iż nie da się wysyłać zdarzeń z klas dziedziczących - musimy to umożlwić.
		/// </summary>
		/// <param name="propertyName"></param>
		protected void SendPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
