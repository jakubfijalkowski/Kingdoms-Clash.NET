namespace ClashEngine.NET.Graphics.Gui.Conditions
{
	using Interfaces.Graphics.Gui.Conditions;

	/// <summary>
	/// Wielowarunkowy warunek.
	/// </summary>
	public class MultiIf
		: Conditions, IMultiIf
	{
		#region ICondition Members
		/// <summary>
		/// Wyzwalacze wywoływane przy spełnieniu warunku.
		/// </summary>
		public ITriggersCollection Triggers { get; private set; }
		#endregion

		#region Constructors
		public MultiIf()
		{
			this.Triggers = new TriggersCollection();
			base.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(MultiIf_PropertyChanged);
		}
		#endregion

		#region Private methods
		void MultiIf_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (base.Value)
			{
				this.Triggers.TrigAll();
			}
		}
		#endregion
	}
}
