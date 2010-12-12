using System;
using OpenTK.Input;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;
	using Interfaces.Graphics.Gui.Controls;

	/// <summary>
	/// Metody rozszerzające klasę Container dodające łatwiejszą obsługę kontrolek.
	/// </summary>
	public static class ContainerExtension
	{
		/// <summary>
		/// Sprawdza, czy nad danym przyciskiem został puszczony LPM.
		/// </summary>
		/// <param name="container">this</param>
		/// <param name="id">Identyfikator.</param>
		/// <returns></returns>
		public static bool Button(this IContainer container, string id)
		{
			return container.Button(id, MouseButton.LastButton);
		}

		/// <summary>
		/// Sprawdza, czy dany klawisz myszki jest wciśnięty nad przyciskiem.
		/// </summary>
		/// <param name="container">this</param>
		/// <param name="id">Identyfikator.</param>
		/// <returns></returns>
		public static bool Button(this IContainer container, string id, MouseButton button)
		{
			return (container.Control(id) & (1 << (int)button)) != 0;
		}

		/// <summary>
		/// Pobiera tekst ze wskazanej kontrolki.
		/// </summary>
		/// <remarks>
		/// Kontrolka musi implementować ITextBox.
		/// </remarks>
		/// <param name="container">this</param>
		/// <param name="id">Identyfikator.</param>
		/// <returns></returns>
		public static string TextBox(this IContainer container, string id)
		{
			var ctrl = container.Controls[id];
			if (!(ctrl is ITextBox))
			{
				throw new ArgumentException("Control does not implement ITextBox interface");
			}
			return (ctrl as ITextBox).Text;
		}

		/// <summary>
		/// Pobiera listę wybranych elementów z rotatora.
		/// </summary>
		/// <remarks>
		/// Kontrolka musi implementować IRotator.
		/// </remarks>
		/// <param name="container">this</param>
		/// <param name="id">Identyfikator.</param>
		/// <returns></returns>
		public static IRotatorSelectedItems Rotator(this IContainer container, string id)
		{
			var ctrl = container.Controls[id];
			if (!(ctrl is IRotator))
			{
				throw new ArgumentException("Control does not implement ITextBox interface");
			}
			return (ctrl as IRotator).Selected;
		}
	}
}
