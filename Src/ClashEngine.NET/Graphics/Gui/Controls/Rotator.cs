using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Markup;

namespace ClashEngine.NET.Graphics.Gui.Controls
{
	using Interfaces.Graphics.Gui;
	using Interfaces.Graphics.Gui.Controls;

	/// <summary>
	/// Kontrolka - rotator.
	/// </summary>
	[ContentProperty("Items")]
	[RuntimeNameProperty("Id")]
	public class Rotator
		: IRotator, ISupportInitialize
	{
		#region Private fields
		private int _First = 0;
		private IContainerControl _Owner = null;
		private IUIData _Data = null;
		private object _DataContext = null;
		#endregion

		#region IControl Members
		/// <summary>
		/// Identyfikator.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Właściciel kontrolki.
		/// </summary>
		IContainerControl IControl.Owner
		{
			get { return this._Owner; }
			set { this._Owner = value; }
		}

		/// <summary>
		/// Dane UI.
		/// </summary>
		IUIData IControl.Data
		{
			get { return this._Data; }
			set { this._Data = value; }
		}

		/// <summary>
		/// Offset dla kontrolki ustawiany przez kontener.
		/// Nieużywane - zawsze 0,0.
		/// </summary>
		public OpenTK.Vector2 ContainerOffset
		{
			get { return new OpenTK.Vector2(0, 0); }
			set { }
		}

		/// <summary>
		/// Pozycja.
		/// Nieużywane - zawsze 0,0.
		/// </summary>
		public OpenTK.Vector2 Position
		{
			get { return new OpenTK.Vector2(0, 0); }
			set { }
		}

		/// <summary>
		/// Pozycja kontrolki - absoulutna, uwzględnia offset kontenera.
		/// Nieużywane - zawsze 0,0.
		/// </summary>
		public virtual OpenTK.Vector2 AbsolutePosition
		{
			get { return new OpenTK.Vector2(0, 0); }
		}

		/// <summary>
		/// Rozmiar - nieużywane, zawsze 0x0.
		/// </summary>
		public OpenTK.Vector2 Size
		{
			get { return new OpenTK.Vector2(0, 0); }
			set { }
		}

		/// <summary>
		/// Nieużywane - zawsze false.
		/// </summary>
		public bool Visible
		{
			get { return false; }
			set { }
		}

		/// <summary>
		/// Zawsze false.
		/// </summary>
		public bool PermanentActive
		{
			get { return false; }
		}

		/// <summary>
		/// Wywoływane przy dodaniu do kontenera.
		/// </summary>
		public void OnAdd()
		{ }

		/// <summary>
		/// Wywoływane przy usunięciu z kontenera.
		/// </summary>
		public void OnRemove()
		{ }

		/// <summary>
		/// Nieużywane - zawsze false.
		/// </summary>
		/// <returns></returns>
		public bool ContainsMouse()
		{
			return false;
		}

		/// <summary>
		/// Nieużywane - zawsze 0.
		/// </summary>
		/// <returns></returns>
		public int Check()
		{
			return 0;
		}

		/// <summary>
		/// Nieużywane.
		/// </summary>
		public void Render()
		{ }

		/// <summary>
		/// Nieużywane.
		/// </summary>
		/// <param name="delta"></param>
		public void Update(double delta)
		{ }
		#endregion

		#region IRotator Members
		/// <summary>
		/// Obiekty rotatora.
		/// </summary>
		public IRotatorObjectsCollection Items { get; private set; }

		/// <summary>
		/// Liczba elementów które mogą być aktualnie wyświetlane.
		/// </summary>
		public uint MaxSelectedItems { get; set; }

		/// <summary>
		/// Pierwszy wybrany element.
		/// </summary>
		public int First
		{
			get { return this._First; }
			set
			{
				if (value > this.Items.Count - this.MaxSelectedItems)
				{
					value = (int)(this.Items.Count - MaxSelectedItems);
				}
				if (value < 0)
					value = 0;
				if (this._First != value)
				{
					this._First = value;
					(this.Selected as Internals.RotatorSelectedItems).RaiseChanged();
				}
			}
		}

		/// <summary>
		/// Pobiera jeden z wybranych elementów.
		/// </summary>
		/// <remarks>Elementy mogą być puste.</remarks>
		/// <param name="index">Indeks. Od 0 do SelectedItemsCount.</param>
		/// <returns></returns>
		public object this[int index]
		{
			get
			{
				if (index > this.MaxSelectedItems)
				{
					throw new IndexOutOfRangeException("Index must be less than SelectedItemsCount");
				}
				return this.Items[this.First + index];
			}
		}

		/// <summary>
		/// Aktualnie wybrane elementy.
		/// </summary>
		public IRotatorSelectedItems Selected { get; private set; }
		#endregion

		#region IDataContext Members
		/// <summary>
		/// Kontekst danych dla rotatora.
		/// </summary>
		/// <remarks>
		/// Obiekt musi implementować IEnumerable.
		/// Można ustawić tylko raz.
		/// </remarks>
		public object DataContext
		{
			get { return this._DataContext; }
			set
			{
				if (this._DataContext != null)
				{
					throw new InvalidOperationException("DataContext can be specified only once");
				}
				if (value != null && !(value is IEnumerable))
				{
					throw new ArgumentException("DataContext must implement IEnumerable", "value");
				}
				this._DataContext = value;
				if (this._DataContext != null)
				{
					foreach (object item in (value as IEnumerable))
					{
						this.Items.Add(item);
					}
				}
			}
		}
		#endregion

		#region ISupportInitialize Members
		/// <summary>
		/// Nieużywane.
		/// </summary>
		public void BeginInit()
		{ }

		/// <summary>
		/// Sprawdza, czy Id nie jest puste.
		/// </summary>
		public void EndInit()
		{
			if (string.IsNullOrWhiteSpace(this.Id))
			{
				throw new System.InvalidOperationException("Cannot create control with empty Id");
			}
		}
		#endregion

		#region INotifyPropertyChanged Members
		/// <summary>
		/// Wywoływane przy zmianie właściwości.
		/// </summary>
		#pragma warning disable 0067
		public event PropertyChangedEventHandler PropertyChanged;
		#pragma warning restore 0067
		#endregion

		#region Constructors
		public Rotator()
		{
			this.Items = new Internals.RotatorObjectsCollection(this);
			this.Selected = new Internals.RotatorSelectedItems(this);
		}
		#endregion

		#region Internals
		/// <summary>
		/// Używane przez RotatorObjectsCollection.
		/// </summary>
		/// <param name="index"></param>
		internal void SendItemChanged(int index)
		{
			if (index == -1)
			{
				(this.Selected as Internals.RotatorSelectedItems).RaiseChanged();
			}
			if (index >= this.First && index < this.First + this.MaxSelectedItems)
			{
				(this.Selected as Internals.RotatorSelectedItems).RaiseChanged(index - this.First);
			}
		}
		#endregion
	}
}
