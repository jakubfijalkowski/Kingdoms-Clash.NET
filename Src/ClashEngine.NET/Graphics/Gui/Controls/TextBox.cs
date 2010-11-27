using System.ComponentModel;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Pole tekstowe.
	/// </summary>
	/// <remarks>
	/// PropertyChanged jest wywoływane dla:
	///    Text
	/// </remarks>
	public class TextBox
		: ControlBase, ITextBox, INotifyPropertyChanged
	{
		#region Private fields
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private string _Text = string.Empty;
		#endregion

		#region ITextBox Members
		/// <summary>
		/// Tekst.
		/// </summary>
		public string Text
		{
			get { return this._Text; }
			set
			{
				this._Text = value;
				base.SendPropertyChanged("Text");
			}
		}
		#endregion

		#region ControlBase Members
		/// <summary>
		/// Potrzebujemy aktywności na więcej niż jedną klatkę.
		/// </summary>
		public override bool PermanentActive
		{
			get { return true; }
		}

		/// <summary>
		/// Aktualizuje tekst, jeśli jest jaiś znak do aktualizacji.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{
			base.Update(delta);

			if (this.IsActive)
			{
				if (this.Data.Input.LastCharacter != '\0')
				{
					if (this.Data.Input.LastCharacter == '\b' && this.Text.Length > 0)
					{
						this.Text = this.Text.Remove(this.Text.Length - 1, 1);
					}
					else if (!char.IsControl(this.Data.Input.LastCharacter))
					{
						this.Text += this.Data.Input.LastCharacter;
					}
					this.Data.Input.LastCharacter = '\0';
				}
			}
		}

		/// <summary>
		/// Zwraca długość tekstu.
		/// </summary>
		/// <returns></returns>
		public override int Check()
		{
			return this.Text.Length;
		}
		#endregion
	}
}
