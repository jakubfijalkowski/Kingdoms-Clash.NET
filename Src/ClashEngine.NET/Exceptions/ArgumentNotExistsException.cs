using System;

namespace ClashEngine.NET.Exceptions
{
	/// <summary>
	/// Obiekt wskazywany przez parametr nie istnieje.
	/// </summary>
	public class ArgumentNotExistsException
		: ArgumentException
	{
		public ArgumentNotExistsException() : base("Not exists") { }
		public ArgumentNotExistsException(string paramName) : base("Not exists", paramName) { }
		public ArgumentNotExistsException(string paramName, string message) : base(message, paramName) { }
		public ArgumentNotExistsException(string paramName, string message, Exception inner) : base(message, paramName, inner) { }
		public ArgumentNotExistsException(string message, Exception inner) : base(message, inner) { }

		protected ArgumentNotExistsException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
