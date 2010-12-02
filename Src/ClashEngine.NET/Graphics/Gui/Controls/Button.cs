using System.Diagnostics;

namespace ClashEngine.NET.Graphics.Gui.Controls
{
	using Interfaces.Graphics.Gui.Controls;

	/// <summary>
	/// Przycisk.
	/// </summary>
	/// <remarks>
	///	PropertyChanged jest wywoływane dla:
	///	  Clicked
	/// </remarks>
	public class Button
		: ControlBase, IButton
	{
		#region Private fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool WasActive = false;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool _Clicked = false;
		#endregion
		
		#region IButton Members
		/// <summary>
		/// Czy przycisk jest wciśnięty.
		/// </summary>
		public bool Clicked
		{
			get { return this._Clicked; }
			private set
			{
				if (value != this._Clicked)
				{
					this._Clicked = value;
					base.SendPropertyChanged("Clicked");
				}
			}
		}
		#endregion

		#region ControlBase Members
		/// <summary>
		/// Nie potrzebujemy być aktywnym dłuższy czas.
		/// </summary>
		public override bool PermanentActive
		{
			get { return false; }
		}

		/// <summary>
		/// Jeśli puszczono myszkę nad przyciskiem to ustawiamy Clicked na true.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{
			this.WasActive = this.IsActive;
			base.Update(delta);

			this.Clicked = this.WasActive && this.Data.Active == null && this.IsHot;
		}

		/// <summary>
		/// Jeśli jest kliknięty zwraca 1, w przeciwnym razie 0.
		/// </summary>
		/// <returns></returns>
		public override int Check()
		{
			return (this.Clicked ? 1 : 0);
		}
		#endregion
	}
}
