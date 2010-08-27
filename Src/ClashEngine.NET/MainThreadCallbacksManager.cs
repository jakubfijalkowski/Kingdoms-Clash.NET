using System.Collections.Generic;

namespace ClashEngine.NET
{
	using Interfaces;

	/// <summary>
	/// Manager globalnych callbacków.
	/// Callbacki wywoływane są w metodzie Update obiektu gry.
	/// </summary>
	public class MainThreadCallbacksManager
		: IMainThreadCallbacksManager
	{
		private List<MainThreadCallback> Callbacks = new List<MainThreadCallback>();

		#region Singleton
		private static IMainThreadCallbacksManager Instance_;

		/// <summary>
		/// Instancja managera.
		/// </summary>
		public static IMainThreadCallbacksManager Instance
		{
			get
			{
				if (Instance_ == null)
				{
					Instance_ = new MainThreadCallbacksManager();
				}
				return Instance_;
			}
		}

		private MainThreadCallbacksManager()
		{ }
		#endregion

		#region IMainThreadCallbacksManager members
		/// <summary>
		/// Dodaje callback do listy.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void Add(MainThreadCallback callback)
		{
			this.Callbacks.Add(callback);
		}

		/// <summary>
		/// Wywołuje WSZYSTKIE dodane callbacki i usuwa te, które tego zarządają.
		/// </summary>
		public void Call()
		{
			for (int i = 0; i < this.Callbacks.Count; i++)
			{
				if (this.Callbacks[i]())
				{
					this.Callbacks.RemoveAt(i);
					--i;
				}
			}
		}
		#endregion
	}
}
