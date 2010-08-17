using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClashEngine.NET.Interfaces
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
	/// Bazowy interfejs dla informacji o systemie.
	/// </summary>
	public interface ISystemInformation
	{
		#region OS
		/// <summary>
		/// System operacyjny.
		/// </summary>
		OperatingSystem System { get; }

		/// <summary>
		/// Czy SO jest 64-bitowy.
		/// </summary>
		bool Is64BitOS { get; }

		/// <summary>
		/// Czy proces jest 64-bitowy.
		/// </summary>
		bool Is64BitProcess { get; }

		/// <summary>
		/// Wesja CLR.
		/// </summary>
		Version CLRVersion { get; }
		#endregion

		#region RAM
		/// <summary>
		/// Rozmiar pamięci RAM w kilobajtach.
		/// </summary>
		uint MemorySize { get; }
		#endregion

		#region Graphics card
		/// <summary>
		/// Nazwa karty graficznej.
		/// </summary>
		string GraphicsCardName { get; }

		/// <summary>
		/// Rozmiar VRAM w bajtach.
		/// </summary>
		uint VRAMSize { get; }

		/// <summary>
		/// Wersja zainstalowanych sterowników karty graficznej.
		/// </summary>
		string GraphicsDriverVersion { get; }
		#endregion

		#region Processor
		/// <summary>
		/// Nazwa procesora.
		/// </summary>
		string ProcessorName { get; }

		/// <summary>
		/// Architektura procesora.
		/// </summary>
		ProcessorArchitecture ProcessorArchitecture { get; }

		/// <summary>
		/// Taktowanie procesora. W MHz.
		/// </summary>
		uint ProcessorSpeed { get; }

		/// <summary>
		/// Liczba rdzeni.
		/// </summary>
		uint NumberOfCores { get; }
		#endregion

		#region OpenGL
		/// <summary>
		/// Wersja OpenGL.
		/// Używać PO zainicjalizowaniu kontekstu OpenGL(po utworzeniu obiektu gry)!
		/// </summary>
		string OpenGLVersion { get; }

		/// <summary>
		/// Wersja GLSL.
		/// Używać PO zainicjalizowaniu kontekstu OpenGL(po utworzeniu obiektu gry)!
		/// </summary>
		string GLSLVersion { get; }

		/// <summary>
		/// Dostępne rozszerzenia.
		/// Używać PO zainicjalizowaniu kontekstu OpenGL(po utworzeniu obiektu gry)!
		/// </summary>
		string Extensions { get; }
		#endregion
	}
}
