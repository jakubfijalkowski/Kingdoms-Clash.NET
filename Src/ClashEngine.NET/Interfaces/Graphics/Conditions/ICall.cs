using System.ComponentModel;
using System.Windows.Markup;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Conditions
{
	/// <summary>
	/// Wyzwalacz wywołujący metodę.
	/// </summary>
	public interface ICall
		: ITrigger
	{
		/// <summary>
		/// Obiekt na którym będziemy wywoływać metodę.
		/// </summary>
		[TypeConverter(typeof(NameReferenceConverter))]
		object Object { get; set; }

		/// <summary>
		/// Nazwa metody.
		/// </summary>
		string Method { get; set; }

		/// <summary>
		/// Parametry wywołania metody.
		/// </summary>
		IParametersCollection Parameters { get; }
	}
}
