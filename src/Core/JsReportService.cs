using System;
using System.Collections.Generic;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;

namespace Core
{
  public class JsReportService : IReportService
  {
    private readonly IReportRepository _reportRepository;

    public JsReportService(IReportRepository reportRepository)
    {
      _reportRepository = reportRepository;
    }


    public (bool condition, string message, byte[] file) CreateMinutes
      (Meeting meetingItem, IEnumerable<MeetingAttendee> attendeescollection, IEnumerable<MeetingAgenda> agendaCollection, IEnumerable<MeetingNote> notecollection)
    {
      var agenditems = new List<dynamic>();
      var noteItems = new List<dynamic>();
      var people = new List<dynamic>();
      foreach (var agenda in agendaCollection)
      {
        agenditems.Add(new {agendaHeading = agenda.AgendaHeading ,agendaText = agenda.AgendaText });
      }
        
      foreach (var note in notecollection)
      {
        noteItems.Add(new {agendaHeading = note.NoteText });
      } 
      
      foreach (var attendee in attendeescollection)
      {
        noteItems.Add(new {name = attendee.Name, role = attendee.Role });
      } 
         
      var meeting = new
      {
        name = meetingItem.Name,
        date = meetingItem.Date,
        location = meetingItem.Location,
        time = meetingItem.Time,
        purpose = meetingItem.Purpose,
        outcome = meetingItem.Outcome,
        agenda = agenditems,
        attendees = people,
        notes = noteItems
      };
      return _reportRepository.CreateMinutesReport(meeting);
    }
  }
}