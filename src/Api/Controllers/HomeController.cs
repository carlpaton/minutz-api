using System;
using System.Collections.Generic;
using System.Text;
using Api.Models;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class HomeController : Controller
    {        
        public IActionResult Index()
        {
            return View();
        }

        
        
        public ActionResult iCalendar()
        {
            var now = DateTime.Now;
            var later = now.AddHours(1);

//Repeat daily for 5 days
            var rrule = new RecurrencePattern(FrequencyType.Daily, 1) { Count = 5 };
            var organizer = new Organizer("Minutz");
            organizer.CommonName = "Minutz";
            organizer.SentBy =  new Uri("mailto:invite@minutz.net");
			
            var attendee = new Attendee
            {
                CommonName = "Lee-Roy Ashworth",
                Rsvp = true,
                Value = new Uri("mailto:leeroya@gmail.com")
            };
            var e = new CalendarEvent
            {
                Summary = "Summery Text",
                Location = "Durban",
                Start = new CalDateTime(now),
                End = new CalDateTime(later),
                RecurrenceRules = new List<RecurrencePattern> { rrule },
                Description = "This is some info in the description",
                Duration = TimeSpan.FromHours(1),
                Attendees = new List<Attendee>(){ attendee}
                //Organizer = organizer
                //GeographicLocation = new GeographicLocation("durban")
				
            };
            var calendar = new Calendar();
            calendar.Events.Add(e);

            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);

           // var serializer = factory.Build(iCal.GetType(), ctx) as IStringSerializer;

            //var output = serializer.SerializeToString(iCal);
             
            var bytes = Encoding.UTF8.GetBytes(serializedCalendar);

            return this.File(bytes, "text/calendar", "meeting.ics");
           
        }
    }
}