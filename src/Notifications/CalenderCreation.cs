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

			var e = new CalendarEvent
			{
				Name = meetingName,
				//Location = location,
				Start = new CalDateTime(now),
				End = new CalDateTime(later),
				RecurrenceRules = new List<RecurrencePattern> { rrule },
				Description = "This is some info in the description",
				Duration = TimeSpan.FromHours(2),
				//Organizer = new Organizer("minutz user"),
				//GeographicLocation = new GeographicLocation("durban")
				
			};
			var attendee = new Attendee
			{
				CommonName = toUserName,
				Rsvp = true,
				Value = new Uri($"mailto:{toUser}")
			};
			//e.Attendees = new List<Attendee> {attendee};

			var calendar = new Calendar();
			
			calendar.Events.Add(e);

			var serializer = new CalendarSerializer();
			
			var serializedCalendar = serializer.SerializeToString(calendar);
			var bytesCalendar = System.Text.Encoding.Default.GetBytes(serializedCalendar);
			var ics = Convert.ToBase64String(bytesCalendar);
			//FileStream ms = new FileStream(serializedCalendar);
			
			message.AddAttachment("minutz.ics",ics,"text/calender");
			//System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, "minutz.ics", "text/calendar");
			return message;
		}
	}
}