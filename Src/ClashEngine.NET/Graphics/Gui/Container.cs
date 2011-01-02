using OpenTK.Input;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces;
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Kontener GUI.
	/// </summary>
	public class Container
		: IContainer
	{
		#region Private fields
		private UIData CurrentData = new UIData();
		private IGameInfo _GameInfo = null;
		#endregion

		#region IContainer Members
		/// <summary>
		/// Informacje o grze.
		/// </summary>
		public IGameInfo GameInfo
		{
			get { return this._GameInfo; }
			set
			{
				if (value == null)
				{
					throw new System.ArgumentNullException("value");
				}
				this._GameInfo = value;
				this.CurrentData.Input = this.GameInfo.MainWindow.Input;
				this.CurrentData.Renderer = this.GameInfo.Renderer;
			}
		}

		/// <summary>
		/// Kamera używana przez kontener.
		/// Może być null.
		/// </summary>
		public Interfaces.Graphics.ICamera Camera { get; set; }

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
				if (c.Visible)
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
		}

		/// <summary>
		/// Renderuje wszystkie kontrolki.
		/// </summary>
		public void Render()
		{
			var oldMode = this.GameInfo.Renderer.SortMode;
			this.GameInfo.Renderer.SortMode = Interfaces.Graphics.SortMode.FrontToBack;
			this.GameInfo.Renderer.Camera = this.Camera;
			foreach (var c in this.Controls)
			{
				if (c.Visible)
				{
					c.Render();
				}
			}
			this.GameInfo.Renderer.SortMode = oldMode;
		}

		/// <summary>
		/// Sprawdza stan kontrolki za pomocą <see cref="IControl.Check"/>.
		/// </summary>
		/// <param name="id">Identyfikator kontrolki.</param>
		/// <returns>Nr akcji bądź 0, gdy żadna akcja nie zaszła.</returns>
		public int Control(string id)
		{
			return this.Controls[id].Check();
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje kontener.
		/// </summary>
		/// <param name="input">Wejście.</param>
		/// <param name="renderer">Renderer.</param>
		public Container(Interfaces.IGameInfo gameInfo = null)
		{
			this.Controls = new Internals.ControlsCollection(this, this.CurrentData);
			this.GameInfo = gameInfo;
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
