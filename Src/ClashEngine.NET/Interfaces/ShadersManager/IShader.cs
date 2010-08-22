using System;
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
		IntPtr ShaderId { get; }

		/// <summary>
		/// Typ shadera.
		/// </summary>
		ShaderType Type { get; }

		/// <summary>
		/// Używana wersja shaderów.
		/// Akceptowalne wartości: 0(najnowszy), 1, 2, 3, 4.
		/// </summary>
		int ShaderVersion { get; set; }

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
