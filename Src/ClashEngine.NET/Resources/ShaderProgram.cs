using System;
using OpenTK.Graphics.OpenGL;

namespace ClashEngine.NET.Resources
{
	using Interfaces.Resources;
	using Interfaces.ResourcesManager;
	using System.IO;

	/// <summary>
	/// Shader program w GLSL.
	/// Przy inicjalizowaniu automatycznie przypisuje FragmentShaderFile i VertexShaderFile na wartość {Id}.frag i {Id}.vert.
	/// </summary>
	public class ShaderProgram
		: ResourcesManager.Resource, IShaderProgram
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");
		
		#region Properties
		/// <summary>
		/// Identyfikator fragment shadera.
		/// </summary>
		public int FragmentShaderId { get; private set; }

		/// <summary>
		/// Identyfikator vertex shadera.
		/// </summary>
		public int VertexShaderId { get; private set; }
		#endregion
		
		#region IShaderProgram members
		#region Properties
		/// <summary>
		/// Identyfikator shader programu.
		/// </summary>
		public int ShaderProgramId { get; private set; }

		/// <summary>
		/// Plik źródłowy z kodem fragment shadera.
		/// </summary>
		public string FragmentShaderFile { get; private set; }

		/// <summary>
		/// Plik źródłowy z kodem vertex shadera.
		/// </summary>
		public string VertexShaderFile { get; private set; }
		#endregion

		#region Methods
		/// <summary>
		/// Binduje shader program.
		/// </summary>
		public void Bind()
		{
			GL.UseProgram(this.ShaderProgramId);
		}
		#endregion
		#endregion

		#region Resource members
		public override void Init(string id)
		{
			base.Init(id);
			this.FragmentShaderFile = id + ".frag";
			this.VertexShaderFile = id + ".vert";
		}

		/// <summary>
		/// Ładuje zasób.
		/// </summary>
		/// <returns>Stan załadowania zasobu.</returns>
		public override ResourceLoadingState Load()
		{
			string fragmentSource = string.Empty;
			string vertexSource = string.Empty;

			#region Loading source
			try
			{
				using (FileStream fragFs = new FileStream(this.FragmentShaderFile, FileMode.Open, FileAccess.Read),
					vertFs = new FileStream(this.VertexShaderFile, FileMode.Open, FileAccess.Read))
				{
					using (StreamReader fragReader = new StreamReader(fragFs),
						vertReader = new StreamReader(vertFs))
					{
						fragmentSource = fragReader.ReadToEnd();
						vertexSource = vertReader.ReadToEnd();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.ErrorException("Cannot load shader sources.", ex);
				return ResourceLoadingState.Failure;
			}
			Logger.Trace("Shader source loaded");
			#endregion

			#region Creating shaders
			this.FragmentShaderId = GL.CreateShader(ShaderType.FragmentShader);
			this.VertexShaderId = GL.CreateShader(ShaderType.VertexShader);
			if (Error("Cannot create shaders. Error: {0}"))
			{
				return ResourceLoadingState.Failure;
			}

			GL.ShaderSource(this.FragmentShaderId, fragmentSource);
			GL.CompileShader(this.FragmentShaderId);
			if (Error("Cannot compile fragment shader. Error: {0}"))
			{
				Logger.Error("Compilation log: {0}", GL.GetShaderInfoLog(this.FragmentShaderId));
				return ResourceLoadingState.Failure;
			}
			Logger.Trace("Fragment shader compiled succesfully");

			GL.ShaderSource(this.VertexShaderId, vertexSource);
			GL.CompileShader(this.VertexShaderId);
			if (Error("Cannot compile vertex shader. Error: {0}"))
			{
				Logger.Error("Compilation log: {0}", GL.GetShaderInfoLog(this.VertexShaderId));
				return ResourceLoadingState.Failure;
			}
			Logger.Trace("Vertex shader compiled succesfully");
			#endregion

			#region Creating program
			this.ShaderProgramId = GL.CreateProgram();
			GL.AttachShader(this.ShaderProgramId, this.VertexShaderId);
			GL.AttachShader(this.ShaderProgramId, this.FragmentShaderId);
			GL.LinkProgram(this.ShaderProgramId);
			if (Error("Cannot create shader program. Error: {0}"))
			{
				return ResourceLoadingState.Failure;
			}
			#endregion

			Logger.Trace("Shader program created succesfully");
			return ResourceLoadingState.Success;
		}

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		public override void Free()
		{
			GL.DeleteProgram(this.ShaderProgramId);
			GL.DeleteShader(this.VertexShaderId);
			GL.DeleteShader(this.FragmentShaderId);
		}
		#endregion

		#region Private utilities
		private static bool Error(string msg)
		{
			ErrorCode err = GL.GetError();
			if (err != ErrorCode.NoError)
			{
				Logger.Error(msg, err.ToString());
				return true;
			}
			return false;
		}
		#endregion
	}
}
