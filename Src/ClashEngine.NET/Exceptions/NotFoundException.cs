using System;

namespace ClashEngine.NET.Exceptions
{
	/// <summary>
	/// Nie znaleziono.
	/// </summary>
	public class NotFoundException : Exception
	{
		/// <summary>
		/// CO nie zostało znalezione.
		/// </summary>
		public string What { get; private set; }

		public NotFoundException() { }
		public NotFoundException(string what) : base(string.Format("{0} was not found", what)) { this.What = what; }
		public NotFoundException(string message, Exception inner) : base(message, inner) { }
		public NotFoundException(string what, string message, Exception inner) : base(message, inner) { this.What = what; }
	}
}
