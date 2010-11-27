using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Markup;	

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;
	using Utilities;

	/// <summary>
	/// Bazowa klasa dla kontrolek zdolnych do serializacji do XAML.
	/// </summary>
	[ContentProperty("Objects")]
	[RuntimeNameProperty("Id")]
	[DebuggerDisplay("{GetType().Name,nq} {Id,nq}")]
	public abstract class ControlBase
		: IControl, INotifyPropertyChanged
	{
		#region Private fields
		private bool _IsActive = false;
		private bool _IsHot = false;
		#endregion
		
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
		[TypeConverter(typeof(Converters.Vector2Converter))]
		public OpenTK.Vector2 Position { get; set; }

		/// <summary>
		/// Rozmiar przycisku.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector2Converter))]
		public OpenTK.Vector2 Size { get; set; }

		/// <summary>
		/// Dane UI.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected IUIData Data { get { return (this as IControl).Data; } }

		/// <summary>
		/// Lista z obiektami dla renderera.
		/// </summary>
		public IObjectsCollection Objects { get; private set; }

		/// <summary>
		/// Czy kontrolka jest aktywna.
		/// </summary>
		/// <remarks>
		/// Uaktualniane w metodzie Update.
		/// </remarks>
		public bool IsActive
		{
			get { return this._IsActive; }
			private set
			{
				if (value != this._IsActive)
				{
					this._IsActive = value;
					this.SendPropertyChanged("IsActive");
				}
			}
		}

		/// <summary>
		/// Czy kontrolka jest "gorąca".
		/// </summary>
		/// <remarks>
		/// Uaktualniane w metodzie Update.
		/// </remarks>
		public bool IsHot
		{
			get { return this._IsHot; }
			private set
			{
				if (value != this._IsHot)
				{
					this._IsHot = value;
					this.SendPropertyChanged("IsHot");
				}
			}
		}

		/// <summary>
		/// Czy kontrolka ma być "permanentnie" aktywna, tzn. czy po puszczeniu przycisku myszy przestaje być aktywna.
		/// </summary>
		public abstract bool PermanentActive { get; }

		/// <summary>
		/// Rysuje wszystkie obiekty kontrolki.
		/// </summary>
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

		#region INotifyPropertyChanged Members
		/// <summary>
		/// Wywoływane przy zmianie IsActive, IsHot.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region To override
		/// <summary>
		/// Aktualizuje kontrolkę.
		/// </summary>
		/// <param name="delta"></param>
		public virtual void Update(double delta)
		{
			this.IsActive = this.Data.Active == this;
			this.IsHot = this.Data.Hot == this;
		}

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

		#region Protected methods
		/// <summary>
		/// Wywyła, jeśli może, zdarzenie PropertyChanged.
		/// </summary>
		/// <param name="propertyName">Nazwa właściwości.</param>
		protected void SendPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion
	}
}
