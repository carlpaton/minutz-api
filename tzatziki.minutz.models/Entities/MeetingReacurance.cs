using System;

namespace tzatziki.minutz.models.Entities
{
	public class MeetingReacurance
	{
		public string Type { get; set; }
		public int Number { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int OccuranceNumber { get; set; }
		public int WeekOnWeekDay { get; set; }
		public string MonthOnWeekDayNumber { get; set; }
		public string MonthOnDay { get; set; }
	}
}
