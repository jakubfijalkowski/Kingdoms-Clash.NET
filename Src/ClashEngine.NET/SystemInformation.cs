using System;
using System.Management;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace ClashEngine.NET
{
	/// <summary>
	/// Architektura procesora.
	/// </summary>
	public enum ProcessorArchitecture
		: ushort
	{
		x86 = 0,
		MIPS,
		Alpha,
		PowerPC,
		Itanium = 6,
		x64 = 9
	}

	/// <summary>
	/// Informacje o systemie - system, procesor, pamięć itp.
	/// Obsługuję tylko jedną kartę graficzną, jeden procesor i jeden tablicę pamięci(RAM).
	/// Singleton.
	/// Do pobierania używa WMI.
	/// </summary>
	public class SystemInformation
	{
		#region Singleton
		private static SystemInformation Instance_;

		/// <summary>
		/// Instancja informacji o systemie.
		/// </summary>
		public static SystemInformation Instance
		{
			get
			{
				if (Instance_ == null)
				{
					Instance_ = new SystemInformation();
				}
				return Instance_;
			}
		}
		#endregion

		#region Properties
		#region OS
		/// <summary>
		/// System operacyjny.
		/// </summary>
		public OperatingSystem System { get; private set; }

		/// <summary>
		/// Czy SO jest 64-bitowy.
		/// </summary>
		public bool Is64BitOS { get; private set; }

		/// <summary>
		/// Czy proces jest 64-bitowy.
		/// </summary>
		public bool Is64BitProcess { get; private set; }
		#endregion

		#region RAM
		/// <summary>
		/// Rozmiar pamięci RAM w kilobajtach.
		/// </summary>
		public uint MemorySize { get; private set; }
		#endregion

		#region Graphics card
		/// <summary>
		/// Nazwa karty graficznej.
		/// </summary>
		public string GraphicsCardName { get; private set; }

		/// <summary>
		/// Rozmiar VRAM w bajtach.
		/// </summary>
		public uint VRAMSize { get; private set; }

		/// <summary>
		/// Wersja zainstalowanych sterowników.
		/// </summary>
		public string DriverVersion { get; private set; }
		#endregion

		#region Processor
		/// <summary>
		/// Nazwa procesora.
		/// </summary>
		public string ProcessorName { get; private set; }

		/// <summary>
		/// Architektura procesora.
		/// </summary>
		public ProcessorArchitecture ProcessorArchitecture { get; private set; }

		/// <summary>
		/// Taktowanie procesora. W MHz.
		/// </summary>
		public uint ProcessorSpeed { get; private set; }

		/// <summary>
		/// Liczba rdzeni.
		/// </summary>
		public uint NumberOfCores { get; private set; }
		#endregion

		#region OpenGL
		private string OpenGLVersion_ = string.Empty;
		private string GLSLVersion_ = string.Empty;
		private string Extensions_ = string.Empty;

		/// <summary>
		/// Wersja OpenGL.
		/// Używać PO zainicjalizowaniu kontekstu OpenGL(po utworzeniu obiektu gry)!
		/// </summary>
		public string OpenGLVersion
		{
			get
			{
				if (string.IsNullOrEmpty(this.OpenGLVersion_))
				{
					this.OpenGLVersion_ = GL.GetString(StringName.Version);
				}
				return this.OpenGLVersion_;
			}
		}

		/// <summary>
		/// Wersja GLSL.
		/// Używać PO zainicjalizowaniu kontekstu OpenGL(po utworzeniu obiektu gry)!
		/// </summary>
		public string GLSLVersion
		{
			get
			{
				if (string.IsNullOrEmpty(this.GLSLVersion_))
				{
					this.GLSLVersion_ = GL.GetString(StringName.Version);
				}
				return this.GLSLVersion_;
			}
		}

		/// <summary>
		/// Dostępne rozszerzenia.
		/// Używać PO zainicjalizowaniu kontekstu OpenGL(po utworzeniu obiektu gry)!
		/// </summary>
		public string Extensions
		{
			get
			{
				if (string.IsNullOrEmpty(this.Extensions_))
				{
					this.Extensions_ = GL.GetString(StringName.Version);
				}
				return this.Extensions_;
			}
		}

		#endregion
		#endregion

		private SystemInformation()
		{
			this.System = Environment.OSVersion;
			this.Is64BitOS = Environment.Is64BitOperatingSystem;
			this.Is64BitProcess = Environment.Is64BitProcess;

			foreach (var item in this.Get("Win32_PhysicalMemoryArray"))
			{
				this.MemorySize = (uint)item["MaxCapacity"];
				break;
			}

			foreach (var item in this.Get("Win32_VideoController"))
			{
				this.GraphicsCardName = (string)item["Caption"];
				this.VRAMSize = (uint)item["AdapterRAM"];
				this.DriverVersion = (string)item["DriverVersion"];
				break;
			}

			foreach (var item in this.Get("Win32_Processor"))
			{
				this.ProcessorName = (string)item["Name"];
				this.ProcessorArchitecture = (ProcessorArchitecture)(ushort)item["Architecture"];
				this.ProcessorSpeed = (uint)item["CurrentClockSpeed"];
				this.NumberOfCores = (uint)item["NumberOfCores"];
				break;
			}

			//this.OpenGLVersion = GL.GetString(StringName.Version);
			//this.GLSLVersion = GL.GetString(StringName.ShadingLanguageVersion);
			//this.Extensions = GL.GetString(StringName.Extensions);
		}

		/// <summary>
		/// Pobiera elementy z WMI.
		/// </summary>
		/// <param name="class">Klasa obiektu.</param>
		/// <returns>Lista obiektów.</returns>
		private ManagementObjectCollection Get(string @class, string condition = "")
		{
			SelectQuery select = new SelectQuery(@class, condition);
			return new ManagementObjectSearcher(select).Get();
		}
	}
}
