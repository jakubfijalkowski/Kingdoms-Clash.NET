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
		private LeftToRightLayout Layouter;

		[SetUp]
		public void SetUp()
		{
			this.Layouter = new LeftToRightLayout();
		}

		[Test]
		public void AllElementsFitOneLine()
		{
			List<PositionableElement> lst = new List<PositionableElement>();
			lst.Add(new PositionableElement(1, 1));
			lst.Add(new PositionableElement(2, 1));
			lst.Add(new PositionableElement(3, 1));

			Assert.AreEqual(new OpenTK.Vector2(8, 2), this.Layouter.Layout(lst, new OpenTK.Vector2(8, 2)), "Invalid resize");

			PositionTestHelper(lst, new Vector2(0, 0), new Vector2(1, 0), new Vector2(3, 0));
		}

		[Test]
		public void AllElementsFitThreeLines()
		{
			List<PositionableElement> lst = new List<PositionableElement>();
			lst.Add(new PositionableElement(1, 1));
			lst.Add(new PositionableElement(2, 1));
			lst.Add(new PositionableElement(3, 1));
			lst.Add(new PositionableElement(2, 1));

			Assert.AreEqual(new OpenTK.Vector2(3, 3), this.Layouter.Layout(lst, new OpenTK.Vector2(3, 1)), "Invalid resize");

			PositionTestHelper(lst, new Vector2(0, 0), new Vector2(1, 0),
				new Vector2(0, 1),
				new Vector2(0, 2));
		}

		[Test]
		public void ElementsDontFitAnyLine()
		{
			List<PositionableElement> lst = new List<PositionableElement>();
			lst.Add(new PositionableElement(3, 1));
			lst.Add(new PositionableElement(5, 1));

			Assert.AreEqual(new OpenTK.Vector2(5, 2), this.Layouter.Layout(lst, new OpenTK.Vector2(1, 1)), "Invalid resize");

			PositionTestHelper(lst, new Vector2(0, 0), new Vector2(0, 1));
		}

		private static void PositionTestHelper(List<PositionableElement> elements, params Vector2[] positions)
		{
			for (int i = 0; i < elements.Count; i++)
			{
				Assert.AreEqual(elements[i].Position, positions[i], "Invalid position at: {0}", i);
			}
		}
	}
}
