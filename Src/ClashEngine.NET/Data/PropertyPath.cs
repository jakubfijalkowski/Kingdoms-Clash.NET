using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClashEngine.NET.Data
{
	using Interfaces.Data;

	/// <summary>
	/// Ścieżka do właściwości lub pola.
	/// </summary>
	/// <remarks>
	/// Po utworzeniu(po ustawieniu na wartości poprawne Path i Root) nie można zmieniać typu(i tylko jego) wartości głównej.
	/// </remarks>
	public class PropertyPath
		//: IPropertyPath
	{
		#region IPropertyPath Members
		/// <summary>
		/// Ścieżka jako tekst.
		/// </summary>
		public string Path { get; private set; }

		/// <summary>
		/// Obiekt główny.
		/// </summary>
		public object Root { get; set; }

		/// <summary>
		/// Typ obiektu głównego.
		/// </summary>
		public Type RootType { get; private set; }

		/// <summary>
		/// Aktualna wartość.
		/// </summary>
		public object Value { get; set; }
		#endregion
	}
}
