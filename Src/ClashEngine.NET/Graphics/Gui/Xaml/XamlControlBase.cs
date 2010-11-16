namespace ClashEngine.NET.Graphics.Gui.Xaml
{
	using Interfaces.Graphics.Gui;
	using Interfaces.Graphics.Gui.Xaml;
	
	/// <summary>
	/// Bazowa klasa dla kontrolek zdolnych do serializacji do XAML.
	/// </summary>
	[System.Windows.Markup.ContentProperty("Objects")]
	public abstract class XamlControlBase
		: IXamlControl
	{
		#region IXamlControl Members
		/// <summary>
		/// Pozycja.
		/// </summary>
		public OpenTK.Vector2 Position { get; set; }
		#endregion

		#region IControl Members
		/// <summary>
		/// Identyfikator.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Dane UI.
		/// </summary>
		IUIData IControl.Data { get; set; }

		/// <summary>
		/// Dane UI.
		/// </summary>
		protected IUIData Data { get { return (this as IControl).Data; } }

		/// <summary>
		/// Lista z obiektami dla renderera.
		/// </summary>
		IObjectsCollection IControl.Objects { get { return this.Objects; } }

		/// <summary>
		/// Lista z obiektami dla renderera.
		/// </summary>
		public ObjectsCollection Objects { get; private set; }

		/// <summary>
		/// Czy kontrolka ma być "permanentnie" aktywna, tzn. czy po puszczeniu przycisku myszy przestaje być aktywna.
		/// </summary>
		public abstract bool PermanentActive { get; }

		public void Render()
		{
			foreach (var obj in this.Objects)
			{
				this.Data.Renderer.Draw(obj);
			}
		}
		#endregion

		#region To overrite
		/// <summary>
		/// Sprawdza, czy myszka znajduje się nad kontrolką.
		/// </summary>
		/// <returns></returns>
		public abstract bool ContainsMouse();

		/// <summary>
		/// Aktualizuje kontrolkę.
		/// </summary>
		/// <param name="delta"></param>
		public abstract void Update(double delta);

		/// <summary>
		/// Sprawdza stan kontrolki.
		/// </summary>
		/// <returns></returns>
		public abstract int Check();
		#endregion

		#region Constructors
		public XamlControlBase()
		{
			this.Objects = new ObjectsCollection();
		}
		#endregion
	}
}
