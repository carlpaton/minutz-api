using System;
using System.Collections.Generic;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace Notifications
{
	public class CalenderCreation
	{
		public void Reacurring()
		{
			var now = DateTime.Now;
			var later = now.AddHours(1);

			//Repeat daily for 5 days
			var rrule = new RecurrencePattern(FrequencyType.Daily, 1) { Count = 5 };

			var e = new CalendarEvent
			{
				Start = new CalDateTime(now),
				End = new CalDateTime(later),
				RecurrenceRules = new List<RecurrencePattern> { rrule },
			};

			var calendar = new Calendar();
			calendar.Events.Add(e);

			var serializer = new CalendarSerializer();
			var serializedCalendar = serializer.SerializeToString(calendar);
		}
	}
}