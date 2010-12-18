using System;
using System.Windows.Markup;

namespace ClashEngine.NET.Data
{
	using System.Xaml;
	using Interfaces.Data;

	/// <summary>
	/// Rozszerzenie XAML, które pobiera i zwraca element główny dokumentu.
	/// </summary>
	[MarkupExtensionReturnType(typeof(object))]
	public class RootObjectExtension
		: MarkupExtension, IRootObjectExtension
	{
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			var rootProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
			if (rootProvider == null || rootProvider.RootObject == null)
			{
				throw new InvalidOperationException("IRootObjectProvider");
			}
			return rootProvider.RootObject;
		}
	}
}
