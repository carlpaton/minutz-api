using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Interface.Repositories.Feature.Meeting;
using Interface.Repositories.Feature.Meeting.Agenda;
using Interface.Repositories.Feature.Meeting.Note;
using Interface.Services;
using Interface.Services.Feature.Meeting.Note;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Minutz.Models.ViewModels;
using Newtonsoft.Json;

namespace Api.Controllers.Feature.Reports
{
    public class MinutesController : Controller
    {
        private readonly IApplicationSetting _applicationSetting;
        private readonly IGetMeetingRepository _getMeetingRepository;
        private readonly IMinutzAgendaRepository _agendaRepository;
        private readonly IMinutzAgendaRepository _minutzAgendaRepository;
        private readonly IMinutzNoteRepository _noteRepository;
        private readonly IMinutzAttendeeRepository _minutzAttendeeRepository;
        
        public MinutesController(IApplicationSetting applicationSetting,
                                 IGetMeetingRepository getMeetingRepository,
                                 IMinutzAgendaRepository agendaRepository,
            IMinutzNoteRepository noteRepository,
            IMinutzAttendeeRepository attendeeRepository)
        {
            _applicationSetting = applicationSetting;
            _getMeetingRepository = getMeetingRepository;
            _agendaRepository = agendaRepository;
            _noteRepository = noteRepository;
        }

        public ActionResult Invoice(Guid m, string i)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, i, _applicationSetting.GetInstancePassword(i));
            var masterConnectionString = _applicationSetting.CreateConnectionString();
            
            var meetingRequest = _getMeetingRepository.Get(m, i, instanceConnectionString);
            

            var meeting = !meetingRequest.Condition ? new Minutz.Models.Entities.Meeting() : meetingRequest.Meeting;
            

            if (string.IsNullOrEmpty(meeting.Outcome)) meeting.Outcome = "Outcome not set.";
            if (string.IsNullOrEmpty(meeting.Location)) meeting.Location = "Location not set.";
            if (string.IsNullOrEmpty(meeting.Purpose)) meeting.Purpose = "Purpose not set.";
            if (string.IsNullOrEmpty(meeting.Time)) meeting.Time = "0:00";

            
            var agendaCollection = _agendaRepository.GetMeetingAgendaCollection(m, i, instanceConnectionString);
            var agendaItems = new List<JsReportAgenda>();
            if (agendaCollection.Condition)
            {
                foreach (var meetingAgenda in agendaCollection.AgendaCollection)
                {
                    agendaItems.Add(new JsReportAgenda
                    {
                        agendaHeading = meetingAgenda.AgendaHeading,
                        agendaText = meetingAgenda.AgendaText.Replace("<p>","").Replace("</p>","")
                            .Replace("<b>","").Replace("</b>","").Replace("&nbsp;","&")
                    });
                }
            }
            var noteRequest = _noteRepository.GetNoteCollection(m, i, instanceConnectionString);
            var notes = new List<JsReportNote>();
            if (noteRequest.Condition)
            {
                foreach (var meetingNote in noteRequest.NoteCollection)
                {
                    notes.Add(new JsReportNote
                    {
                        noteText =  meetingNote.NoteText.Replace("<p>","").Replace("</p>","")
                            .Replace("<b>","").Replace("</b>","").Replace("&nbsp;","&")
                    });
                }
            }

            var attendeeResult = _minutzAttendeeRepository.GetAttendees(m, i, instanceConnectionString, masterConnectionString);
            var attendees = new List<JsReportAttendee>();
            if (attendeeResult.Condition)
            {
                foreach (var attendee in attendeeResult.Attendees)
                {
                    attendees.Add(new JsReportAttendee
                    {
                        name =  attendee.Name,
                        role = attendee.Role
                    });
                }
            }


            var client = new HttpClient();
            var url = "https://minutz.jsreportonline.net/api/report";
           
            var data = new JsReportObject
                       {
                           name = meeting.Name,
                           location = meeting.Location,
                           time = meeting.Time,
                           outcome = meeting.Outcome.Replace("<p>","").Replace("</p>","")
                               .Replace("<b>","").Replace("</b>","").Replace("&nbsp;","&"),
                           purpose = meeting.Purpose.Replace("<p>","").Replace("</p>","")
                               .Replace("<b>","").Replace("</b>","").Replace("&nbsp;","&"),
                           agenda = agendaItems,
                           notes = notes,
                           attendees = attendees
                       };
            
            var model = new SendData
                        {
                            options = new options{timeout = 60000},
                            data = data,
                            template = new template{shortid = "SJKYFyoYM"}
                        };
            var token = CreateAuthHeader();
            var payload = model.ToStringContent();
            client.DefaultRequestHeaders.Add ("Authorization", token);
            var reportRequest = client.PostAsync(url, payload).Result;
            var report = reportRequest.Content.ReadAsByteArrayAsync().Result;
            return new FileContentResult(report, "application/pdf");
        }
        
       
        private string CreateAuthHeader()
        {
            var username = "leeroya@gmail.com";
            var password = "@nathan01";
            var hash = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password)));
            return $"Basic {hash}";
        }

        public class SendData
        {
            public template template { get; set; }
            public JsReportObject data { get; set; }
            public options options { get; set; }
        }
        
        public class options
        {
            public int timeout { get; set; }
        }
        
        public class template
        {
            public string shortid { get; set; }
            
        }

        public dynamic BuildPayload()
        {
            var agenda = new List<MeetingAgenda>();
            agenda.Add(new MeetingAgenda(){AgendaHeading = "Agenda Item",AgendaText = "Some text"});
            var meeting = new MeetingViewModel
                          {
                              Name = "Demo",
                              Location = "Durban",
                              Date = DateTime.UtcNow,
                              Time = "1:00",
                              Purpose = "Test",
                              Outcome = "report",
                              Agenda = agenda
                          };

            var json = JsonConvert.SerializeObject(meeting);
            var dyn = JsonConvert.DeserializeObject<dynamic>(json);
            return dyn;
        }
    }

    public class JsReportObject
    {
        public string name { get; set; }
        public string location { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string purpose { get; set; }
        public string outcome { get; set; }
        public List<JsReportAgenda> agenda { get; set; }
        public List<JsReportAttendee> attendees { get; set; }
        public List<JsReportNote> notes { get; set; }
    }

    public class JsReportAgenda
    {
        public string agendaHeading { get; set; }
        public string agendaText { get; set; }
    }
    
    public class JsReportAttendee
    {
        public string name { get; set; }
        public string role { get; set; }
    }

    public class JsReportNote
    {
        public string noteText { get; set; }
    }
    
    public static class Extentions 
    {
        public static StringContent ToStringContent(this MinutesController.SendData payload)
        {
            return new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        }
    }
}

