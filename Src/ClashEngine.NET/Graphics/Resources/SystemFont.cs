using System;
using System.Drawing;
using OpenTK;

namespace ClashEngine.NET.Graphics.Resources
{
	using Extensions;
	using Interfaces.Graphics.Objects;
	using Interfaces.Graphics.Resources;

	/// <summary>
	/// Czcionka systemowa.
	/// Używa System.Drawing.Font.
	/// </summary>
	public class SystemFont
		: IFont
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");

		#region Private fields
		private Bitmap MeasuringBitmap;
		private System.Drawing.Graphics Measuring;
		private Font Font;
		#endregion
		
		#region IFont Members
		/// <summary>
		/// Nazwa czcionki.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Rozmiar w pikselach.
		/// </summary>
		public int Size { get; private set; }

		/// <summary>
		/// Czy jest to czcionka pogrubiona.
		/// </summary>
		public bool Bold { get; private set; }

		/// <summary>
		/// Czy jest to czcionka pochylona.
		/// </summary>
		public bool Italic { get; private set; }

		#region Single line
		/// <summary>
		/// Rysuje tekst na obiekt renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <returns>Utworzony obiekt.</returns>
		public IText Draw(string text, Color color)
		{
			var obj = new Objects.Internals.SystemFontObject();
			obj.Texture = CreateEmptyTexture();
			this.DrawString(text, color, obj.Texture);
			obj.Content = text;
			return obj;
		}

		/// <summary>
		/// Rysuje tekst na obiekt renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <returns>Utworzony obiekt.</returns>
		public IText Draw(string text, Vector4 color)
		{
			return this.Draw(text, color.ToColor());
		}

		/// <summary>
		/// Rysuje tekst na obiekt renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="into">Obiekt(utworzony wcześniej przez rodzica) w który zostanie wrysowany tekst.</param>
		public void Draw(string text, Color color, IText into)
		{
			if (into == null)
			{
				throw new ArgumentNullException("item");
			}
			else if (!(into is Objects.Internals.SystemFontObject))
			{
				throw new ArgumentException("Text object wasn't created by this class", "into");
			}
			this.DrawString(text, color, (into as Objects.Internals.SystemFontObject).Texture);
			(into as Objects.Internals.SystemFontObject).Content = text;
		}

		/// <summary>
		/// Rysuje tekst na obiekt renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="into">Obiekt(utworzony wcześniej przez rodzica) w który zostanie wrysowany tekst.</param>
		public void Draw(string text, Vector4 color, IText into)
		{
			this.Draw(text, color.ToColor(), into);
		}
		#endregion

		#region Textbox
		/// <summary>
		/// Rysuje tekst do obiektu renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="textBox">Pole tekstowe, w którym ma się zmieścić tekst.</param>
		/// <returns>Utworzony obiekt.</returns>
		public IText Draw(string text, Color color, RectangleF textBox)
		{
			IText textObj = new Objects.Internals.SystemFontObject()
			{
				Texture = CreateEmptyTexture()
			};
			this.Draw(text, color, textBox, textObj);
			return textObj;
		}

		/// <summary>
		/// Rysuje tekst do obiektu renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <returns>Utworzony obiekt.</returns>
		public IText Draw(string text, Vector4 color, RectangleF textBox)
		{
			return this.Draw(text, color.ToColor(), textBox);
		}

		/// <summary>
		/// Rysuje tekst do obiektu renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="textBox">Pole tekstowe, w którym ma się zmieścić tekst.</param>
		/// <param name="into">Obiekt(utworzony wcześniej przez rodzica) w który zostanie wrysowany tekst.</param>
		public void Draw(string text, Color color, RectangleF textBox, IText into)
		{
			if (into == null)
			{
				throw new ArgumentNullException("into");
			}
			if(!(into is Objects.Internals.SystemFontObject))
			{
				throw new ArgumentException("Text object wasn't created by this class", "into");
			}

			(into as Objects.Internals.SystemFontObject).Content = text;
			using (var bitmap = new Bitmap((int)(textBox.Left + textBox.Width), (int)(textBox.Top + textBox.Height)))
			{
				using (var g = System.Drawing.Graphics.FromImage(bitmap))
				{
					g.FillRectangle(Brushes.Transparent, 0, 0, bitmap.Width, bitmap.Height);
					g.DrawString(text, this.Font, new SolidBrush(color), textBox);
				}
				((into as Objects.Internals.SystemFontObject).Texture as Internals.ChangableTexture).Set(bitmap);
			}
		}

		/// <summary>
		/// Rysuje tekst do obiektu renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="textBox">Pole tekstowe, w którym ma się zmieścić tekst.</param>
		/// <param name="into">Obiekt(utworzony wcześniej przez rodzica) w który zostanie wrysowany tekst.</param>
		public void Draw(string text, Vector4 color, RectangleF textBox, IText into)
		{
			this.Draw(text, color.ToColor(), textBox, into);
		}
		#endregion

		/// <summary>
		/// Tworzy pusty obiekt na tekst.
		/// </summary>
		/// <returns></returns>
		public static IText CreateEmptyObject()
		{
			var obj = new Objects.Internals.SystemFontObject();
			obj.Texture = CreateEmptyTexture();
			return obj;
		}
		#endregion

		#region IResource Members
		/// <summary>
		/// Identyfikator czcionki - nazwa,rozmiar_w_pikselach,[ib]
		/// i - pochyła
		/// b - pogrubiona
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Nazwa czcionki - identycznie jak <see cref="Id"/>.
		/// </summary>
		public string FileName
		{
			get { return this.Id; }
			set { }
		}

		/// <summary>
		/// Manager-rodzic zasobu.
		/// </summary>
		public Interfaces.IResourcesManager Manager { get; set; }

		/// <summary>
		/// Wczytuje czcionkę parsując Id.
		/// </summary>
		/// <returns>Failure, gdy Id ma niepoprawny format, DefaultUsed, gdy nie znaleziono czcionki, inaczej Success.</returns>
		public Interfaces.ResourceLoadingState Load()
		{
			string[] values = this.Id.Split(',');
			int size = 0;
			if (values.Length != 3 || !int.TryParse(values[1].Trim(), out size))
			{
				return Interfaces.ResourceLoadingState.Failure;
			}
			this.Name = values[0].Trim();
			this.Size = size;
			this.Italic = values[2].Contains("i");
			this.Bold = values[2].Contains("b");

			this.Font = new Font(this.Name, this.Size,
				(this.Italic ? FontStyle.Italic : FontStyle.Regular) | (this.Bold ? FontStyle.Bold : FontStyle.Regular), GraphicsUnit.Pixel);
			if (this.Font.Name == "Microsoft Sans Serif")
			{
				return Interfaces.ResourceLoadingState.DefaultUsed;
			}

			return Interfaces.ResourceLoadingState.Success;
		}

		/// <summary>
		/// Zwalnia czcionkę.
		/// </summary>
		public void Free()
		{
			this.Measuring.Dispose();
			this.MeasuringBitmap.Dispose();
			this.Font.Dispose();
		}
		#endregion

		#region Constructors
		public SystemFont()
		{
			this.MeasuringBitmap = new Bitmap(1, 1);
			this.Measuring = System.Drawing.Graphics.FromImage(this.MeasuringBitmap);
		}
		#endregion

		#region Private members
		/// <summary>
		/// Rysuje tekst na istniejącą teksturę.
		/// Jeśli onto ma puste Id to ustawia mu je na wartość "Text.(text)" i rejestruje w managerze.
		/// </summary>
		/// <param name="text">Tekst do wypiania.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="onto">Tekstura. Musi być to tekstura zwrócona przez DrawString.</param>
		/// <exception cref="ArgumentNullException">onto jest nullem lub text jest pusty/nullem.</exception>
		/// <exception cref="ArgumentException">Tekstura onto nie była stworzona przez metodę CreateEmptyText.</exception>
		private void DrawString(string text, Color color, ITexture onto)
		{
			if (string.IsNullOrEmpty(onto.Id))
			{
				onto.Id = "Text." + text;
				this.Manager.Add(onto);
			}

			var size = this.Measuring.MeasureString(text, this.Font);
			if (size.Width == 0)
			{
				size.Width = 1;
			}
			if (size.Height == 0)
			{
				size.Height = 1;
			}
			using (var bm = new Bitmap((int)size.Width, (int)size.Height))
			{
				using (var g = System.Drawing.Graphics.FromImage(bm))
				{
					g.FillRectangle(Brushes.Transparent, 0, 0, bm.Width, bm.Height);
					g.DrawString(text, this.Font, new SolidBrush(color), 0f, 0f);
				}
				(onto as Internals.ChangableTexture).Set(bm);
			}
		}

		/// <summary>
		/// Tworzy pustą teksturę, by móc jej później użyć w metodzie <see cref="DrawString(text,ITexture)"/>.
		/// </summary>
		/// <returns>Nowa tekstura.</returns>
		private static ITexture CreateEmptyTexture()
		{
			return new Internals.ChangableTexture();
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			this.Free();
		}
		#endregion
	}
}
