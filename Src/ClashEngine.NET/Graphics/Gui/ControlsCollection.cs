using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Kontener na kontrolki.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	public class ControlsCollection
		: KeyedCollection<string, IControl>, IControlsCollection
	{
		#region Private fields
		private IUIData UIData = null;
		private IContainer Owner = null;
		#endregion

		#region IControlsCollection Members
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
				item.Owner = this.Owner;
				item.Data = this.UIData;
				base.Add(item);
			}
		}

		/// <summary>
		/// Dodaje kontrolkę, która jest w kontrolce niżej.
		/// </summary>
		/// <param name="control"></param>
		public void AddChildControl(IControl control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("item");
			}
			if (this.Contains(control.Id))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("item");
			}
			base.InsertItem(this.Count, control);
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
			item.Data = this.UIData;
			base.InsertItem(index, item);
		}
		#endregion

		#region Constructors
		public ControlsCollection(IContainer owner = null, IUIData uiData = null)
		{
			this.UIData = uiData;
			this.Owner = owner;
		}
		#endregion
	}
}
