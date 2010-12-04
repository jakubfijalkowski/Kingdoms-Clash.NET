namespace Kingdoms_Clash.NET.Units.XAML
{
	/// <summary>
	/// Zasób - drewno.
	/// </summary>
	[System.Windows.Markup.ContentProperty("Value")]
	public class Wood
		: IResource
	{
		#region IResource Members
		public string Name
		{
			get { return "wood"; }
		}

		public uint Value { get; set; }
		#endregion
	}
}
