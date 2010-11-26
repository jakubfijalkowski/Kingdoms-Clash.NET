using System.IO;

namespace ClashEngine.NET
{
	using Interfaces;

	/// <summary>
	/// Manager zasobów umożliwiający podmianę plików "na gorąco"(w trakcie działania aplikacji).
	/// Do tego celu wykorzystuje System.IO.FileSystemWatcher.
	/// </summary>
	public class HotReplaceResourcesManager
		: ResourcesManager, System.ICloneable
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");
		
		private FileSystemWatcher Watcher;

		public override string ContentDirectory
		{
			get
			{
				return base.ContentDirectory;
			}
			set
			{
				base.ContentDirectory = value;
				this.Watcher.Path = base.ContentDirectory;
			}
		}

		public HotReplaceResourcesManager()
		{
			Logger.Debug("Using resources manager supporting \"hot replace\"");
			this.Watcher = new FileSystemWatcher(base.ContentDirectory);
			this.Watcher.BeginInit();
			this.Watcher.NotifyFilter = NotifyFilters.LastWrite;
			this.Watcher.Changed += new FileSystemEventHandler(Watcher_Changed);
			this.Watcher.EnableRaisingEvents = true;
			this.Watcher.IncludeSubdirectories = true;
			this.Watcher.EndInit();
		}

		void Watcher_Changed(object sender, FileSystemEventArgs e)
		{
			IResource res;
			string name = ResourcesManager.PrepareId(e.Name.Replace('\\', '/'));
			if (base.Resources.TryGetValue(name, out res))
			{
				//Zwalniamy i tworzymy od nowa.
				Logger.Debug("Resource {0} modified. Reloading. {1}", name, e.ChangeType);

				//Musimy to wywołać na głównym wątku - kontekst OpenGL jest thread-specific
				MainThreadCallbacksManager.Instance.Add(() =>
					{
						res.Free();
						res.Load();
						return true;
					});
			}
		}

		#region ICloneable Members
		public object Clone()
		{
			return new HotReplaceResourcesManager()
			{
				ContentDirectory = this.ContentDirectory
			};
		}
		#endregion
	}
}
