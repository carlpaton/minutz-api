using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Helper;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models;
using Minutz.Models.Entities;

namespace Core
{
  public class MeetingService : IMeetingService
  {
    private readonly IMeetingRepository _meetingRepository;
    private readonly IMeetingAgendaRepository _meetingAgendaRepository;
    private readonly IMeetingAttendeeRepository _meetingAttendeeRepository;
    private readonly IMeetingActionRepository _meetingActionRepository;
    private readonly IMeetingAttachmentRepository _meetingAttachmentRepository;
    private readonly IMeetingNoteRepository _meetingNoteRepository;
    private readonly ILogService _logService;
    private readonly IReportRepository _reportRepository;
    private readonly IMeetingActionService _actionService;
    private readonly IMeetingDecisionService _decisionService;
    private readonly IApplicationSetting _applicationSetting;
    private readonly IDecisionRepository _decisionRepository;

    public MeetingService
    (IMeetingRepository meetingRepository,IMeetingAgendaRepository meetingAgendaRepository,
      IMeetingAttendeeRepository meetingAttendeeRepository,IMeetingActionRepository meetingActionRepository,
      IApplicationSetting applicationSetting,IMeetingAttachmentRepository meetingAttachmentRepository,
      IMeetingNoteRepository meetingNoteRepository,IDecisionRepository decisionRepository,
      ILogService logService, IReportRepository reportRepository,
      IMeetingActionService actionService, IMeetingDecisionService decisionService)
    {
      _meetingRepository = meetingRepository;
      _meetingAgendaRepository = meetingAgendaRepository;
      _meetingAttendeeRepository = meetingAttendeeRepository;
      _meetingActionRepository = meetingActionRepository;
      _meetingAttachmentRepository = meetingAttachmentRepository;
      _meetingNoteRepository = meetingNoteRepository;
      _logService = logService;
      _reportRepository = reportRepository;
      _actionService = actionService;
      _decisionService = decisionService;
      _applicationSetting = applicationSetting;
      _decisionRepository = decisionRepository;
    }


    public MeetingAgenda CreateMeetingAgendaItem
      (MeetingAgenda agenda, AuthRestModel user)
    {
      if (agenda == null) throw new ArgumentNullException(nameof(agenda), "Please provide a agenda model");
      if (string.IsNullOrEmpty(agenda.ReferenceId)) throw new ArgumentNullException(nameof(agenda.ReferenceId), "Please provide a meeting id for the agenda item.");

      var connectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server, _applicationSetting.Catalogue,
        user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
      
      var result = _meetingAgendaRepository.Add(agenda, user.InstanceId, connectionString);
      return result ? agenda : new MeetingAgenda();
    }

    public List<MeetingAgenda> UpdateMeetingAgendaItems
      (string meetingId, List<MeetingAgenda> data, AuthRestModel user)
    {
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
          
//      var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
//        _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
      
      if (data.Any())
      {
        var meetingAgendaItems = _meetingAgendaRepository.GetMeetingAgenda(
          Guid.Parse(data.FirstOrDefault()?.ReferenceId), user.InstanceId, instanceConnectionString);
        foreach (var updateAgendaItem in data)
        {
          if (updateAgendaItem.Id == Guid.Empty)
          {
            updateAgendaItem.Id = Guid.NewGuid();
          }
          var q = meetingAgendaItems.FirstOrDefault(i => i.Id == updateAgendaItem.Id);
          if (q != null)
          {
            if (updateAgendaItem.ReferenceId == null)
            {
              updateAgendaItem.ReferenceId = meetingId;
            }
            _meetingAgendaRepository.Update(updateAgendaItem, user.InstanceId, instanceConnectionString);
          }
          else
          {
            _meetingAgendaRepository.Add(updateAgendaItem, user.InstanceId, instanceConnectionString);
          }
        }
        return _meetingAgendaRepository.GetMeetingAgenda(
          Guid.Parse(data.FirstOrDefault()?.ReferenceId), user.InstanceId, instanceConnectionString);
      }
      foreach (var agendaitem in data)
      {
        _meetingAgendaRepository.Add(agendaitem, user.InstanceId, instanceConnectionString);
      }
      return _meetingAgendaRepository.GetMeetingAgenda(
        Guid.Parse(data.FirstOrDefault()?.ReferenceId), user.InstanceId, instanceConnectionString);
    }

    public List<MeetingAttendee> UpdateMeetingAttendees
      (List<MeetingAttendee> data, AuthRestModel user)
    {
      var userConnectionString = _applicationSetting.CreateConnectionString(
        _applicationSetting.Server, _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
      
      var masterConnectionString =  _applicationSetting.CreateConnectionString(
          _applicationSetting.Server, _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);      
      if (data.Any())
      {
        var attendees = _meetingAttendeeRepository.GetMeetingAttendees
          (data.First().ReferenceId, user.InstanceId,userConnectionString, masterConnectionString);
        foreach (var attendee in data)
        {
          var q = attendees.FirstOrDefault(i => i.Id == attendee.Id);
          if (q != null)
          {
            _meetingAttendeeRepository.Update(attendee, user.InstanceId, userConnectionString, masterConnectionString);
          }
          else
          {
            _meetingAttendeeRepository.Add(attendee, user.InstanceId, userConnectionString);
          }
        }
        return _meetingAttendeeRepository.GetMeetingAttendees(
          data.First().ReferenceId,user.InstanceId,userConnectionString, masterConnectionString);
      }
      foreach (var newAttendee in data)
      {
        _meetingAttendeeRepository.Add(newAttendee, user.InstanceId, userConnectionString);
      }
      return _meetingAttendeeRepository.GetMeetingAttendees(
        data.First().ReferenceId, user.InstanceId,  userConnectionString, masterConnectionString);
    }

    public Minutz.Models.ViewModels.MeetingViewModel GetMeeting
      (AuthRestModel user, string id)
    {
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
          
      var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
      
      var meeting = _meetingRepository.Get(Guid.Parse(id), user.InstanceId, instanceConnectionString);
      var availibleAttendees = _meetingAttendeeRepository.GetAvalibleAttendees(user.InstanceId, instanceConnectionString, masterConnectionString);
      if (availibleAttendees.All(i => i.Email != user.Email))
      {
        var userAttendee = new MeetingAttendee
        {
          Id = Guid.NewGuid(),
          Email = user.Email,
          Name = user.Name,PersonIdentity = user.Sub,
          Picture = user.Picture,
          ReferenceId = meeting.Id,
          Role = user.Role,
          Status = "availible"
        };
        _meetingAttendeeRepository.AddAvailibleAttendee(userAttendee, user.InstanceId, instanceConnectionString);
        availibleAttendees = _meetingAttendeeRepository.GetAvalibleAttendees(user.InstanceId, instanceConnectionString, masterConnectionString);
      }

      var agendaItems =
        _meetingAgendaRepository.GetMeetingAgenda(meeting.Id, user.InstanceId, instanceConnectionString);
      var attachments =
        _meetingAttachmentRepository.GetMeetingAttachments(meeting.Id, user.InstanceId, instanceConnectionString);
      var attendees = _meetingAttendeeRepository.GetMeetingAttendees(meeting.Id, user.InstanceId,
        instanceConnectionString, masterConnectionString);
      var notes = _meetingNoteRepository.GetMeetingNotes(meeting.Id, user.InstanceId, instanceConnectionString);
      var actions = _meetingActionRepository.GetMeetingActions(meeting.Id, user.InstanceId, instanceConnectionString);
      var descions = _decisionRepository.GetMeetingDecisions(meeting.Id, user.InstanceId, instanceConnectionString);
      
      var meetingViewModel = new Minutz.Models.ViewModels.MeetingViewModel
      {
        Id = meeting.Id.ToString(),
        Name = meeting.Name,
        Date = meeting.Date,
        Duration = meeting.Duration,
        IsFormal = meeting.IsFormal,
        IsLocked = meeting.IsLocked,
        IsPrivate = meeting.IsPrivate,
        IsReacurance = meeting.IsReacurance,
        MeetingOwnerId = meeting.MeetingOwnerId,
        Outcome = meeting.Outcome,
        Location = meeting.Location,
        Status = meeting.Status,
        Purpose = meeting.Purpose,
        ReacuranceType = int.Parse(meeting.ReacuranceType),
        Tag = meeting.Tag.Split(',').ToList(),
        Time = meeting.Time,
        TimeZone = meeting.TimeZone,
        UpdatedDate = DateTime.UtcNow,
        AvailableAttendeeCollection = availibleAttendees,
        MeetingAgendaCollection = agendaItems
          ,
        MeetingAttachmentCollection = attachments
          ,
        MeetingAttendeeCollection = attendees
          ,
        MeetingNoteCollection = notes
         ,
        MeetingActionCollection = actions
          ,
        MeetingDecisionCollection = descions
          
      };
      

      return meetingViewModel;
    }

    public (bool condition, int statusCode, string message, IEnumerable<Minutz.Models.ViewModels.MeetingViewModel> value) GetMeetings
      (AuthRestModel user, string referenceKey)
    {
      (string key, string reference) referenceItems = (string.Empty, string.Empty);
      if (!string.IsNullOrEmpty(referenceKey))
      {
        referenceItems = referenceKey.TupleSplit();
      }

      try
      {
        if (user.InstanceId == null)
          return (true, 404, "There are no meetings.", new List<Minutz.Models.ViewModels.MeetingViewModel>());

        var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
          _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
          
        var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
          _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
        
        var result = new List<Minutz.Models.ViewModels.MeetingViewModel>();
        if (user.Role == AuthenticationHelper.Guest)
        {
          if (string.IsNullOrEmpty(user.Related))
          {
            return (true, 200, "Success", new List<Minutz.Models.ViewModels.MeetingViewModel>());
          }
          var relatedTuple = user.Related.TupleSplit();
          var relatedItems = relatedTuple.value.SplitToList("&", ";");
          var meetingIds = new List<string>();
          foreach (var relatedItem in relatedItems)
          {
            meetingIds.Add(relatedItem.value);
          }
          
          var filtered = _meetingRepository.List(user.InstanceId, instanceConnectionString, meetingIds);
         
          foreach (var meeting in filtered)
          {
            var availibleAttenddees =
              _meetingAttendeeRepository.GetAvalibleAttendees(user.InstanceId, instanceConnectionString,
                masterConnectionString);
            var agendaItems =
              _meetingAgendaRepository.GetMeetingAgenda(meeting.Id, user.InstanceId, instanceConnectionString);
            var attachements =
              _meetingAttachmentRepository.GetMeetingAttachments(meeting.Id, user.InstanceId, instanceConnectionString);
            var attendees = _meetingAttendeeRepository.GetMeetingAttendees(meeting.Id, user.InstanceId,
              instanceConnectionString, masterConnectionString);
            var notes = _meetingNoteRepository.GetMeetingNotes(meeting.Id, user.InstanceId, instanceConnectionString);
            
            var meetingViewModel = new Minutz.Models.ViewModels.MeetingViewModel
            {
              Id = meeting.Id.ToString(),
              Name = meeting.Name,
              Date = meeting.Date,
              Duration = meeting.Duration,
              IsFormal = meeting.IsFormal,
              IsLocked = meeting.IsLocked,
              IsPrivate = meeting.IsPrivate,
              IsReacurance = meeting.IsReacurance,
              MeetingOwnerId = meeting.MeetingOwnerId,
              Outcome = meeting.Outcome,
              Purpose = meeting.Purpose,
              Status = meeting.Status,
              ReacuranceType = int.Parse(meeting.ReacuranceType),
              Tag = meeting.Tag.Split(',').ToList(),
              Time = meeting.Time,
              TimeZone = meeting.TimeZone,
              UpdatedDate = DateTime.UtcNow,
              AvailableAttendeeCollection = availibleAttenddees
               ,
              MeetingAgendaCollection = agendaItems
                ,
              MeetingAttachmentCollection = attachements
                ,
              MeetingAttendeeCollection = attendees
                ,
              MeetingNoteCollection = notes
                
            };

            if (meetingViewModel.AvailableAttendeeCollection.All(i => i.Email != user.Email))
            {
              meetingViewModel.AvailableAttendeeCollection.Add(new MeetingAttendee
              {
                Email = user.Email,
                Name = user.Name,PersonIdentity = user.Sub,
                Picture = user.Picture,
                ReferenceId = Guid.Parse(meetingViewModel.Id),
                Role = user.Role,
                Status = "Availible"
              });
            }
            result.Add(meetingViewModel);
          }
          return (true, 200, "Success", result);
        }
        
        var meetings = _meetingRepository.List(user.InstanceId, instanceConnectionString);
        foreach (var meeting in meetings)
        {
          var avalibleAttendees =
            _meetingAttendeeRepository.GetAvalibleAttendees(user.InstanceId, instanceConnectionString,
              masterConnectionString);
          
          var agendaItems =
            _meetingAgendaRepository.GetMeetingAgenda(meeting.Id, user.InstanceId, instanceConnectionString);
          
          var attachments =
            _meetingAttachmentRepository.GetMeetingAttachments(meeting.Id, user.InstanceId, instanceConnectionString);
          
          var attendees = _meetingAttendeeRepository.GetMeetingAttendees(meeting.Id, user.InstanceId,
            instanceConnectionString, masterConnectionString);
          
          var notes = _meetingNoteRepository.GetMeetingNotes(meeting.Id, user.InstanceId, instanceConnectionString);

          var actions =
            _meetingActionRepository.GetMeetingActions(meeting.Id, user.InstanceId, instanceConnectionString);
          
          
          
          var meetingViewModel = new Minutz.Models.ViewModels.MeetingViewModel
          {
            Id = meeting.Id.ToString(),
            Name = meeting.Name,
            Date = meeting.Date,
            Duration = meeting.Duration,
            IsFormal = meeting.IsFormal,
            IsLocked = meeting.IsLocked,
            IsPrivate = meeting.IsPrivate,
            IsReacurance = meeting.IsReacurance,
            MeetingOwnerId = meeting.MeetingOwnerId,
            Outcome = meeting.Outcome,
            Purpose = meeting.Purpose,
            Status = meeting.Status,
            ReacuranceType = int.Parse(meeting.ReacuranceType),
            Tag = meeting.Tag.Split(',').ToList(),
            Time = meeting.Time,
            TimeZone = meeting.TimeZone,
            UpdatedDate = DateTime.UtcNow,
            AvailableAttendeeCollection = avalibleAttendees,
            MeetingAgendaCollection = agendaItems,
            MeetingAttachmentCollection = attachments,
            MeetingAttendeeCollection = attendees,
            MeetingNoteCollection = notes
          };

          result.Add(meetingViewModel);
        }
        return (true, 200, "Success", result);
      }
      catch (Exception ex)
      {
        return (false, 500, ex.Message, null);
      }
    }

    public KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel> CreateMeeting
    (AuthRestModel user, Meeting meeting, List<MeetingAttendee> attendees, List<MeetingAgenda> agenda, List<MeetingNote> notes, List<MeetingAttachment> attachements,
      List<MinutzAction> actions, string instanceId = "")
    {
      if (string.IsNullOrEmpty(instanceId))
      {
        if (string.IsNullOrEmpty(user.Related))
        {
          instanceId = user.InstanceId;
        }
        else
        {
          List<(string instanceId, string meetingId)> relatedInstances =
            user.Related.SplitToList (StringDeviders.InstanceStringDevider, StringDeviders.MeetingStringDevider);
          instanceId = relatedInstances.First().instanceId;
        }
      }

      _logService.Log(LogLevel.Info, "CreateMeeting - Service - entry point {ID}");
      
      if (!string.IsNullOrEmpty(user.InstanceId))
      {
        _logService.Log(LogLevel.Info, "CreateMeeting - Service - auth ");
        
        meeting.Name = " demo";
        meeting.MeetingOwnerId = user.Sub;
        if (string.IsNullOrEmpty(meeting.Status))
        {
          meeting.Status = "create";
        }

        _logService.Log(LogLevel.Info, "CreateMeeting - Service - auth ");
        
        var masterConnectionString = _applicationSetting.CreateConnectionString
          (_applicationSetting.Server, _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
        
        var instanceConnectionString = _applicationSetting.CreateConnectionString
        (_applicationSetting.Server, _applicationSetting.Catalogue, instanceId,
          _applicationSetting.GetInstancePassword(instanceId));
        
        var availibleAttendees = _meetingAttendeeRepository.GetAvalibleAttendees(instanceId, instanceConnectionString,masterConnectionString);

        if (availibleAttendees.All(i => i.Email != user.Email))
        {
          var userAttendee = new MeetingAttendee
          {
            Email = user.Email,
            Name = user.Name,PersonIdentity = user.Sub,
            Picture = user.Picture,
            ReferenceId = meeting.Id,
            Role = user.Role,
            Status = "availible"
          };
          _meetingAttendeeRepository.AddAvailibleAttendee(userAttendee, user.InstanceId, instanceConnectionString);
          availibleAttendees = _meetingAttendeeRepository.GetAvalibleAttendees(user.InstanceId, instanceConnectionString, masterConnectionString);
        }
        
        attendees.Add(new MeetingAttendee
        {
          Name = user.Name,
          Email = user.Email,
          Id = Guid.NewGuid(),
          PersonIdentity = user.Sub,
          Picture = user.Picture,
          Role = "Meeting Owner",
          Status = "Accepted"
        });
        
        
        _logService.Log(LogLevel.Info, "CreateMeeting - Service - added owner to attendees ");
        _logService.Log(LogLevel.Info, "CreateMeeting - Service - auth ");
        
        var saveMeeting = _meetingRepository.Add
        (meeting, instanceId,  _applicationSetting.CreateConnectionString
          (_applicationSetting.Server,_applicationSetting.Catalogue,instanceId,_applicationSetting.GetInstancePassword(instanceId)));
        
        if (saveMeeting)
        {
          foreach (var agendaItem in agenda)
          {
            agendaItem.Id = Guid.NewGuid();
            agendaItem.ReferenceId = meeting.Id.ToString();
            var saveAgenda = _meetingAgendaRepository.Add(agendaItem, instanceId,  _applicationSetting.CreateConnectionString(
              _applicationSetting.Server,_applicationSetting.Catalogue,instanceId,_applicationSetting.GetInstancePassword(instanceId)));
            if (!saveAgenda)
            {
              return new KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel>(false,
                new Minutz.Models.ViewModels.MeetingViewModel
                {
                  ResultMessage =
                    $"There was a issue creating the meetingViewModel agenda item for meetingViewModel {meeting.Name}."
                });
            }
          }

          foreach (var attendee in attendees)
          {
            attendee.Id = Guid.NewGuid();
            attendee.ReferenceId = meeting.Id;
            var savedAttendee = _meetingAttendeeRepository.Add(attendee, instanceId,  _applicationSetting.CreateConnectionString(
              _applicationSetting.Server,_applicationSetting.Catalogue,instanceId,_applicationSetting.GetInstancePassword(instanceId)));
            if (!savedAttendee)
            {
              return new KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel>(false,
                new Minutz.Models.ViewModels.MeetingViewModel
                {
                  ResultMessage = $"There was a issue creating the meetingViewModel attendee for meetingViewModel {meeting.Name}"
                });
            }
          }

          foreach (var attachment in attachements)
          {
            attachment.Id = Guid.NewGuid();
            attachment.ReferanceId = meeting.Id;
            var savedAttachment = _meetingAttachmentRepository.Add(attachment, instanceId,  _applicationSetting.CreateConnectionString(
              _applicationSetting.Server,_applicationSetting.Catalogue,instanceId,_applicationSetting.GetInstancePassword(instanceId)));
            if (!savedAttachment)
            {
              return new KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel>(false,
                new Minutz.Models.ViewModels.MeetingViewModel
                {
                  ResultMessage =
                    "There was a issue creating the meetingViewModel attachment for meetingViewModel {meetingViewModel.Name}"
                });
            }
          }

          foreach (var note in notes)
          {
            note.Id = Guid.NewGuid().ToString();
            note.ReferanceId = meeting.Id.ToString();
            var noteSaved = _meetingNoteRepository.Add(note, instanceId,  _applicationSetting.CreateConnectionString(
              _applicationSetting.Server,_applicationSetting.Catalogue,instanceId,_applicationSetting.GetInstancePassword(instanceId)));
            if (!noteSaved)
            {
              return new KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel>(false,
                new Minutz.Models.ViewModels.MeetingViewModel
                {
                  ResultMessage =
                    "There was a issue creating the meetingViewModel note for meetingViewModel {meetingViewModel.Name}"
                });
            }
          }

          foreach (var action in actions)
          {
            action.Id = Guid.NewGuid();
            action.ReferanceId = meeting.Id;
            var actionSaved = _meetingActionRepository.Add(action, instanceId,  _applicationSetting.CreateConnectionString(
              _applicationSetting.Server,_applicationSetting.Catalogue,instanceId,_applicationSetting.GetInstancePassword(instanceId)));
            if (!actionSaved)
            {
              return new KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel>(false,
                new Minutz.Models.ViewModels.MeetingViewModel
                {
                  ResultMessage =
                    "There was a issue creating the meetingViewModel action for meetingViewModel {meetingViewModel.Name}"
                });
            }
          }

          var result = new Minutz.Models.ViewModels.MeetingViewModel
          {
            Id = meeting.Id.ToString(),
            MeetingAgendaCollection = agenda,
            Name = meeting.Name,
            MeetingAttendeeCollection = attendees,
            AvailableAttendeeCollection = availibleAttendees,
            Date = meeting.Date,
            MeetingAttachmentCollection = attachements,
            Duration = meeting.Duration,
            IsFormal = meeting.IsFormal,
            IsLocked = meeting.IsLocked,
            IsPrivate = meeting.IsPrivate,
            IsReacurance = meeting.IsReacurance,
            MeetingNoteCollection = notes,
            Status = meeting.Status,
            MeetingOwnerId = meeting.MeetingOwnerId,
            Outcome = meeting.Outcome,
            Purpose = meeting.Purpose,
            ReacuranceType = int.Parse(meeting.ReacuranceType),
            ResultMessage = "Successfully Created",
            Tag = meeting.Tag.Split(',').ToList(),
            Time = meeting.Time,
            TimeZone = meeting.TimeZone,
            UpdatedDate = DateTime.UtcNow
          };
          if (result.AvailableAttendeeCollection.All(i => i.Email != user.Email))
          {
            result.AvailableAttendeeCollection.Add(new MeetingAttendee
            {
              Email = user.Email,
              Name = user.Name,PersonIdentity = user.Sub,
              Picture = user.Picture,
              ReferenceId = Guid.Parse(result.Id),
              Role = user.Role,
              Status = "Availible"
            });
          }
          
          return new KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel>(true, result);
        }
        return new KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel>(false,
          new Minutz.Models.ViewModels.MeetingViewModel { ResultMessage = "There was a issue creating the meetingViewModel." });
      }
      else
      {
        var result = new Minutz.Models.ViewModels.MeetingViewModel
        {
          Id = meeting.Id.ToString(),
          MeetingAgendaCollection = agenda,
          Name = meeting.Name,
          MeetingAttendeeCollection = attendees,
          AvailableAttendeeCollection = new List<MeetingAttendee>(),
          Date = meeting.Date,
          MeetingAttachmentCollection = attachements,
          Duration = meeting.Duration,
          IsFormal = meeting.IsFormal,
          IsLocked = meeting.IsLocked,
          IsPrivate = meeting.IsPrivate,
          IsReacurance = meeting.IsReacurance,
          Status = meeting.Status,
          MeetingNoteCollection = notes,
          MeetingOwnerId = meeting.MeetingOwnerId,
          Outcome = meeting.Outcome,
          Purpose = meeting.Purpose,
          ReacuranceType = int.Parse(meeting.ReacuranceType),
          ResultMessage = "Successfully Created",
          Tag = meeting.Tag.Split(',').ToList(),
          Time = meeting.Time,
          TimeZone = meeting.TimeZone,
          UpdatedDate = DateTime.UtcNow
        };
        return new KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel>(true, result);
      }
    }

    public Minutz.Models.ViewModels.MeetingViewModel UpdateMeeting
      (AuthRestModel user, Minutz.Models.ViewModels.MeetingViewModel meetingViewModel)
    {
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
      
      var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
      
      if (!string.IsNullOrEmpty(user.InstanceId))
      {
        if (string.IsNullOrEmpty(meetingViewModel.MeetingOwnerId))
        {
          meetingViewModel.MeetingOwnerId = user.Sub;
        }
        var meetingEntity = new Meeting
        {
          Id = Guid.Parse(meetingViewModel.Id),
          Name = meetingViewModel.Name,
          Date = meetingViewModel.Date,
          Duration = meetingViewModel.Duration,
          IsFormal = meetingViewModel.IsFormal,
          IsLocked = meetingViewModel.IsLocked,
          IsPrivate = meetingViewModel.IsPrivate,
          Location = meetingViewModel.Location,
          IsReacurance = meetingViewModel.IsReacurance,
          MeetingOwnerId = meetingViewModel.MeetingOwnerId,
          Outcome = meetingViewModel.Outcome,
          Purpose = meetingViewModel.Purpose,
          ReacuranceType = meetingViewModel.ReacuranceType.ToString(),
          Tag = String.Join(",", meetingViewModel.Tag.ToArray()),
          Time = meetingViewModel.Time,
          TimeZone = meetingViewModel.TimeZone,
          UpdatedDate = DateTime.UtcNow
        };
        var result = _meetingRepository.Update(meetingEntity, user.InstanceId, instanceConnectionString);

        // Update the agenda items
        foreach (var agendaItem in meetingViewModel.MeetingAgendaCollection)
        {
          var update = _meetingAgendaRepository.Get(agendaItem.Id, user.InstanceId, instanceConnectionString);
          if (update == null || update.Id == Guid.Empty)
          {
            agendaItem.Id = Guid.NewGuid();
            agendaItem.ReferenceId = meetingViewModel.Id;
            _meetingAgendaRepository.Add(agendaItem, user.InstanceId, instanceConnectionString);
          }
          else
          {
            if (agendaItem.ReferenceId == null)
            {
              agendaItem.ReferenceId = meetingEntity.Id.ToString();
            }
            _meetingAgendaRepository.Update(agendaItem, user.InstanceId, instanceConnectionString);
          }
        }

        // Update the descisions
        foreach (var decisionItem in meetingViewModel.MeetingDecisionCollection)
        {
          var update = _decisionRepository.Get(decisionItem.Id, user.InstanceId, instanceConnectionString);
          if (update == null || update.Id == Guid.Empty)
          {
            decisionItem.Id = Guid.NewGuid();
            decisionItem.ReferanceId = Guid.Parse(meetingViewModel.Id);
            _decisionRepository.Add(decisionItem, user.InstanceId, instanceConnectionString);
          }
          else
          {
            if (decisionItem.ReferanceId == Guid.Empty)
            {
              decisionItem.ReferanceId = Guid.Parse(meetingEntity.Id.ToString());
            }
            _decisionRepository.Update(decisionItem, user.InstanceId, instanceConnectionString);
          }
        }
        
        // Update the attendees
        foreach (var attendee in meetingViewModel.MeetingAttendeeCollection)
        {
          var attendeeResult = _meetingAttendeeRepository.Get(attendee.Id, user.InstanceId, instanceConnectionString);
          if (attendeeResult == null || attendeeResult.Id == Guid.Empty)
          {
            attendee.Id = Guid.NewGuid();
            attendee.ReferenceId = Guid.Parse(meetingViewModel.Id);
             _meetingAttendeeRepository.Add(attendee, user.InstanceId, instanceConnectionString);
          }
          else
          {
            _meetingAttendeeRepository.Update(attendee, user.InstanceId, instanceConnectionString, masterConnectionString);
          }
        }

        // Update attachments
        foreach (var attachment in meetingViewModel.MeetingAttachmentCollection)
        {
          var attachmentResult =
            _meetingAttachmentRepository.Get(attachment.Id, user.InstanceId, instanceConnectionString);
          if (attachmentResult == null || attachmentResult.Id == Guid.Empty)
          {
            attachment.Id = Guid.NewGuid();
            attachment.ReferanceId = Guid.Parse(meetingViewModel.Id);
            _meetingAttachmentRepository.Add(attachment, user.InstanceId, instanceConnectionString);
          }
          else
          {
            _meetingAttachmentRepository.Update(attachment, user.InstanceId, instanceConnectionString);
          }
        }

        // Update Notes
        foreach (var note in meetingViewModel.MeetingNoteCollection)
        {
          var savedNote =
            _meetingNoteRepository.Get(Guid.Parse(note.Id), user.InstanceId, instanceConnectionString);
          if (savedNote == null || Guid.Parse(savedNote.Id) == Guid.Empty)
          {
            note.Id = Guid.NewGuid().ToString();
            note.ReferanceId = meetingViewModel.Id;
             _meetingNoteRepository.Add(note, user.InstanceId, instanceConnectionString);
          }
          else
          {
             _meetingNoteRepository.Update(note, user.InstanceId, instanceConnectionString);
          }
        }

        // Update Actions
        foreach (var action in meetingViewModel.MeetingActionCollection)
        {
          var actionAction =
            _meetingActionRepository.Get(action.Id, user.InstanceId, instanceConnectionString);
          if (actionAction == null || actionAction.Id == Guid.Empty)
          {
            action.Id = Guid.NewGuid();
            action.ReferanceId = Guid.Parse( meetingViewModel.Id);
            _meetingActionRepository.Add(action, user.InstanceId, instanceConnectionString);
          }
          else
          {
            _meetingActionRepository.Update(action, user.InstanceId,instanceConnectionString);
          }
        }

        var agendaItems =
          _meetingAgendaRepository.GetMeetingAgenda(meetingEntity.Id, user.InstanceId, instanceConnectionString);
        var availibeAttendees =
          _meetingAttendeeRepository.GetAvalibleAttendees(user.InstanceId,instanceConnectionString,masterConnectionString);
        var attendees =
          _meetingAttendeeRepository.GetMeetingAttendees(meetingEntity.Id, user.InstanceId, instanceConnectionString, masterConnectionString);
        var attachments =
          _meetingAttachmentRepository.GetMeetingAttachments(meetingEntity.Id, user.InstanceId, instanceConnectionString);
        var notes = _meetingNoteRepository.GetMeetingNotes(meetingEntity.Id, user.InstanceId, instanceConnectionString);
        var actions =
          _meetingActionRepository.GetMeetingActions(meetingEntity.Id, user.InstanceId, instanceConnectionString);

        // Get the changes from the database
        meetingViewModel.AvailableAttendeeCollection = availibeAttendees;
        meetingViewModel.MeetingAgendaCollection = agendaItems;
        meetingViewModel.MeetingAttendeeCollection = attendees;
        meetingViewModel.MeetingAttachmentCollection = attachments;
        meetingViewModel.MeetingNoteCollection = notes;
        meetingViewModel.MeetingActionCollection = actions;
      }
      return meetingViewModel;
    }

    public KeyValuePair<bool, string> DeleteMeeting
      (AuthRestModel user, Guid meetingId)
    {
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
      
      var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);

      var meetingResult = _meetingRepository.Delete(meetingId, user.InstanceId, instanceConnectionString);
      if (!meetingResult)
      {
        return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel.");
      }

      if (_meetingAgendaRepository.GetMeetingAgenda(meetingId, user.InstanceId, instanceConnectionString).Any())
      {
        var meetingAgenda =
          _meetingAgendaRepository.DeleteMeetingAgenda(meetingId, user.InstanceId, instanceConnectionString);
        if (!meetingAgenda)
        {
          return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel agenda items.");
        }
      }

      if (_meetingAttendeeRepository.GetMeetingAttendees(meetingId, user.InstanceId, instanceConnectionString, masterConnectionString).Any())
      {
        var meetingAttendee =
          _meetingAttendeeRepository.DeleteMeetingAttendees(meetingId, user.InstanceId, instanceConnectionString);
        if (!meetingAttendee)
        {
          return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel agenda attendee's.");
        }
      }

      if (_meetingAttachmentRepository.GetMeetingAttachments(meetingId, user.InstanceId, instanceConnectionString)
        .Any())
      {
        var meetingAttachments =
          _meetingAttachmentRepository.DeleteMeetingAcchments(meetingId, user.InstanceId, instanceConnectionString);
        if (!meetingAttachments)
        {
          return new KeyValuePair<bool, string>(false,
            "There was a issue removing the meetingViewModel agenda attachments.");
        }
      }

      if (_meetingNoteRepository.GetMeetingNotes(meetingId, user.InstanceId, instanceConnectionString).Any())
      {
        var notesResult =
          _meetingNoteRepository.DeleteMeetingNotes(meetingId, user.InstanceId, instanceConnectionString);
        if (!notesResult)
        {
          return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel agenda notes.");
        }
      }

      if (_meetingActionRepository.GetMeetingActions(meetingId, user.InstanceId, instanceConnectionString).Any())
      {
        var actionResult =
          _meetingActionRepository.DeleteMeetingActions(meetingId, user.InstanceId, instanceConnectionString);
        if (!actionResult)
        {
          return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel agenda actions.");
        }
      }

      return new KeyValuePair<bool, string>(true, "Successful.");
    }

    public MeetingAttendee GetAttendee
      (AuthRestModel user, Guid attendeeId, Guid meetingId)
    {
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
      
      var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
      
      var data = _meetingAttendeeRepository.GetMeetingAttendees(meetingId, user.InstanceId, instanceConnectionString, masterConnectionString)
        .FirstOrDefault(i => i.Id == attendeeId);
      return data;
    }


    public bool InviteUser
      (AuthRestModel user, MeetingAttendee attendee, string referenceMeetingId, string inviteEmail)
    {
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
      
      var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
      
      return _meetingAttendeeRepository.AddInvitee
        (attendee, user.InstanceId, instanceConnectionString, masterConnectionString,  _applicationSetting.Schema, referenceMeetingId, inviteEmail);
    }

    public KeyValuePair<bool, string> SendMinutes
      (AuthRestModel user, Guid meetingId)
    {
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
      
      var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
      
      var meeting = this.GetMeeting(user, meetingId.ToString());
      foreach (var attendee in meeting.MeetingAttendeeCollection)
      {
      }
      return new KeyValuePair<bool, string>(true, "");
    }

    public KeyValuePair<bool, string> GetMinutesPreview
      (AuthRestModel user, Guid meetingId, string folderPath)
    {
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
      
      var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
      
      var meeting =
        _meetingRepository.Get(meetingId, user.InstanceId, instanceConnectionString);
      var agendaItems =
        _meetingAgendaRepository.GetMeetingAgenda(meeting.Id, user.InstanceId, instanceConnectionString);
      var notes =
        _meetingNoteRepository.GetMeetingNotes(meeting.Id, user.InstanceId, instanceConnectionString);
      var attendees =
        _meetingAttendeeRepository.GetMeetingAttendees
          (meeting.Id, user.InstanceId, instanceConnectionString, masterConnectionString);
      
      (bool condition, string message, byte[] file) report = _reportRepository.CreateMinutesReport
        (CreateReportRequestPayload(meeting,agendaItems,attendees,notes));

      var fileName = $"{folderPath}/{DateTime.UtcNow}.pdf";
      using (var bw = new BinaryWriter(File.Open(fileName, FileMode.OpenOrCreate)))
      {
        bw.Write(report.file);
      }
      
      return new KeyValuePair<bool, string>(report.condition,fileName);
    }

    public KeyValuePair<bool, string> SendInvatations
      (AuthRestModel user, Guid meetingId, IInvatationService invatationService)
    {
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
      
      var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
      
      var meeting = this.GetMeeting(user, meetingId.ToString());
      foreach (var attendee in meeting.MeetingAttendeeCollection)
      {
        var invatation = invatationService.SendMeetingInvatation(attendee, meeting,"instanceId");
      }
      return new KeyValuePair<bool, string>(true, "successful");
    }

    public IEnumerable<KeyValuePair<string, string>> ExtractQueries
      (string returnUri)
    {
      var queries = new List<KeyValuePair<string, string>>();
      var queryCollection = returnUri.Split('?');
      foreach (var query in queryCollection)
      {
        if (query.Contains("="))
        {
          var temp = query.Split('=');
          queries.Add(new KeyValuePair<string, string>(temp[0], temp[1]));
        }
      }
      return queries;
    }

    private dynamic CreateReportRequestPayload
      (Meeting meeting, List<MeetingAgenda> agendaItems, List<MeetingAttendee> attendeeCollection, List<MeetingNote> noteCollection )
    {
      var agenditems = new List<dynamic>();
      var attendees = new List<dynamic>();
      var notes = new List<dynamic>();
      
      foreach (var agendaItem in agendaItems)
      {
        agenditems.Add(new { agendaHeading = agendaItem.AgendaHeading ,agendaText = agendaItem.AgendaText });
      }

      foreach (var attendee in attendeeCollection)
      {
        attendees.Add(new {name = attendee.Name , role = attendee.Role });
      }
      
      foreach (var note in noteCollection)
      {
        attendees.Add(new {noteText = note.NoteText });
      }
      return new 
      {
        name = meeting.Name,
        date = meeting.Date,
        location =  meeting.Location,
        time = meeting.Time,
        purpose = meeting.Purpose,
        outcome = meeting.Outcome,
        agenda = agenditems,
        attendees = attendees,
        notes = notes
      };
    }
  }
}