using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Kingdoms_Clash.NET.UserData
{
	using Interfaces.Units;
	using Units;

	/// <summary>
	/// Klasa pomocnicza dla loadera nacji
	/// </summary>
	internal class NationLoader
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");

		/// <summary>
		/// Plik z nacją.
		/// </summary>
		private string NationFile = string.Empty;

		/// <summary>
		/// Lista dostępnych komponentów.
		/// </summary>
		private List<Type> Components = null;

		/// <summary>
		/// Inicjalizuje loader.
		/// </summary>
		/// <param name="file">Plik z nacją.</param>
		/// <param name="components">Lista dostępnych komponentów.</param>
		public NationLoader(string file, IEnumerable<Type> components)
		{
			this.NationFile = file;
			this.Components = new List<Type>();
			foreach (var c in components)
			{
				if (c.GetConstructor(Type.EmptyTypes) == null)
				{
					Logger.Warn("Component {0} does not have parameterless constructor, skipping", c.Name);
				}
				else if (!c.GetInterfaces().Any(i => i == typeof(IUnitComponentDescription)))
				{
					//To się nie powinno zdarzyć, ale człowiek nie jest nieomylny, więc lepiej się zabezpieczyć.
					throw new ArgumentException(string.Format("Component description '{0}' does not derive from IUnitComponentDescription", c.Name));
				}
				else
				{
					this.Components.Add(c);
				}
			}
		}

		/// <summary>
		/// Tworzy nację z podanego pliku.
		/// </summary>
		/// <returns>Obiekt nacji lub null, gdy nie dało się utworzyć.</returns>
		public Interfaces.Units.INation Create()
		{
			XmlDocument document = new XmlDocument();
			try
			{
				document.Load(this.NationFile);
				if (document.DocumentElement.Name != "nation")
				{
					throw new XmlException("Root element is not nation");
				}
				string name = document.DocumentElement.GetAttribute("name");
				string image = document.DocumentElement.GetAttribute("image");
				if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(image))
				{
					throw new XmlException("Nation must have 'name' and 'image' attributes");
				}
				
				List<IUnitDescription> units = new List<IUnitDescription>();

				foreach (XmlElement unit in document.DocumentElement.GetElementsByTagName("unit"))
				{
					units.Add(this.LoadUnit(unit));
				}
				
				return new Nation(name, image, units);
			}
			catch(Exception ex)
			{
				Logger.WarnException("Cannot load nation from file " + this.NationFile, ex);
				return null;
			}
		}

		/// <summary>
		/// Deserializuje jednostkę z podanego elementu.
		/// </summary>
		/// <param name="unit">Element opisujący jednostkę.</param>
		/// <returns>Jednostka.</returns>
		private IUnitDescription LoadUnit(XmlElement unit)
		{
			#region Validation
			if (!unit.HasAttribute("id") || !unit.HasAttribute("health") || !unit.HasAttribute("width") || !unit.HasAttribute("height"))
			{
				throw new XmlException("Element nation must have id, health, width and height attributes");
			}

			string id = unit.GetAttribute("id");
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("Attribute id must not be null or empty");
			}

			int health = int.Parse(unit.GetAttribute("health"));
			if (health <= 0)
			{
				throw new ArgumentException("Attribute health must be grater than 0");
			}

			float width = int.Parse(unit.GetAttribute("width"));
			if (width <= 0f)
			{
				throw new ArgumentException("Attribute width must be grater than 0");
			}

			float height = int.Parse(unit.GetAttribute("height"));
			if (height <= 0f)
			{
				throw new ArgumentException("Attribute height must be grater than 0");
			}
			#endregion

			IUnitDescription desc = new UnitDescription(id, health, width, height);

			#region Components
			var components = unit["components"];
			if (components == null)
			{
				throw new XmlException("Unit description must contain 'components' section");
			}
			foreach (XmlElement el in components.ChildNodes)
			{
				var componentType = this.Components.Find(t => t.Name.ToLower() == el.Name.ToLower());
				if (componentType == null)
				{
					Logger.Warn("Cannot find component '{0}'", el.Name.ToLower());
					continue;
				}
				IUnitComponentDescription componentDesc = Activator.CreateInstance(componentType) as IUnitComponentDescription;
				try
				{
					componentDesc.Deserialize(el);
				}
				catch (Exception ex)
				{
					Logger.WarnException("Cannot deserialize component description for " + componentType.Name, ex);
				}
				desc.Components.Add(componentDesc);
			}
			#endregion

			var costs = unit["costs"];
			if (costs == null)
			{
				throw new XmlException("Unit description must contain 'costs' section");
			}
			new ResourcesCollectionSerializer(desc.Costs).Deserialize(costs);
			
			return desc;
		}
	}
}
