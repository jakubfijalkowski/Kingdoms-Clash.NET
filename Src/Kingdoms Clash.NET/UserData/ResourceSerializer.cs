using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using OpenTK;

namespace Kingdoms_Clash.NET.UserData
{
	using Interfaces.Resources;
	using Resources;

	/// <summary>
	/// Serializer dla zasobów.
	/// </summary>
	internal static  class ResourceSerializer
	{
		/// <summary>
		/// Deserializuje opis zasobu.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static IResourceDescription Deserialize(string filename)
		{
			var doc = new XmlDocument();
			doc.Load(filename);
			var res = doc["resource"];
			if (res == null)
			{
				throw new XmlException("Cannot find main element");
			}
			var converter = new ClashEngine.NET.Converters.Vector2Converter();

			string id = res.GetAttribute("id").Trim();
			var nameEl = res["name"];
			string name = string.Empty;
			string image = res.GetAttribute("image").Trim();
			if (string.IsNullOrEmpty(id) || nameEl == null || string.IsNullOrEmpty(name = nameEl.InnerText) || string.IsNullOrEmpty(image) ||
				string.IsNullOrEmpty(res.GetAttribute("size").Trim()))
			{
				throw new XmlException("Attributes 'id', 'name', 'image' and 'size' must not be empty");
			}
			Vector2 size = (Vector2)converter.ConvertFrom(res.GetAttribute("size").Trim());

			var descriptionEl = res["description"];
			string description = (descriptionEl != null ? descriptionEl.InnerText : string.Empty);

			List<Vector2> polygon = new List<Vector2>();
			var polygonEl = res["polygon"];
			if (polygonEl == null)
			{
				throw new XmlException("Element 'polygon' is not presented");
			}
			foreach (XmlElement point in polygonEl.GetElementsByTagName("point"))
			{
				polygon.Add((Vector2)converter.ConvertFrom(point.InnerText));
			}
			if (polygon.Count < 2)
			{
				throw new XmlException("Too less points in 'polygon' element. Minimal value: 2");
			}

			return new ResourceDescription(id, name, description, size, image, polygon.ToArray());
		}
	}
}
