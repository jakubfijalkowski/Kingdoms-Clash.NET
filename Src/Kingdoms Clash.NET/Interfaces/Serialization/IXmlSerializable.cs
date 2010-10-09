using System.Xml;

namespace Kingdoms_Clash.NET.Interfaces.Serialization
{
	/// <summary>
	/// Interfejs do serializacji XML.
	/// Nie używamy wbudowanego serializera gdyż nie udostępnia on serializowania jako DOM tylko SAX.
	/// </summary>
	public interface IXmlSerializable
	{
		/// <summary>
		/// Serializuje obiekt.
		/// </summary>
		/// <param name="element">Element w który serializujemy.</param>
		void Serialize(XmlElement element);

		/// <summary>
		/// Deserializuje obiekt.
		/// </summary>
		/// <param name="element">Element który deserializujemy.</param>
		void Deserialize(XmlElement element);
	}
}
