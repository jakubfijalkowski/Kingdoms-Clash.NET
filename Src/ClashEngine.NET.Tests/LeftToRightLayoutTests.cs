using System.Collections.Generic;
using ClashEngine.NET.Graphics.Gui.Layout;
using NUnit.Framework;
using OpenTK;

namespace ClashEngine.NET.Tests
{
	using TestObjects;

	[TestFixture(Description = "Testuje silnik layouty: LeftToRight")]
	public class LeftToRightLayoutTests
	{
		#region Multiline
		[Test]
		public void MultilineAllElementsFitOneLine()
		{
			var layoutEngine = new LeftToRightLayout();
			List<PositionableElement> lst = new List<PositionableElement>();
			lst.Add(new PositionableElement(1, 1));
			lst.Add(new PositionableElement(2, 2));
			lst.Add(new PositionableElement(3, 1));

			Assert.AreEqual(new OpenTK.Vector2(8, 2), layoutEngine.Layout(lst, new OpenTK.Vector2(8, 2)), "Invalid resize");

			PositionTestHelper(lst, new Vector2(0, 0), new Vector2(1, 0), new Vector2(3, 0));
		}

		[Test]
		public void MultilineAllElementsFitOneLineAlignRight()
		{
			var layoutEngine = new LeftToRightLayout() { AlignRight = true };
			List<PositionableElement> lst = new List<PositionableElement>();
			lst.Add(new PositionableElement(1, 1));
			lst.Add(new PositionableElement(2, 2));
			lst.Add(new PositionableElement(3, 1));

			Assert.AreEqual(new OpenTK.Vector2(8, 2), layoutEngine.Layout(lst, new OpenTK.Vector2(8, 2)), "Invalid resize");

			PositionTestHelper(lst, new Vector2(2, 0), new Vector2(3, 0), new Vector2(5, 0));
		}

		[Test]
		public void MultilineAllElementsFitThreeLines()
		{
			var layoutEngine = new LeftToRightLayout();
			List<PositionableElement> lst = new List<PositionableElement>();
			lst.Add(new PositionableElement(1, 1));
			lst.Add(new PositionableElement(2, 1));
			lst.Add(new PositionableElement(3, 2));
			lst.Add(new PositionableElement(2, 1));

			Assert.AreEqual(new OpenTK.Vector2(3, 5), layoutEngine.Layout(lst, new OpenTK.Vector2(3, 1)), "Invalid resize");

			PositionTestHelper(lst, new Vector2(0, 0), new Vector2(1, 0),
				new Vector2(0, 1),
				new Vector2(0, 3));
		}

		[Test]
		public void MultilineAllElementsFitThreeLinesAlignRight()
		{
			var layoutEngine = new LeftToRightLayout() { AlignRight = true };
			List<PositionableElement> lst = new List<PositionableElement>();
			lst.Add(new PositionableElement(1, 1));
			lst.Add(new PositionableElement(2, 1));
			lst.Add(new PositionableElement(3, 2));
			lst.Add(new PositionableElement(2, 1));

			Assert.AreEqual(new OpenTK.Vector2(4, 5), layoutEngine.Layout(lst, new OpenTK.Vector2(4, 1)), "Invalid resize");

			PositionTestHelper(lst, new Vector2(1, 0), new Vector2(2, 0),
				new Vector2(1, 1),
				new Vector2(2, 3));
		}

		[Test]
		public void MultilineElementsDontFitAnyLine()
		{
			var layoutEngine = new LeftToRightLayout();
			List<PositionableElement> lst = new List<PositionableElement>();
			lst.Add(new PositionableElement(3, 1));
			lst.Add(new PositionableElement(5, 1));

			Assert.AreEqual(new OpenTK.Vector2(5, 2), layoutEngine.Layout(lst, new OpenTK.Vector2(1, 1)), "Invalid resize");

			PositionTestHelper(lst, new Vector2(0, 0), new Vector2(0, 1));
		}

		[Test]
		public void MultilineElementsDontFitAnyLineAlignRight()
		{
			var layoutEngine = new LeftToRightLayout() { AlignRight = true };
			List<PositionableElement> lst = new List<PositionableElement>();
			lst.Add(new PositionableElement(3, 1));
			lst.Add(new PositionableElement(5, 1));

			Assert.AreEqual(new OpenTK.Vector2(5, 2), layoutEngine.Layout(lst, new OpenTK.Vector2(1, 1)), "Invalid resize");

			PositionTestHelper(lst, new Vector2(2, 0), new Vector2(0, 1));
		}
		#endregion

		#region Singleline
		[Test]
		public void SinglelineElementsFitOneLine()
		{
			var layoutEngine = new LeftToRightLayout() { Singleline = true };
			List<PositionableElement> lst = new List<PositionableElement>();
			lst.Add(new PositionableElement(1, 1));
			lst.Add(new PositionableElement(2, 2));
			lst.Add(new PositionableElement(3, 1));

			Assert.AreEqual(new OpenTK.Vector2(8, 2), layoutEngine.Layout(lst, new OpenTK.Vector2(8, 2)), "Invalid resize");

			PositionTestHelper(lst, new Vector2(0, 0), new Vector2(1, 0), new Vector2(3, 0));
		}

		[Test]
		public void SinglelineElementsFitOneLineAlignRight()
		{
			var layoutEngine = new LeftToRightLayout() { Singleline = true, AlignRight = true };
			List<PositionableElement> lst = new List<PositionableElement>();
			lst.Add(new PositionableElement(1, 1));
			lst.Add(new PositionableElement(2, 2));
			lst.Add(new PositionableElement(3, 1));

			Assert.AreEqual(new OpenTK.Vector2(8, 2), layoutEngine.Layout(lst, new OpenTK.Vector2(8, 2)), "Invalid resize");

			PositionTestHelper(lst, new Vector2(2, 0), new Vector2(3, 0), new Vector2(5, 0));
		}

		[Test]
		public void SinglelineElementsFitThreeLines()
		{
			var layoutEngine = new LeftToRightLayout() { Singleline = true };
			List<PositionableElement> lst = new List<PositionableElement>();
			lst.Add(new PositionableElement(1, 1));
			lst.Add(new PositionableElement(2, 2));
			lst.Add(new PositionableElement(3, 1));

			Assert.AreEqual(new OpenTK.Vector2(6, 2), layoutEngine.Layout(lst, new OpenTK.Vector2(2, 2)), "Invalid resize");

			PositionTestHelper(lst, new Vector2(0, 0), new Vector2(1, 0), new Vector2(3, 0));
		}
		#endregion

		private static void PositionTestHelper(List<PositionableElement> elements, params Vector2[] positions)
		{
			for (int i = 0; i < elements.Count; i++)
			{
				Assert.AreEqual(positions[i], elements[i].Position, "Invalid position at: {0}", i);
			}
		}
	}
}
