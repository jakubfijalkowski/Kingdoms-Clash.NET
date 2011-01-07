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
	[DebuggerDisplay("{GetType().Name,nq} {Id,nq}")]
	public abstract class ControlBase
		: Data.DataContextBase, IControl, ISupportInitialize
	{
		#region Private fields
		private bool _IsActive = false;
		private bool _IsHot = false;
		private bool _Visible = false;
		private IContainerControl _Owner = null;
		private Vector2 _Offset = Vector2.Zero;
		private Vector2 _Position = Vector2.Zero;
		private Vector2 _AbsolutePosition = Vector2.Zero;
		#endregion

		#region IControl Members
		/// <summary>
		/// Identyfikator.
		/// </summary>
		public string Id { get; set; }

		#region Owner
		/// <summary>
		/// Właściciel kontrolki.
		/// </summary>
		IContainerControl IControl.Owner
		{
			get { return this.Owner; }
			set { this.Owner = value; }
		}

		/// <summary>
		/// Właściciel kontrolki.
		/// </summary>
		protected virtual IContainerControl Owner
		{
			get { return this._Owner; }
			set
			{
				if (this._Owner != null)
				{
					throw new System.InvalidOperationException("Control can be assigned only to one container");
				}
				this._Owner = value;
				base.RaisePropertyChanged(() => Owner);
			}
		}
		#endregion

		#region Data
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
		#endregion

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
				base.RaisePropertyChanged(() => AbsolutePosition);
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
					this.RaisePropertyChanged(() => IsActive);
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
					this.RaisePropertyChanged(() => IsHot);
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
					this.RaisePropertyChanged(() => Visible);
				}
			}
		}

		/// <summary>
		/// Sprawdza, czy myszka znajduje się nad kontrolką.
		/// </summary>
		/// <returns></returns>
		public virtual bool ContainsMouse()
		{
			return this.Data.Input.Transform(this.Data.Renderer.Camera).IsIn(this.AbsolutePosition, this.Size);
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

		#region To override
		/// <summary>
		/// Czy kontrolka ma być "permanentnie" aktywna, tzn. czy po puszczeniu przycisku myszy przestaje być aktywna.
		/// </summary>
		public abstract bool PermanentActive { get; }

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
		/// Renderuje wszystkie obiekty.
		/// </summary>
		public abstract void Render();

		/// <summary>
		/// Sprawdza stan kontrolki.
		/// </summary>
		/// <returns></returns>
		public abstract int Check();
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje kontrolkę.
		/// </summary>
		public ControlBase()
		{
			this.Visible = true;
		}
		#endregion
	}
}
