using System;
using System.Collections.Generic;
using OpenTK.Input;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Kontener GUI.
	/// </summary>
	public class Container
		: IContainer
	{
		#region Private fields
		private UIData CurrentData = new UIData();
		private List<IControl> Controls = new List<IControl>();
		#endregion

		#region IGuiContainer Members
		/// <summary>
		/// Wejście dla GUI.
		/// </summary>
		public Interfaces.IInput Input
		{
			get
			{
				return this.CurrentData.Input;
			}
			set
			{
				this.CurrentData.Input = value;
			}
		}

		/// <summary>
		/// Renderer GUI.
		/// </summary>
		public Interfaces.Graphics.IRenderer Renderer
		{
			get { return this.CurrentData.Renderer; }
			set { this.CurrentData.Renderer = value; }
		}

		/// <summary>
		/// Pobiera kontrolkę o wskazanym Id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Kontrolka lub null, gdy nie znaleziono.</returns>
		public IControl this[string id]
		{
			get { return this.Controls.Find(c => c.Id == id); }
		}

		/// <summary>
		/// Uaktualnia wszystkie kontrolki w kontenerze.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		public void Update(double delta)
		{
			this.CurrentData.Hot = null;
			foreach (var c in this.Controls)
			{
				if (c.ContainsMouse())
				{
					this.CurrentData.Hot = c;
				}
				if (this.CurrentData.Input[MouseButton.Left])
				{
					this.CurrentData.Active = this.CurrentData.Hot;
				}
				else if (this.CurrentData.Active != null && !this.CurrentData.Active.PermanentActive)
				{
					this.CurrentData.Active = null;
				}
				c.Update(delta);
			}
		}

		/// <summary>
		/// Renderuje wszystkie kontrolki.
		/// </summary>
		public void Render()
		{
			foreach (var c in this.Controls)
			{
				c.Render();
			}
		}

		/// <summary>
		/// Sprawdza stan kontrolki za pomocą <see cref="IControl.Check"/>.
		/// </summary>
		/// <param name="id">Identyfikator kontrolki.</param>
		/// <returns>Nr akcji bądź 0, gdy żadna akcja nie zaszła.</returns>
		public int Control(string id)
		{
			var ctrl = this.Controls.Find(c => c.Id == id);
			if (ctrl == null)
			{
				throw new Exceptions.NotFoundException("control");
			}
			return ctrl.Check();
		}

		/// <summary>
		/// Dodaje listę kontrolek do kolekcji.
		/// </summary>
		/// <param name="items">Lista.</param>
		public void AddRange(IEnumerable<IControl> items)
		{
			foreach (var item in items)
			{
				if (this.Contains(item))
				{
					throw new Exceptions.ArgumentAlreadyExistsException("item");
				}
				item.Data = this.CurrentData;
			}
			this.Controls.AddRange(items);
		}

		/// <summary>
		/// Usuwa kontrolkę o wskazanym ID.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Czy udało się usunąć kontrolkę.</returns>
		public bool Remove(string id)
		{
			return this.Controls.RemoveAll(c => c.Id == id) > 0;
		}
		#endregion

		#region ICollection<IControl> Members
		/// <summary>
		/// Dodaje kontrolkę do kolekcji.
		/// Musi mieć unikatowe Id.
		/// </summary>
		/// <param name="item">Kontrolka do dodania.</param>
		/// <exception cref="Exceptions.ArgumentAlreadyExistsException">Rzucane, gdy kontrolka z takim Id już istnieje.</exception>
		/// <exception cref="ArgumentNullException">Rzucane, gdy item jest null, bądź item.Id jest puste.</exception>
		public void Add(IControl item)
		{
			if (this.Contains(item))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("item");
			}
			item.Data = this.CurrentData;
			this.Controls.Add(item);
		}

		/// <summary>
		/// Czyści listę kontrolek.
		/// </summary>
		public void Clear()
		{
			this.Controls.Clear();
		}

		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się kontrolka o id identycznym ze wskazaną.
		/// </summary>
		/// <param name="item">Kontroa do porównania z.</param>
		/// <returns>Czy znaleziono.</returns>
		/// <exception cref="ArgumentNullException">Rzucane, gdy item jest null, bądź item.Id jest puste.</exception>
		public bool Contains(IControl item)
		{
			if (item == null || string.IsNullOrWhiteSpace(item.Id))
			{
				throw new ArgumentNullException("item");
			}
			return this.Controls.Find(c => c.Id == item.Id) != null;
		}

		/// <summary>
		/// Zwraca liczbę kontrolek w kolekcji.
		/// </summary>
		public int Count
		{
			get { return this.Controls.Count; }
		}

		/// <summary>
		/// Niewspierane.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		bool ICollection<IControl>.Remove(IControl item)
		{
			throw new NotSupportedException("Use Remove(string) instead.");
		}

		/// <summary>
		/// Kopiuje kolekcje do tablicy.
		/// </summary>
		/// <param name="array">Tablica.</param>
		/// <param name="arrayIndex">Indeks początkowy.</param>
		void ICollection<IControl>.CopyTo(IControl[] array, int arrayIndex)
		{
			this.Controls.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Czy jest tylko do odczytu - zawsze false.
		/// </summary>
		bool ICollection<IControl>.IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region IEnumerable<IControl> Members
		public IEnumerator<IControl> GetEnumerator()
		{
			return this.Controls.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Controls.GetEnumerator();
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje kontener.
		/// </summary>
		/// <param name="input">Wejście.</param>
		/// <param name="renderer">Renderer.</param>
		public Container(Interfaces.IInput input = null, Interfaces.Graphics.IRenderer renderer = null)
		{
			this.CurrentData.Input = input;
			this.Renderer = renderer;
		}
		#endregion

		#region UIData
		private class UIData
			: IUIData
		{
			#region IUIData Members
			public IControl Hot { get; set; }
			public IControl Active { get; set; }
			public Interfaces.IInput Input { get; set; }
			public Interfaces.Graphics.IRenderer Renderer { get; set; }
			#endregion
		}
		#endregion
	}
}
