using System.Collections.Generic;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models;
using Minutz.Models.Entities;

namespace Core
{
  public class JsReportService : IReportService
  {
    private readonly IReportRepository _reportRepository;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IApplicationSetting _applicationSetting;
    private readonly ILogService _logService;

    public JsReportService
      (IReportRepository reportRepository, IMeetingRepository meetingRepository,
      IApplicationSetting applicationSetting, ILogService logService)
    {
      _reportRepository = reportRepository;
      _meetingRepository = meetingRepository;
      _applicationSetting = applicationSetting;
      _logService = logService;
    }


    public (bool condition, string message, byte[] file) CreateMinutes
      (Meeting meetingItem, IEnumerable<MeetingAttendee> attendeescollection,
      IEnumerable<MeetingAgenda> agendaCollection, IEnumerable<MeetingNote> notecollection,
      AuthRestModel user)
    {
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
      
      var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
      
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
      var jsReportResult = _reportRepository.CreateMinutesReport(meeting);
      if (jsReportResult.condition)
      {
        var createMinutesResult = _meetingRepository.CreateUpdateMeetingMinutes
          (user.InstanceId, instanceConnectionString, meetingItem.Id.ToString(), jsReportResult.file);
        if (!createMinutesResult.condition)
        {
          _logService.Log(LogLevel.Error, $"JsReportService - Line:73 - _meetingRepository.CreateUpdateMeetingMinutes{ createMinutesResult.message}");
        }
      }

      return jsReportResult;
    }
  }
}