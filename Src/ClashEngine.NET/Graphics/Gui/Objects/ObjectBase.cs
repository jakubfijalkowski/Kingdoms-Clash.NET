using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ClashEngine.NET.Graphics.Gui.Objects
{
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Wewnętrzna klasa bazowa dla obiektów GUI.
	/// </summary>
	public abstract class ObjectBase
		: IObject
	{
		#region Private fields
		private OpenTK.Vector2 _Position = OpenTK.Vector2.Zero;
		#endregion

		#region IObject Members
		/// <summary>
		/// Pozycja relatywna - nie uwzględnia pozycji kontrolki.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector2Converter))]
		public OpenTK.Vector2 Position
		{
			get { return this._Position; }
			set
			{
				this._Position = value;
				if (this.Owner != null)
				{
					this.AbsolutePosition = this._Position + this.Owner.AbsolutePosition;
				}
			}
		}

		/// <summary>
		/// Pozycja absolutna - uwzględnia pozycję(absolutną!) kontrolki(<see cref="Owner"/>).
		/// </summary>
		public abstract OpenTK.Vector2 AbsolutePosition { get; protected set; }

		/// <summary>
		/// Kontrolka-rodzic.
		/// </summary>
		public IStylizableControl Owner { get; set; }

		/// <summary>
		/// Czy obiekt jest widoczny.
		/// </summary>
		public bool Visible { get; set; }

		/// <summary>
		/// Poprawia rozmiar elementu tak, by pasował do kontrolki.
		/// </summary>
		public abstract void Finish();
		#endregion

		#region IObject Members
		public abstract Interfaces.Graphics.Resources.ITexture Texture { get; set; }
		public abstract float Depth { get; set; }
		public abstract Interfaces.Graphics.Vertex[] Vertices { get; }
		public abstract int[] Indecies { get; }
		#endregion
	}
}
