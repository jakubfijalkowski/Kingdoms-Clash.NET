using System.ComponentModel;
using OpenTK;

namespace ClashEngine.NET.Graphics.Gui.Objects
{
	using Converters;
	using Graphics.Objects;
	using Interfaces.Graphics.Gui.Objects;

	/// <summary>
	/// Prostokąt.
	/// </summary>
	public class Rectangle
		: Quad, IRectangle
	{
		//Nadpisujemy, by dodać konwertery typów.

		/// <summary>
		/// Pozycja.
		/// </summary>
		[TypeConverter(typeof(Vector2Converter))]
		public new Vector2 Position
		{
			get { return base.Position; }
			set { base.Position = value; }
		}

		/// <summary>
		/// Rozmiar
		/// </summary>
		[TypeConverter(typeof(Vector2Converter))]
		public new Vector2 Size
		{
			get { return base.Size; }
			set { base.Size = value; }
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

		public Rectangle()
			: base(Vector2.Zero, Vector2.Zero, System.Drawing.Color.White)
		{ }
	}
}
