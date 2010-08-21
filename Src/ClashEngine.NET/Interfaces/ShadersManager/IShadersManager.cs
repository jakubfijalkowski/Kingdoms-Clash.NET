namespace ClashEngine.NET.Interfaces.ShadersManager
{
	/// <summary>
	/// Bazowy interfejs managera shaderów.
	/// Wskazane jest, by klasa która dziedziczy z niego do ładowania ich używała istniejącego managera zasobów.
	/// </summary>
	public interface IShadersManager
	{
		#region Loading/freeing
		/// <summary>
		/// Ładuje shader lub pobiera już załadowany.
		/// </summary>
		/// <param name="filename">Nazwa pliku.</param>
		/// <returns>Shader.</returns>
		IShader Load(string filename);

		/// <summary>
		/// Zwalnia nieużywany shader.
		/// </summary>
		/// <param name="filename">Nazwa pliku.</param>
		void Free(string filename);

		/// <summary>
		/// Zwalnia nieużywany shader.
		/// </summary>
		/// <param name="shader">Shader.</param>
		void Free(IShader shader);
		#endregion

		/// <summary>
		/// Binduje shader jako aktualny.
		/// </summary>
		/// <param name="shader">Shader.</param>
		void Bind(IShader shader);
	}
}
