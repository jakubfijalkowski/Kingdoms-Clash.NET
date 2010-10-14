using System;
using System.Linq;
using System.Xml;

namespace Kingdoms_Clash.NET.UserData
{
	using Interfaces.Resources;

	/// <summary>
	/// Klasa-serializer dla kolekcji zasobów.
	/// </summary>
	/// <remarks>
	/// XML, na którym operujemy ma format:
	/// Serializuje jako:
	/// &lt;elementname&gt;
	///		&lt;resId value="resValue" /&gt;
	///		&lt;res2Id value="res2Value" /&gt;
	/// &lt;/elementname&gt;
	/// </remarks>
	class ResourcesCollectionSerializer
		: Interfaces.Serialization.IXmlSerializable
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");
		
		/// <summary>
		/// Kolekcja zasobów do serializacji.
		/// </summary>
		public IResourcesCollection Resources { get; set; }

		#region IXmlSerializable Members
		/// <summary>
		/// Serializuje obiekt Resources do XML.
		/// </summary>
		/// <param name="element"></param>
		public void Serialize(XmlElement element)
		{
			var doc = element.OwnerDocument;

			foreach (var res in this.Resources)
			{
				var el = doc.CreateElement(res.Key);
				el.SetAttribute("value", res.Value.ToString());
				element.AppendChild(el);
			}
		}

		/// <summary>
		/// Deserializuje obiekt z XML do właściwości Resources.
		/// </summary>
		/// <param name="element"></param>
		public void Deserialize(XmlElement element)
		{
			foreach (XmlElement el in element.ChildNodes.OfType<XmlElement>())
			{
				if (!Kingdoms_Clash.NET.Resources.ResourcesList.Instance.Exists(el.Name))
				{
					Logger.Warn("Resource {0} does not exists, skipping", el.Name);
					continue;
				}
				uint value = 0;
				try
				{
					value = uint.Parse(el.GetAttribute("value"));
				}
				catch
				{
					Logger.Warn("Cannot parse value for {0}, skipping", el.Name);
					continue;
				}
				this.Resources.Add(el.Name, value);
			}
		}
		#endregion

		/// <summary>
		/// Inicjalizuje serializer.
		/// </summary>
		/// <param name="resources">Lita zasobów.</param>
		public ResourcesCollectionSerializer(IResourcesCollection resources)
		{
			if (resources == null)
			{
				throw new ArgumentNullException("resources");
			}
			this.Resources = resources;
		}
	}
}
