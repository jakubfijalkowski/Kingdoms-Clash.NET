using System.IO;

namespace ClashEngine.NET.ResourcesManager
{
	using Interfaces.ResourcesManager;

	/// <summary>
	/// Manager zasobów umożliwiający podmianę plików "na gorąco"(w trakcie działania aplikacji).
	/// Do tego celu wykorzystuje System.IO.FileSystemWatcher
	/// </summary>
	internal class HotReplaceResourcesManager
		: ResourcesManager
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
			this.Watcher.Changed += new FileSystemEventHandler(Watcher_Changed);
			this.Watcher.EnableRaisingEvents = true;
		}

		void Watcher_Changed(object sender, FileSystemEventArgs e)
		{
			var id = e.FullPath.Replace(this.ContentDirectory + "\\", "");
			IResource res;
			if (base.Resources.TryGetValue(id.Replace('\\', '/'), out res))
			{
				//Zwalniamy i tworzymy od nowa.
				Logger.Debug("Resource {0} modified. Reloading.", id);
				res.Free();
				res.Load();
			}
		}
	}
}
