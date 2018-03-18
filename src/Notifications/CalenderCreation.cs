using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using SendGrid.Helpers.Mail;

namespace Notifications
{
	public static class CalenderCreation
	{
		public static SendGridMessage CreateCalenderEvent(
			this SendGridMessage message,
			string toUser,
			string toUserName,
			string location,
			string meetingName)
		{
			var now = DateTime.UtcNow;
			var later = now.AddHours(2);

			//Repeat daily for 5 days
			var rrule = new RecurrencePattern(FrequencyType.Daily, 1) { Count = 1 };
			var attendee = new Attendee
			{
				CommonName = toUserName,
				Rsvp = true,
				Value = new Uri($"mailto:{toUser}")
			};
			
			var e = new CalendarEvent
			{
				Summary = meetingName,
				Location = location?? "Durban",
				Start = new CalDateTime(now),
				End = new CalDateTime(later),
				RecurrenceRules = new List<RecurrencePattern> { rrule },
				Description = "This is some info in the description",
				Attendees = new List<Attendee>(){ attendee},
				Duration = TimeSpan.FromHours(1),
				//Organizer = organizer
				//GeographicLocation = new GeographicLocation("durban")
				
			};
			var calendar = new Calendar();
			calendar.Events.Add(e);

			var serializer = new CalendarSerializer();
			
			var serializedCalendar = serializer.SerializeToString(calendar);
			var bytesCalendar = System.Text.Encoding.Default.GetBytes(serializedCalendar);
			var ics = Convert.ToBase64String(bytesCalendar);
			message.AddAttachment("minutz.ics",ics,"text/calender");
			return message;
		}
	}
}