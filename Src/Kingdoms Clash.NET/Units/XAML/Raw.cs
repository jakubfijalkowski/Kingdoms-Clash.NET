namespace Kingdoms_Clash.NET.Units.XAML
{
	/// <summary>
	/// Inny zasób - niezdefiniowany.
	/// </summary>
	[System.Windows.Markup.ContentProperty("Value")]
	public class Raw
		: IResource
	{
		#region IResource Members
		public string Name { get; set; }
		public uint Value { get; set; }
		#endregion
	}
}
