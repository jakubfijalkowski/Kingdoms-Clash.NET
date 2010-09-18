using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ClashEngine.NET.Utilities
{
	/// <summary>
	/// Wierzchołek.
	/// Zawiera dwuwymiarową pozycję.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vertex2DP
	{
		public Vector2 Position;
	}

	/// <summary>
	/// Wierzchołek.
	/// Zawiera dwuwymiarową pozycję i kolor w formacie BGRA(4xfloat).
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vertex2DPC
	{
		public Vector2 Position;
		public Vector4 Color;
	}

	/// <summary>
	/// Wierzchołek.
	/// Zawiera dwuwymiarową pozycję i koordynaty tekstur.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vertex2DPTC
	{
		public Vector2 Position;
		public Vector2 TexCoord;
	}

	/// <summary>
	/// Podwójny Vertex Buffer Object.
	/// Przechowuje w sobie dwa VBO - jeden na dane wierzchołków drugi na indeksy, ponieważ i tak przeważnie używa się ich w parach.
	/// </summary>
	public class VBO
		: Interfaces.Utilities.IVBO
	{
		/// <summary>
		/// Indeksy VBO.
		/// 0 - wierzchołki.
		/// 1 - indeksy.
		/// </summary>
		private int[] VBOIds = new int[2];

		#region Properties
		/// <summary>
		/// Identyfikator VBO wierzchołków.
		/// </summary>
		public int VerticesId { get { return this.VBOIds[0]; } }

		/// <summary>
		/// Identyfikator VBO indeksów.
		/// </summary>
		public int IndeciesId { get { return this.VBOIds[1]; } }

		/// <summary>
		/// Liczba wierzchołków.
		/// </summary>
		public int VerticesCount { get; private set; }

		/// <summary>
		/// Liczba indeksów.
		/// </summary>
		public int IndeciesCount { get; private set; }
		#endregion

		#region Contructors
		/// <summary>
		/// Inicjalizuje VBO.
		/// </summary>
		/// <param name="indecies">Indeksy.</param>
		/// <param name="vertices">Wierzchołki</param>
		public VBO(uint[] indecies, Vertex2DP[] vertices)
		{
			if (indecies == null || indecies.Length == 0)
			{
				throw new ArgumentNullException("indecies");
			}
			else if (vertices == null || vertices.Length == 0)
			{
				throw new ArgumentNullException("vertices");
			}

			this.IndeciesCount = indecies.Length;
			this.VerticesCount = vertices.Length;

			GL.GenBuffers(2, this.VBOIds);

			this.Bind();
			this.SetLayoutFloats(2);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.VerticesCount * 2 * sizeof(float)), vertices, BufferUsageHint.StaticDraw);
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(this.IndeciesCount * sizeof(uint)), indecies, BufferUsageHint.StaticDraw);
		}

		/// <summary>
		/// Inicjalizuje VBO.
		/// </summary>
		/// <param name="indecies">Indeksy.</param>
		/// <param name="vertices">Wierzchołki</param>
		public VBO(uint[] indecies, Vertex2DPC[] vertices)
		{
			if (indecies == null || indecies.Length == 0)
			{
				throw new ArgumentNullException("indecies");
			}
			else if (vertices == null || vertices.Length == 0)
			{
				throw new ArgumentNullException("vertices");
			}

			this.IndeciesCount = indecies.Length;
			this.VerticesCount = vertices.Length;

			GL.GenBuffers(2, this.VBOIds);

			this.Bind();
			this.SetLayoutFloats(2, 4);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.VerticesCount * 6 * sizeof(float)), vertices, BufferUsageHint.StaticDraw);
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(this.IndeciesCount * sizeof(uint)), indecies, BufferUsageHint.StaticDraw);
		}

		/// <summary>
		/// Inicjalizuje VBO.
		/// </summary>
		/// <param name="indecies">Indeksy.</param>
		/// <param name="vertices">Wierzchołki</param>
		public VBO(uint[] indecies, Vertex2DPTC[] vertices)
		{
			if (indecies == null || indecies.Length == 0)
			{
				throw new ArgumentNullException("indecies");
			}
			else if (vertices == null || vertices.Length == 0)
			{
				throw new ArgumentNullException("vertices");
			}

			this.IndeciesCount = indecies.Length;
			this.VerticesCount = vertices.Length;

			GL.GenBuffers(2, this.VBOIds);

			this.Bind();
			this.SetLayoutFloats(2, 0, 2);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.VerticesCount * 6 * sizeof(float)), vertices, BufferUsageHint.StaticDraw);
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(this.IndeciesCount * sizeof(uint)), indecies, BufferUsageHint.StaticDraw);
		}
		#endregion

		/// <summary>
		/// Binduje VBO.
		/// </summary>
		public void Bind()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, this.VerticesId);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.IndeciesId);
		}

		/// <summary>
		/// Rysuje VBO.
		/// </summary>
		public void Draw()
		{
			this.Bind();
			GL.DrawElements(BeginMode.Triangles, this.IndeciesCount, DrawElementsType.UnsignedInt, 0);
		}

		#region Internals
		/// <summary>
		/// Ustawia layout wierzchołka na wskazany.
		/// Float jest przyjmowany jako typ wszystkich składowych.
		/// </summary>
		/// <param name="v">Liczba komponentów pozycji.</param>
		/// <param name="c">Liczba komponentów koloru.</param>
		/// <param name="tc">Liczba komponentów koordynatów tekstur.</param>
		private void SetLayoutFloats(int v, int c = 0, int tc = 0)
		{
			int size = sizeof(float) * (v + c + tc);
			if (v > 0)
			{
				GL.VertexPointer(v, VertexPointerType.Float, size, 0);
			}
			if (c > 0)
			{
				GL.ColorPointer(c, ColorPointerType.Float, size, sizeof(float) * v);
			}
			if (tc > 0)
			{
				GL.TexCoordPointer(c, TexCoordPointerType.Float, size, sizeof(float) * (v + c));
			}
		}
		#endregion
	}
}
