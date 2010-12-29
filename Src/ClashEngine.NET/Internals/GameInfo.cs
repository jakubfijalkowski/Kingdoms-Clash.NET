using System;

namespace ClashEngine.NET.Internals
{
	using Interfaces;
	using Interfaces.Graphics;

	/// <summary>
	/// Informacje o grze.
	/// </summary>
	internal class GameInfo
		: IGameInfo
	{
		#region Private fields
		private IWindow _Window = null;
		#endregion

		#region IGameInfo Members
		/// <summary>
		/// Okno gry.
		/// </summary>
		public IWindow MainWindow
		{
			get { return this._Window; }
			internal set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._Window = value;
			}
		}

		/// <summary>
		/// Manager ekranów dla gry.
		/// </summary>
		public IScreensManager Screens { get; internal set; }

		/// <summary>
		/// Manager zasobów.
		/// </summary>
		public IResourcesManager Content { get; internal set; }

		/// <summary>
		/// Renderer.
		/// </summary>
		public IRenderer Renderer { get; internal set; }
		#endregion
	}
}
