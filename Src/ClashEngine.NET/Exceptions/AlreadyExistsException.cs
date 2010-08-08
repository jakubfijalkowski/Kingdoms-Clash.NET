using System;

namespace ClashEngine.NET.Exceptions
{
	/// <summary>
	/// Coś już istnieje.
	/// </summary>
	public class AlreadyExistsException
		: Exception
	{
		/// <summary>
		/// Inicjalizuje wyjątek.
		/// </summary>
		/// <param name="what">CO już istnieje..</param>
		public AlreadyExistsException(string what)
			: base(string.Format("{0} already exists"))
		{ }
	}
}
