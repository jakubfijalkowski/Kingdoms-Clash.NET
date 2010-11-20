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
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private UIData CurrentData = new UIData();
		#endregion

		#region IContainer Members
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
		/// Kolekcja kontrolek.
		/// </summary>
		public IControlsCollection Controls { get; private set; }

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
			var oldMode = this.Renderer.SortMode;
			this.Renderer.SortMode = Interfaces.Graphics.SortMode.FrontToBack;
			foreach (var c in this.Controls)
			{
				c.Render();
			}
			this.Renderer.SortMode = oldMode;
		}

		/// <summary>
		/// Sprawdza stan kontrolki za pomocą <see cref="IControl.Check"/>.
		/// </summary>
		/// <param name="id">Identyfikator kontrolki.</param>
		/// <returns>Nr akcji bądź 0, gdy żadna akcja nie zaszła.</returns>
		public int Control(string id)
		{
			var ctrl = this.Controls[id];
			if (ctrl == null)
			{
				throw new Exceptions.NotFoundException("control");
			}
			return ctrl.Check();
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
			this.Controls = new ControlsCollection(this.CurrentData);
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
