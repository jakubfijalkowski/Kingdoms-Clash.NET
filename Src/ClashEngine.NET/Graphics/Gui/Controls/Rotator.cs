using System;
using System.Windows.Markup;

namespace ClashEngine.NET.Graphics.Gui.Controls
{
	using Interfaces.Graphics.Gui.Controls;

	/// <summary>
	/// Kontrolka - rotator.
	/// </summary>
	[ContentProperty("Items")]
	public class Rotator
		: ControlBase, IRotator
	{
		#region Private fields
		private int _First = 0;
		#endregion

		#region IRotator Members
		/// <summary>
		/// Obiekty rotatora.
		/// </summary>
		public IRotatorObjectsCollection Items { get; private set; }

		/// <summary>
		/// Liczba elementów które mogą być aktualnie wyświetlane.
		/// </summary>
		public uint SelectedItemsCount { get; set; }

		/// <summary>
		/// Pierwszy wybrany element.
		/// </summary>
		public int First
		{
			get { return this._First; }
			set
			{
				if (value > this.Items.Count - this.First)
				{
					value = this.Items.Count - this.First;
				}
				if (value < 0)
					value = 0;
				this._First = value;
				this.SendPropertyChanged("Item");
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
				if (index > this.SelectedItemsCount)
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

		#region Unused
		public override bool PermanentActive { get { return false; } }

		public override bool ContainsMouse()
		{
			return false;
		}

		public override int Check()
		{ return 0; }
		#endregion

		#region Constructors
		public Rotator()
		{
			this.Items = new Internals.RotatorObjectsCollection();
			this.Selected = new Internals.RotatorSelectedItems(this);
			base.Size = new OpenTK.Vector2(1, 1);
		}
		#endregion
	}
}
