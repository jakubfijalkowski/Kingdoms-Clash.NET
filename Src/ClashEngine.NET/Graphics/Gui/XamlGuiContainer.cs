using System;
using System.Xaml;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces;
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Kontener XAML.
	/// </summary>
	[System.Windows.Markup.ContentProperty("Controls")]
	public class XamlGuiContainer
		: IXamlGuiContainer
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");
		private bool Usable = true;

		#region IXamlGuiContainer Members
		/// <summary>
		/// Kontrolki.
		/// </summary>
		IControlsCollection IXamlGuiContainer.Controls { get { return this.Controls; } }

		/// <summary>
		/// Kontrolki.
		/// </summary>
		public ControlsCollection Controls { get; private set; }

		/// <summary>
		/// Binduje kontrner XAML do kontenera GUI.
		/// </summary>
		/// <param name="container"></param>
		public void Bind(Interfaces.Graphics.Gui.IContainer container)
		{
			if (!this.Usable)
			{
				throw new NotSupportedException("Multiple binding to container is not supported");
			}
			this.Usable = false;
			container.Controls.AddRange(this.Controls);
		}

		/// <summary>
		/// Zapisuje kontener do wskazanego wyjścia.
		/// </summary>
		/// <param name="output">Wyjście.</param>
		public void Save(System.IO.TextWriter output)
		{
			XamlServices.Save(output, this);
		}
		#endregion

		#region IResource Members
		string IResource.Id { get; set; }
		string IResource.FileName { get; set; }
		IResourcesManager IResource.Manager { get; set; }

		/// <summary>
		/// Ładuje kontrolkę z pliku XAML. Zwraca Failure w przypadku niepowodzenia, inaczej Success.
		/// </summary>
		/// <returns></returns>
		public Interfaces.ResourceLoadingState Load()
		{
			try
			{
				XamlXmlReader reader = new XamlXmlReader((this as IResource).FileName);
				XamlObjectWriter writer = new XamlObjectWriter(reader.SchemaContext, new XamlObjectWriterSettings
				{
					RootObjectInstance = this
				});
				XamlServices.Transform(reader, writer);
			}
			catch (Exception ex)
			{
				Logger.ErrorException("Cannot load GUI from XAML", ex);
				return ResourceLoadingState.Failure;
			}
			return ResourceLoadingState.Success;
		}

		public void Free()
		{
			this.Controls.Clear();
			this.Usable = true;
		}
		#endregion

		#region Constructors
		public XamlGuiContainer()
		{
			this.Controls = new ControlsCollection();
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			this.Free();
		}
		#endregion
	}
}
