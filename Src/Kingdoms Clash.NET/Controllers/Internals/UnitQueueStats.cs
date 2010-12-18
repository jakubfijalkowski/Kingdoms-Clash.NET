namespace Kingdoms_Clash.NET.Controllers.Internals
{
	using Interfaces.Controllers;
	using Interfaces.Units;

	internal class UnitQueueStats
		: IUnitQueueStats
	{
		#region IUnitQueueStats Members
		public IUnitRequestToken CurrentToken { get; private set; }
		public uint QueueLength { get; set; }
		#endregion

		public UnitQueueStats(IUnitRequestToken token, uint length)
		{
			this.CurrentToken = token;
			this.QueueLength = length;
		}
	}
}
