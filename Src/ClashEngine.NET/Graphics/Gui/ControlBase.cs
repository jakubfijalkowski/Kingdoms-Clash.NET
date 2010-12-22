using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Markup;
using OpenTK;

namespace ClashEngine.NET.Graphics.Gui
{
	using Extensions;
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Bazowa klasa dla kontrolek GUI.
	/// </summary>
	[RuntimeNameProperty("Id")]
	[ContentProperty("Objects")]
	[DebuggerDisplay("{GetType().Name,nq} {Id,nq}")]
	public abstract class ControlBase
		: IControl, INotifyPropertyChanged, ISupportInitialize
	{
		#region Private fields
		private bool _IsActive = false;
		private bool _IsHot = false;
		private bool _Visible = false;
		private IContainer _Owner = null;
		private Vector2 _Offset = Vector2.Zero;
		private Vector2 _Position = Vector2.Zero;
		private Vector2 _AbsolutePosition = Vector2.Zero;
		private object _DataContext = null;
		#endregion

		#region IControl Members
		/// <summary>
		/// Identyfikator.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Właściciel kontrolki.
		/// </summary>
		IContainer IControl.Owner
		{
			get { return this.Owner; }
			set { this.Owner = value; }
		}

		/// <summary>
		/// Właściciel kontrolki.
		/// </summary>
		protected virtual IContainer Owner
		{
			get { return this._Owner; }
			set
			{
				if (this._Owner != null)
				{
					throw new System.InvalidOperationException("Control can be assigned only to one container");
				}
				this._Owner = value;
			}
		}

		/// <summary>
		/// Dane UI.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		IUIData IControl.Data
		{
			get { return this.Data; }
			set { this.Data = value; }
		}

		/// <summary>
		/// Dane UI.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		protected IUIData Data { get; set; }

		/// <summary>
		/// Offset dla kontrolki ustawiany przez kontener.
		/// Nie do zmiany ręcznej.
		/// </summary>
		public OpenTK.Vector2 ContainerOffset
		{
			get { return this._Offset; }
			set
			{
				this._Offset = value;
				this.AbsolutePosition = this.Position + this._Offset;
			}
		}

		/// <summary>
		/// Pozycja.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector2Converter))]
		public OpenTK.Vector2 Position
		{
			get { return this._Position; }
			set
			{
				this._Position = value;
				this.AbsolutePosition = this._Position + this._Offset;
			}
		}

		/// <summary>
		/// Pozycja kontrolki - absoulutna, uwzględnia offset kontenera.
		/// </summary>
		public virtual OpenTK.Vector2 AbsolutePosition
		{
			get { return this._AbsolutePosition; }
			protected set
			{
				this._AbsolutePosition = value;
				(this.Objects as Internals.ObjectsCollection).UpdatePositions();
			}
		}

		/// <summary>
		/// Rozmiar przycisku.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector2Converter))]
		public OpenTK.Vector2 Size { get; set; }

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
		/// Czy kontrolka jest widoczna.
		/// </summary>
		public bool Visible
		{
			get { return this._Visible; }
			set
			{
				if (value != this._Visible)
				{
					this._Visible = value;
					this.SendPropertyChanged("Visible");
				}
			}
		}

		/// <summary>
		/// Lista z obiektami dla renderera.
		/// </summary>
		public IObjectsCollection Objects { get; private set; }

		/// <summary>
		/// Czy kontrolka ma być "permanentnie" aktywna, tzn. czy po puszczeniu przycisku myszy przestaje być aktywna.
		/// </summary>
		public abstract bool PermanentActive { get; }

		/// <summary>
		/// Renderuje wszystkie obiekty.
		/// </summary>
		public void Render()
		{
			foreach (var obj in this.Objects)
			{
				if (obj.Visible)
				{
					this.Data.Renderer.Draw(obj);
				}
			}
		}

		/// <summary>
		/// Sprawdza, czy myszka znajduje się nad kontrolką.
		/// </summary>
		/// <returns></returns>
		public virtual bool ContainsMouse()
		{
			return this.Data.Input.TransformMousePosition(this.Owner.Camera).IsIn(this.AbsolutePosition, this.Size);
		}

		/// <summary>
		/// Wywoływane przy dodaniu do kontenera.
		/// </summary>
		public virtual void OnAdd()
		{ }

		/// <summary>
		/// Wywoływane przy usunięciu z kontenera.
		/// </summary>
		public virtual void OnRemove()
		{ }
		#endregion

		#region IDataContext Members
		/// <summary>
		/// Kontekst danych dla danego
		/// </summary>
		public virtual object DataContext
		{
			get { return this._DataContext; }
			set
			{
				if (value != this._DataContext)
				{
					this._DataContext = value;
					this.SendPropertyChanged("DataContext");
				}
			}
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
			this.Visible = true;
			this.Objects = new Internals.ObjectsCollection(this);
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

		#region ISupportInitialize Members
		public void BeginInit()
		{ }

		/// <summary>
		/// Sprawdza, czy wszystkie właściwości są poprawne.
		/// </summary>
		public void EndInit()
		{
			if (this.Size.X == 0 || this.Size.Y == 0)
			{
				throw new System.InvalidOperationException("Cannot create control with 0 size");
			}
			if (string.IsNullOrWhiteSpace(this.Id))
			{
				throw new System.InvalidOperationException("Cannot create control with empty Id");
			}
		}
		#endregion
	}
}
