using System;

namespace ClashEngine.NET.Exceptions
{
	/// <summary>
	/// Obiekt wskazywany przez argument już istnieje.
	/// </summary>
	public class ArgumentAlreadyExistsException
		: ArgumentException
	{
		public ArgumentAlreadyExistsException() : base("Already exists") { }
		public ArgumentAlreadyExistsException(string paramName) : base("Already exists", paramName) { }
		public ArgumentAlreadyExistsException(string paramName, string message) : base(message, paramName) { }
		public ArgumentAlreadyExistsException(string paramName, string message, Exception inner) : base(message, paramName, inner) { }
		public ArgumentAlreadyExistsException(string message, Exception inner) : base(message, inner) { }

		protected ArgumentAlreadyExistsException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
