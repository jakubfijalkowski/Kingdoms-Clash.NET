using System;
using System.Diagnostics;
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

		#region Private fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool Usable = true;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IResourcesManager Manager = null;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IResourcesManager ParentManager = null;
		#endregion

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
		/// Warunki do stylizacji GUI.
		/// </summary>
		IConditionsCollection IXamlGuiContainer.Triggers { get { return this.Triggers; } }

		/// <summary>
		/// Warunki do stylizacji GUI.
		/// </summary>
		public ConditionsCollection Triggers { get; private set; }

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
		IResourcesManager IResource.Manager 
		{
			get { return this.Manager; }
			set { this.ParentManager = value; }
		}

		/// <summary>
		/// Ładuje kontrolkę z pliku XAML. Zwraca Failure w przypadku niepowodzenia, inaczej Success.
		/// </summary>
		/// <returns></returns>
		public Interfaces.ResourceLoadingState Load()
		{
			try
			{
				Logger.Info("Creating resource manager for GUI");
				if (this.ParentManager is ICloneable)
				{
					this.Manager = (this.ParentManager as ICloneable).Clone() as IResourcesManager;
				}
				else
				{
					this.Manager = new ResourcesManager();
					this.Manager.ContentDirectory = this.ParentManager.ContentDirectory;
				}
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
			this.Manager.Dispose();
			this.Usable = true;
		}
		#endregion

		#region Constructors
		public XamlGuiContainer()
		{
			this.Controls = new ControlsCollection();
			this.Triggers = new ConditionsCollection();
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
