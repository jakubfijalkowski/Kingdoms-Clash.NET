using System.Diagnostics;
using OpenTK.Input;

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
		private bool WasActive = false;
		private ushort _Clicked = 0;
		#endregion

		#region IButton Members
		/// <summary>
		/// Lista wciśniętych przycisków.
		/// </summary>
		/// <remarks>
		/// Poszczególne bity odpowiadają stanowi klawisza z MouseButton.
		/// </remarks>
		public ushort ClickedButtons
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

			int newState = 0;
			if (this.IsHot)
			{
				for (int i = 0; i < (int)MouseButton.LastButton; i++)
				{
					if (this.Data.Input[(MouseButton)i])
					{
						newState |= (1 << i);
					}
					else
					{
						newState &= ~(1 << i);
					}
				}
				if (this.WasActive && this.Data.Active == null)
				{
					newState |= (1 << 12);
				}
				else
				{
					newState &= ~(1 << 12);
				}

				this.ClickedButtons = (ushort)newState;
			}
			else
			{
				this.ClickedButtons = 0;
			}
		}

		/// <summary>
		/// Jeśli jest kliknięty zwraca 1, w przeciwnym razie 0.
		/// </summary>
		/// <returns></returns>
		public override int Check()
		{
			return this.ClickedButtons;
		}
		#endregion
	}
}
