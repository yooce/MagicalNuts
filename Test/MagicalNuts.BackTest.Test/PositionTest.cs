using System;
using Xunit;

namespace MagicalNuts.BackTest.Test
{
	public class PositionTest
	{
		[Fact]
		public void ExitTest()
		{
			Position position = new Position(null, PositionDirection.Long, 2, null, new DateTime(2021, 11, 11), 100, 200, 10);
			position.Exit(new DateTime(2021, 11, 12), 200, 400, 20, 1);
			Assert.Equal(170, position.Return);
			Assert.Equal(0.85m, position.ReturnRate);

			position = new Position(null, PositionDirection.Short, 2, null, new DateTime(2021, 11, 11), 100, 200, 10);
			position.Exit(new DateTime(2021, 11, 12), 50, 100, 20, 1);
			Assert.Equal(70, position.Return);
			Assert.Equal(0.35m, position.ReturnRate);
		}
	}
}