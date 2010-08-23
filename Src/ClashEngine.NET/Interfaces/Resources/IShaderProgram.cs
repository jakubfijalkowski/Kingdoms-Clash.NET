namespace ClashEngine.NET.Interfaces.Resources
{
	/// <summary>
	/// Bazowa klasa dla shaderów. 
	/// </summary>
	public interface IShaderProgram
		: ResourcesManager.IResource
	{
		#region Properties
		/// <summary>
		/// Identyfikator shader programu.
		/// </summary>
		int ShaderProgramId { get; }

		/// <summary>
		/// Plik źródłowy z kodem fragment shadera.
		/// </summary>
		string FragmentShaderFile { get; }

		/// <summary>
		/// Plik źródłowy z kodem vertex shadera.
		/// </summary>
		string VertexShaderFile { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Binduje shader program.
		/// </summary>
		void Bind();
		#endregion
	}
}
