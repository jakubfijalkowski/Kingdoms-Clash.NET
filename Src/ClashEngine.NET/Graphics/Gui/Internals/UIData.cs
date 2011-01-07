namespace ClashEngine.NET.Graphics.Gui.Internals
{
	using Interfaces;
	using Interfaces.Graphics;
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Dane UI.
	/// </summary>
	internal class UIData
		: IUIData
	{
		#region IUIData Members
		/// <summary>
		/// Aktualnie "gorąca" kontrolka.
		/// 
		/// Np. użytkownik trzyma nad nią myszkę, ale jeszcze nie kliknął.
		/// </summary>
		public IControl Hot { get; set; }

		/// <summary>
		/// Aktualnie aktywna kontrolka.
		/// 
		/// Aktualnie katywna kontrolka - np. button zaraz przed wciśnięciem.
		/// </summary>
		public IControl Active { get; set; }

		/// <summary>
		/// Wejście dla GUI.
		/// </summary>
		public IInput Input { get; private set; }

		/// <summary>
		/// Renderer.
		/// </summary>
		public IRenderer Renderer { get; private set; }
		#endregion

		#region Constructors
		public UIData(IInput input, IRenderer renderer)
		{
			this.Input = input;
			this.Renderer = renderer;
		}
		#endregion
	}
}
