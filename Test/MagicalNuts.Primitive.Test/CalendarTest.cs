using System;
using Xunit;

namespace MagicalNuts.Primitive.Test
{
	public class CalendarTest
	{
		[Fact]
		public void GetBusinessDayBeforeTest()
		{
			Calendar calendar = new Calendar();
			Assert.Equal(new DateTime(2021, 10, 14), calendar.GetBusinessDayBefore(new DateTime(2021, 10, 18), 2));
		}

		[Fact]
		public void GetBusinessDayAfterTest()
		{
			Calendar calendar = new Calendar();
			Assert.Equal(new DateTime(2021, 10, 18), calendar.GetBusinessDayAfter(new DateTime(2021, 10, 14), 2));
		}

		[Fact]
		public void GetFirstBusinessDayToBeforeTest()
		{
			Calendar calendar = new Calendar();
			Assert.Equal(new DateTime(2021, 10, 15), calendar.GetFirstBusinessDayToBefore(new DateTime(2021, 10, 17)));
		}

		[Fact]
		public void GetFirstBusinessDayToAfterTest()
		{
			Calendar calendar = new Calendar();
			Assert.Equal(new DateTime(2021, 10, 18), calendar.GetFirstBusinessDayToAfter(new DateTime(2021, 10, 18)));
		}
	}
}
