﻿using System;
using System.ComponentModel;
using System.Windows.Markup;
using System.Xaml;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces;
	using Interfaces.Graphics.Gui;
	using Interfaces.Graphics.Gui.Conditions;

	/// <summary>
	/// Kontener XAML.
	/// </summary>
	[ContentProperty("Controls")]
	public class XamlGuiContainer
		: Container, IXamlGuiContainer
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");

		#region Private fields
		private IResourcesManager Manager = null;
		private IResourcesManager ParentManager = null;
		#endregion

		#region XAML Properties
		/// <summary>
		/// Kontrolki głównego elementu.
		/// </summary>
		public IControlsCollection Controls
		{
			get { return this.Root.Controls; }
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
			return ResourceLoadingState.Success;
		}

		public void Free()
		{
			this.Root.Controls.Clear();
			this.Manager.Dispose();
		}
		#endregion

		#region IXamlGuiContainer Members
		/// <summary>
		/// Warunki do stylizacji GUI.
		/// </summary>
		public IConditionsCollection Triggers { get; private set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje kontener.
		/// </summary>
		/// <param name="gameInfo">Informacje o grze.</param>
		public XamlGuiContainer(IGameInfo gameInfo)
			: base(gameInfo)
		{
			this.Triggers = new Internals.ConditionsCollection();
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
