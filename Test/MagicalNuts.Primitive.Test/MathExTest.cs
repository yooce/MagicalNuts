using System;
using Xunit;

namespace MagicalNuts.Primitive.Test
{
	public class MathExTest
	{
		[Fact]
		public void CeilingTest()
		{
			Assert.Equal(12.33m, MathEx.Ceiling(12.324m, 2));
		}

		[Fact]
		public void FloorTest()
		{
			Assert.Equal(12.32m, MathEx.Floor(12.324m, 2));
		}
	}
}
