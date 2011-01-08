using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ClashEngine.NET.Graphics.Gui.Internals
{
	using Extensions;
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Kontener na kontrolki.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(ControlsCollectionDebugView))]
	internal class ControlsCollection
		: KeyedCollection<string, IControl>, IControlsCollection
	{
		#region Private fields
		private Dictionary<string, IControl> ChildControls = new Dictionary<string, IControl>();
		#endregion

		#region IControlsCollection Members
		/// <summary>
		/// Właściciel.
		/// </summary>
		public IContainerControl Owner { get; private set; }

		/// <summary>
		/// Całkowity rozmiar - uwzględnia dzieci.
		/// </summary>
		public int TotalCount
		{
			get { return this.Count + this.ChildControls.Count; }
		}

		/// <summary>
		/// Pobiera kontrolkę o wskazanym Id.
		/// Może to być kontrolka-dziecko, albo kontrolka zawarta w kontrolce-dziecku.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">Rzucane, gdy nie znaleziono kontrolki.</exception>
		/// <returns>Kontrolka.</returns>
		public IControl Get(string id)
		{
			if (this.Contains(id))
			{
				return this[id];
			}
			return this.ChildControls[id];
		}

		/// <summary>
		/// Dodaje listę kontrolek do kolekcji.
		/// </summary>
		/// <param name="items">Lista.</param>
		public void AddRange(IEnumerable<IControl> items)
		{
			foreach (var item in items)
			{
				if (this.Contains(item.Id))
				{
					throw new Exceptions.ArgumentAlreadyExistsException("item");
				}
				base.Add(item);
			}
		}

		/// <summary>
		/// Dodaje kontrolkę, która jest w kontrolce niżej.
		/// </summary>
		/// <param name="control"></param>
		public void AddChild(IControl control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("item");
			}
			if (this.Contains(control.Id))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("item");
			}
			control.Data = this.Owner.Data; //Dziedziczymy Data od głównej kontrolki
			this.ChildControls.Add(control.Id, control);
			if (this.Owner.Owner != null)
			{
				this.Owner.Owner.Controls.AddChild(control);
			}
		}

		/// <summary>
		/// Usuwa kontrolkę, która jest w kontrolce niżej.
		/// </summary>
		/// <param name="control"></param>
		public bool RemoveChild(IControl control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.ChildControls.Remove(control.Id);
		}
		#endregion

		#region KeyedCollection Members
		protected override string GetKeyForItem(IControl item)
		{
			if (string.IsNullOrWhiteSpace(item.Id))
			{
				throw new ArgumentNullException("item.Id");
			}
			return item.Id;
		}

		protected override void InsertItem(int index, IControl item)
		{
			item.Owner = this.Owner;
			item.Data = this.Owner.Data;
			item.ContainerOffset = this.Owner.AbsolutePosition;
			item.PropertyChanged += this.ChildVisibilityChanged;

			base.InsertItem(index, item);
			item.OnAdd();
			if (this.Owner.Owner != null)
			{
				this.Owner.Owner.Controls.AddChild(item);
			}
		}

		protected override void RemoveItem(int index)
		{
			var item = this[index];
			if (this.Owner.Owner != null)
			{
				this.Owner.Owner.Controls.RemoveChild(item);
			}
			item.PropertyChanged -= this.ChildVisibilityChanged;
			item.OnRemove();
			base.RemoveItem(index);
		}

		protected override void SetItem(int index, IControl item)
		{
			var oldItem = this[index];
			if (this.Owner.Owner != null)
			{
				this.Owner.Owner.Controls.Remove(oldItem);
				this.Owner.Owner.Controls.AddChild(item);
			}
			base.SetItem(index, item);
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje kolekcje kontrolek.
		/// </summary>
		/// <param name="owner">Właściciel.</param>
		public ControlsCollection(IContainerControl owner)
		{
			this.Owner = owner;
			this.Owner.PropertyChanged += this.OwnerChanged;
			this.Owner.PropertyChanged += this.AbsolutePositionChanged;
		}
		#endregion

		#region Events
		/// <summary>
		/// Zmienił się właściciel(może nastąpić raz) - dodajemy swoje kontrolki jako dziedzi dzieci do rodzica.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OwnerChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == this.Owner.NameOf(_ => _.Owner))
			{
				if (this.Owner.Owner != null)
				{
					foreach (var control in base.Items)
					{
						this.Owner.Owner.Controls.AddChild(control);
					}
				}
			}
		}

		/// <summary>
		/// Jeśli zmieniła się absolutna pozycja kontrolki-rodzica musimy uaktualnić offset kontenera w kontrolkach-dzieciach.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void AbsolutePositionChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == this.Owner.NameOf(_ => _.AbsolutePosition))
			{
				foreach (var control in base.Items)
				{
					control.ContainerOffset = this.Owner.AbsolutePosition;
				}
			}
		}

		/// <summary>
		/// Któreś dziecko zmieniło swoją widoczność - poprawiamy layout.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ChildVisibilityChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == (sender as IControl).NameOf(_ => _.Visible))
			{
				this.Owner.Layout();
			}
		}
		#endregion
	}
}
