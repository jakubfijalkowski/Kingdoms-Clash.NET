namespace ClashEngine.NET.Interfaces.Graphics.Gui.Conditions
{
	/// <summary>
	/// Wielowarunkowy warunek.
	/// Np.
	/// a && (b || c) || ((d || e) && f)
	/// </summary>
	public interface IMultiIf
		: ICondition, IMultiIfConditionsCollection
	{ }
}
