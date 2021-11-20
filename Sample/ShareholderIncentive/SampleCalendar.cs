using MagicalNuts.Primitive;
using System.Runtime.Versioning;

namespace ShareholderIncentiveSample
{
	[SupportedOSPlatform("windows")]
	public class SampleCalendar : Calendar
	{
		private readonly string[] datestrs =
		{
			"2020-01-01", "2020-01-02", "2020-01-03", "2020-01-13", "2020-02-11", "2020-02-24", "2020-03-20","2020-04-29", "2020-05-04", "2020-05-05",
			"2020-05-06", "2020-07-23", "2020-07-24", "2020-08-10", "2020-09-21", "2020-09-22", "2020-11-03", "2020-11-23", "2020-12-31"
		};

		private List<DateTime> Holidays = null;

		public override async Task<bool> SetUpAsync()
		{
			Holidays = new List<DateTime>();
			foreach (string datestr in datestrs)
			{
				Holidays.Add(DateTime.Parse(datestr));
			}
			return true;
		}

		public override bool IsHoliday(DateTime dt)
		{
			return dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday
				|| Holidays.Where(holiday => holiday.Date == dt).Count() > 0;
		}
	}
}
