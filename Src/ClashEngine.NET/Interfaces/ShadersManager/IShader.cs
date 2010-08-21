using OpenTK;

namespace ClashEngine.NET.Interfaces.ShadersManager
{
	/// <summary>
	/// Typ shadera.
	/// </summary>
	public enum ShaderType
	{
		/// <summary>
		/// Pixel(fragment) shader.
		/// </summary>
		Pixel,

		/// <summary>
		/// Vertex shader.
		/// </summary>
		Vertex
	}

	/// <summary>
	/// Bazowy interfejs dla shaderów.
	/// </summary>
	public interface IShader
		: ResourcesManager.IResource
	{
		/// <summary>
		/// Identyfikator shadera.
		/// </summary>
		int ShaderId { get; }

		/// <summary>
		/// Typ shadera.
		/// </summary>
		ShaderType Type { get; }

		//Wstępnie tylko jeden, później będzie rozbudowane o więcej.
		#region Uniform binding
		/// <summary>
		/// Ustawia parametr.
		/// </summary>
		/// <param name="name">Nazwa.</param>
		/// <param name="matrix">Macierz.</param>
		void Set(string name, Matrix4 matrix);
		#endregion
	}
}
