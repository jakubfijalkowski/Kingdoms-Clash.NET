using System.Diagnostics;
	
namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;
	using Utilities;

	/// <summary>
	/// Bazowa klasa dla kontrolek zdolnych do serializacji do XAML.
	/// </summary>
	[System.Windows.Markup.ContentProperty("Objects")]
	[System.Windows.Markup.RuntimeNameProperty("Id")]
	[DebuggerDisplay("{GetType().Name,nq} {Id,nq}")]
	public abstract class ControlBase
		: IControl
	{
		#region IControl Members
		/// <summary>
		/// Identyfikator.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Dane UI.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		IUIData IControl.Data { get; set; }

		/// <summary>
		/// Pozycja.
		/// </summary>
		[System.ComponentModel.TypeConverter(typeof(Converters.Vector2Converter))]
		public OpenTK.Vector2 Position { get; set; }

		/// <summary>
		/// Rozmiar przycisku.
		/// </summary>
		[System.ComponentModel.TypeConverter(typeof(Converters.Vector2Converter))]
		public OpenTK.Vector2 Size { get; set; }

		/// <summary>
		/// Dane UI.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected IUIData Data { get { return (this as IControl).Data; } }

		/// <summary>
		/// Lista z obiektami dla renderera.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
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

		/// <summary>
		/// Sprawdza, czy myszka znajduje się nad kontrolką.
		/// </summary>
		/// <returns></returns>
		public virtual bool ContainsMouse()
		{
			return this.Data.Input.MousePosition.IsIn(this.Position, this.Size);
		}
		#endregion

		#region To overrite
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
		public ControlBase()
		{
			this.Objects = new ObjectsCollection(this);
		}
		#endregion
	}
}
