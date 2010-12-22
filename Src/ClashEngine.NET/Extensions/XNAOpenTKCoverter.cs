using TK = OpenTK;
using XNA = Microsoft.Xna.Framework;

namespace ClashEngine.NET.Extensions
{
	/// <summary>
	/// Konwersje z typów OpenTK do XNA(typu używane w Farseer Physics Engine 3) i na odwrót.
	/// </summary>
	public static class XNAOpenTKCoverter
	{
		#region XNA To OpenTK
		public static TK.Vector2 ToOpenTK(this XNA.Vector2 vec)
		{
			return new TK.Vector2(vec.X, vec.Y);
		}

		public static TK.Vector3 ToOpenTK(this XNA.Vector3 vec)
		{
			return new TK.Vector3(vec.X, vec.Y, vec.Z);
		}

		public static TK.Matrix4 ToOpenTK(this XNA.Matrix mat)
		{
			return new TK.Matrix4(
				mat.M11, mat.M12, mat.M13, mat.M14,
				mat.M21, mat.M22, mat.M23, mat.M24,
				mat.M31, mat.M32, mat.M33, mat.M34,
				mat.M41, mat.M42, mat.M43, mat.M44);
		}
		#endregion

		#region OpenTK To XNA
		public static XNA.Vector2 ToXNA(this TK.Vector2 vec)
		{
			return new XNA.Vector2(vec.X, vec.Y);
		}

		public static XNA.Vector3 ToXNA(this TK.Vector3 vec)
		{
			return new XNA.Vector3(vec.X, vec.Y, vec.Z);
		}

		public static XNA.Matrix ToXNA(this TK.Matrix4 mat)
		{
			return new XNA.Matrix(
				mat.M11, mat.M12, mat.M13, mat.M14,
				mat.M21, mat.M22, mat.M23, mat.M24,
				mat.M31, mat.M32, mat.M33, mat.M34,
				mat.M41, mat.M42, mat.M43, mat.M44);
		}
		#endregion
	}
}
