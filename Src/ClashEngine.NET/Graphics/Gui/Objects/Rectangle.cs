using System.ComponentModel;
using System.Diagnostics;
using OpenTK;

namespace ClashEngine.NET.Graphics.Gui.Objects
{
	using Converters;
	using Graphics.Objects;
	using Interfaces.Graphics.Gui.Objects;

	/// <summary>
	/// Prostokąt.
	/// </summary>
	[DebuggerDisplay("Rectangle")]	
	public class Rectangle
		: Quad, IRectangle
	{
		#region Private fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool WasPositionSet = false;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool WasSizeSet = false;
		#endregion
	
		#region Quad Members
		//Nadpisujemy, by dodać konwertery typów.

		/// <summary>
		/// Pozycja.
		/// </summary>
		[TypeConverter(typeof(Vector2Converter))]
		public new Vector2 Position
		{
			get { return base.Position; }
			set
			{
				base.Position = value;
				this.WasPositionSet = true;
			}
		}

		/// <summary>
		/// Rozmiar
		/// </summary>
		[TypeConverter(typeof(Vector2Converter))]
		public new Vector2 Size
		{
			get { return base.Size; }
			set
			{
				base.Size = value;
				this.WasSizeSet = true;
			}
		}

		/// <summary>
		/// Kolor prostokąta.
		/// </summary>
		[TypeConverter(typeof(Vector4Converter))]
		public new Vector4 Color
		{
			get { return base.Color; }
			set { base.Color = value; }
		}

		/// <summary>
		/// Głębokość, na której prostokąt się znajduje.
		/// </summary>
		public new float Depth
		{
			get { return base.Depth; }
			set { base.Depth = value; }
		}
		#endregion

		#region IObject Members
		/// <summary>
		/// Kontrolka-rodzic.
		/// </summary>
		public Interfaces.Graphics.Gui.IControl ParentControl { get; set; }

		/// <summary>
		/// Zmieniamy rozmiar i pozycję, jeśli nie zostały zmienione wcześniej.
		/// </summary>
		public void Finish()
		{
			if (!this.WasPositionSet)
			{
				this.Position = this.ParentControl.Position;
			}
			if (!this.WasSizeSet)
			{
				this.Size = this.ParentControl.Size;
			}
		}
		#endregion

		#region Constructor
		public Rectangle()
			: base(Vector2.Zero, Vector2.Zero, System.Drawing.Color.White)
		{ }
		#endregion
	}
}
