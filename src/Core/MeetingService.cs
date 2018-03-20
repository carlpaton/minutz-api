using System;
using System.Collections.Generic;
using System.Linq;
using Core.Helper;
using Interface.Repositories;
using Interface.Services;
using Microsoft.Extensions.Logging;
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
    private readonly IUserValidationService _userValidationService;
    private readonly IAuthenticationService _authenticationService;
   // private readonly IApplicationSetupRepository _applicationSetupRepository;
    // private readonly IUserRepository _userRepository;
    private readonly IApplicationSetting _applicationSetting;
    private readonly IInstanceRepository _instanceRepository;

    private readonly Microsoft.Extensions.Logging.ILogger _logger;

    public MeetingService(IMeetingRepository meetingRepository,
      IMeetingAgendaRepository meetingAgendaRepository,
      IMeetingAttendeeRepository meetingAttendeeRepository,
      IMeetingActionRepository meetingActionRepository,
      IAuthenticationService authenticationService,
      IUserValidationService userValidationService,
      // IApplicationSetupRepository applicationSetupRepository,
      //IUserRepository userRepository,
      IApplicationSetting applicationSetting,
      IInstanceRepository instanceRepository,
      IMeetingAttachmentRepository meetingAttachmentRepository,
      IMeetingNoteRepository meetingNoteRepository,
      ILoggerFactory logger)
    {
      _meetingRepository = meetingRepository;
      _meetingAgendaRepository = meetingAgendaRepository;
      _meetingAttendeeRepository = meetingAttendeeRepository;
      _meetingActionRepository = meetingActionRepository;
      _meetingAttachmentRepository = meetingAttachmentRepository;
      _meetingNoteRepository = meetingNoteRepository;
      _userValidationService = userValidationService;
      _authenticationService = authenticationService;
      //_applicationSetupRepository = applicationSetupRepository;
      //_userRepository = userRepository;
      _applicationSetting = applicationSetting;
      _instanceRepository = instanceRepository;
      this._logger = logger.CreateLogger("MeetingService");
    }


    public MeetingAgenda CreateMeetingAgendaItem(
      MeetingAgenda agenda, AuthRestModel user)
    {
      // if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token), "Please provide a user token.");
      if (agenda == null) throw new ArgumentNullException(nameof(agenda), "Please provide a agenda model");
      if (string.IsNullOrEmpty(agenda.ReferenceId)) throw new ArgumentNullException(nameof(agenda.ReferenceId), "Please provide a meeting id for the agenda item.");

      /*var auth = new AuthenticationHelper(token,
        _authenticationService,
        _instanceRepository,
        _applicationSetting,
        _userValidationService);*/
      var connectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server, _applicationSetting.Catalogue,
        user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
      
      var result = _meetingAgendaRepository.Add(agenda, user.InstanceId, connectionString);
      return result ? agenda : new MeetingAgenda();
    }

    public List<Minutz.Models.Entities.MeetingAgenda> UpdateMeetingAgendaItems(
      string meetingId, List<Minutz.Models.Entities.MeetingAgenda> data, AuthRestModel user)
    {
//      var userInfo = _authenticationService.GetUserInfo(token);
//      var applicationUserProfile = _userValidationService.GetUser(userInfo.Sub);
//      var instance = _instanceRepository.GetByUsername(applicationUserProfile.InstanceId,
//                                                        _applicationSetting.Schema,
//                                                        _applicationSetting.CreateConnectionString(
//                                                          _applicationSetting.Server,
//                                                          _applicationSetting.Catalogue,
//                                                          _applicationSetting.Username,
//                                                          _applicationSetting.Password));
//      var userConnectionString = GetConnectionString(instance.Password, instance.Username);
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
          
      var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
      
      if (data.Any())
      {
        var meetingAgendaItems = _meetingAgendaRepository.GetMeetingAgenda(
          Guid.Parse(data.FirstOrDefault().ReferenceId), user.InstanceId, instanceConnectionString);
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
          Guid.Parse(data.FirstOrDefault().ReferenceId), user.InstanceId, instanceConnectionString);
      }
      foreach (var agendaitem in data)
      {
        _meetingAgendaRepository.Add(agendaitem, user.InstanceId, instanceConnectionString);
      }
      return _meetingAgendaRepository.GetMeetingAgenda(
        Guid.Parse(data.FirstOrDefault().ReferenceId), user.InstanceId, instanceConnectionString);
    }

    public List<Minutz.Models.Entities.MeetingAttendee> UpdateMeetingAttendees(
      List<Minutz.Models.Entities.MeetingAttendee> data, AuthRestModel user)
    {
//      var userInfo = _authenticationService.GetUserInfo(token);
//      var applicationUserProfile = _userValidationService.GetUser(userInfo.Sub);
//      var instance = _instanceRepository.GetByUsername(applicationUserProfile.InstanceId,
//        _applicationSetting.Schema,
//        _applicationSetting.CreateConnectionString(
//          _applicationSetting.Server,
//          _applicationSetting.Catalogue,
//          _applicationSetting.Username,
//          _applicationSetting.Password));
      
      var userConnectionString = _applicationSetting.CreateConnectionString(
        _applicationSetting.Server, _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
      
      var masterConnectionString =  _applicationSetting.CreateConnectionString(
          _applicationSetting.Server, _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);      
      if (data.Any())
      {
        var attendees = _meetingAttendeeRepository.GetMeetingAttendees(
          data.FirstOrDefault().ReferenceId,
          user.InstanceId,userConnectionString, masterConnectionString);
        foreach (var attendee in data)
        {
          var q = attendees.FirstOrDefault(i => i.Id == attendee.Id);
          if (q != null)
          {
            _meetingAttendeeRepository.Update(attendee, user.InstanceId, userConnectionString);
          }
          else
          {
            _meetingAttendeeRepository.Add(attendee, user.InstanceId, userConnectionString);
          }
        }
        return _meetingAttendeeRepository.GetMeetingAttendees(
          data.FirstOrDefault().ReferenceId,user.InstanceId,userConnectionString, masterConnectionString);
      }
      foreach (var newAttendee in data)
      {
        _meetingAttendeeRepository.Add(newAttendee, user.InstanceId, userConnectionString);
      }
      return _meetingAttendeeRepository.GetMeetingAttendees(
        data.FirstOrDefault().ReferenceId, user.InstanceId,  userConnectionString, masterConnectionString);
    }

//    public Instance GetInstance(string token)
//    {
//        return new AuthenticationHelper(
//          token,
//          _authenticationService,
//          _instanceRepository,
//          _applicationSetting,
//          _userValidationService).Instance;
//    }

    /// <summary>
    /// Gets the meeting.
    /// </summary>
    /// <returns>The meeting.</returns>
    /// <param name="token">Token.</param>
    /// <param name="id">Identifier.</param>
    public Minutz.Models.ViewModels.MeetingViewModel GetMeeting
      (AuthRestModel user, string id)
    {
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
          
      var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
      
      var meeting = _meetingRepository.Get(Guid.Parse(id), user.InstanceId, instanceConnectionString);
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
        Status = meeting.Status,
        Purpose = meeting.Purpose,
        ReacuranceType = int.Parse(meeting.ReacuranceType),
        Tag = meeting.Tag.Split(',').ToList(),
        Time = meeting.Time,
        TimeZone = meeting.TimeZone,
        UpdatedDate = DateTime.UtcNow,
        AvailableAttendeeCollection =
          _meetingAttendeeRepository.GetAvalibleAttendees(user.InstanceId, instanceConnectionString,masterConnectionString),
        MeetingAgendaCollection =
          _meetingAgendaRepository.GetMeetingAgenda(meeting.Id, user.InstanceId, instanceConnectionString),
        MeetingAttachmentCollection =
          _meetingAttachmentRepository.GetMeetingAttachments(meeting.Id, user.InstanceId, instanceConnectionString),
        MeetingAttendeeCollection =
          _meetingAttendeeRepository.GetMeetingAttendees(meeting.Id, user.InstanceId, instanceConnectionString, masterConnectionString),
        MeetingNoteCollection =
          _meetingNoteRepository.GetMeetingNotes(meeting.Id, user.InstanceId, instanceConnectionString)
      };
      return meetingViewModel;
    }

    /// <summary>
    /// Gets the meetings.
    /// </summary>
    /// <returns>The meetings.</returns>
    /// <param name="token">Token.</param>
    public (bool condition, int statusCode, string message, IEnumerable<Minutz.Models.ViewModels.MeetingViewModel> value) GetMeetings
      (AuthRestModel user, string referenceKey)
    {
      (string key, string reference) reference = (string.Empty, string.Empty);
      if (!string.IsNullOrEmpty(referenceKey))
      {
        reference = referenceKey.TupleSplit();
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
              AvailableAttendeeCollection =
                _meetingAttendeeRepository.GetAvalibleAttendees(user.InstanceId, instanceConnectionString,masterConnectionString),
              MeetingAgendaCollection =
                _meetingAgendaRepository.GetMeetingAgenda(meeting.Id, user.InstanceId, instanceConnectionString),
              MeetingAttachmentCollection =
                _meetingAttachmentRepository.GetMeetingAttachments(meeting.Id, user.InstanceId, instanceConnectionString),
              MeetingAttendeeCollection =
                _meetingAttendeeRepository.GetMeetingAttendees(meeting.Id, user.InstanceId, instanceConnectionString, masterConnectionString),
              MeetingNoteCollection =
                _meetingNoteRepository.GetMeetingNotes(meeting.Id, user.InstanceId,  instanceConnectionString)
            };

            result.Add(meetingViewModel);
          }
          return (true, 200, "Success", result);
        }
        
        var meetings = _meetingRepository.List(user.InstanceId, instanceConnectionString);
        foreach (var meeting in meetings)
        {
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
            AvailableAttendeeCollection =
              _meetingAttendeeRepository.GetAvalibleAttendees(user.InstanceId, instanceConnectionString,masterConnectionString),
            MeetingAgendaCollection =
              _meetingAgendaRepository.GetMeetingAgenda(meeting.Id, user.InstanceId, instanceConnectionString),
            MeetingAttachmentCollection =
              _meetingAttachmentRepository.GetMeetingAttachments(meeting.Id, user.InstanceId, instanceConnectionString),
            MeetingAttendeeCollection =
              _meetingAttendeeRepository.GetMeetingAttendees(meeting.Id, user.InstanceId, instanceConnectionString, masterConnectionString),
            MeetingNoteCollection =
              _meetingNoteRepository.GetMeetingNotes(meeting.Id, user.InstanceId,  instanceConnectionString)
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
            user.Related.SplitToList (Minutz.Models.StringDeviders.InstanceStringDevider, Minutz.Models.StringDeviders.MeetingStringDevider);
          instanceId = relatedInstances.First().instanceId;
        }
      }

      this._logger.LogInformation(Core.LogProvider.LoggingEvents.InsertItem, "CreateMeeting - Service - entry point {ID}", 1);
      
      if (!string.IsNullOrEmpty(user.InstanceId))
      {
        this._logger.LogInformation(Core.LogProvider.LoggingEvents.InsertItem, "CreateMeeting - Service - auth ", user);
        
        meeting.Name = " demo";
        meeting.MeetingOwnerId = user.Sub;
        if (string.IsNullOrEmpty(meeting.Status))
        {
          meeting.Status = "create";
        }

        this._logger.LogInformation(Core.LogProvider.LoggingEvents.InsertItem, "CreateMeeting - Service - auth ", user);
        
        var masterConnectionString = _applicationSetting.CreateConnectionString
          (_applicationSetting.Server, _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
        
        var availibleAttendees =
          _meetingAttendeeRepository.GetAvalibleAttendees(instanceId, _applicationSetting.CreateConnectionString
            (_applicationSetting.Server,_applicationSetting.Catalogue,instanceId,_applicationSetting.GetInstancePassword(instanceId)),masterConnectionString);

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
        _logger.LogInformation(Core.LogProvider.LoggingEvents.InsertItem,
          "CreateMeeting - Service - added owner to attendees ", user);
        _logger.LogInformation(Core.LogProvider.LoggingEvents.InsertItem, "CreateMeeting - Service - auth ",
          _applicationSetting.CreateConnectionString());
        
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
            attachment.Id = Guid.NewGuid().ToString();
            attachment.ReferanceId = meeting.Id.ToString();
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
            action.Id = Guid.NewGuid().ToString();
            action.ReferanceId = meeting.Id.ToString();
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
      // var auth = new AuthenticationHelper(token, _authenticationService, _instanceRepository, _applicationSetting,
       //  _userValidationService);
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
        var meetingEntity = new Minutz.Models.Entities.Meeting
        {
          Id = Guid.Parse(meetingViewModel.Id),
          Name = meetingViewModel.Name,
          Date = meetingViewModel.Date,
          Duration = meetingViewModel.Duration,
          IsFormal = meetingViewModel.IsFormal,
          IsLocked = meetingViewModel.IsLocked,
          IsPrivate = meetingViewModel.IsPrivate,
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
            agendaItem.ReferenceId = meetingViewModel.Id.ToString();
            var saveAgenda = _meetingAgendaRepository.Add(agendaItem, user.InstanceId, instanceConnectionString);
          }
          else
          {
            if (agendaItem.ReferenceId == null)
            {
              agendaItem.ReferenceId = meetingEntity.Id.ToString();
            }
            var updateAgenda = _meetingAgendaRepository.Update(agendaItem, user.InstanceId, instanceConnectionString);
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
            var savedAttendee = _meetingAttendeeRepository.Add(attendee, user.InstanceId, instanceConnectionString);
          }
          else
          {
            var savedAttendee = _meetingAttendeeRepository.Update(attendee, user.InstanceId, instanceConnectionString);
          }
        }

        // Update attachments
        foreach (var attachment in meetingViewModel.MeetingAttachmentCollection)
        {
          var attachmentResult =
            _meetingAttachmentRepository.Get(Guid.Parse(attachment.Id), user.InstanceId, instanceConnectionString);
          if (attachmentResult == null || Guid.Parse(attachmentResult.Id) == Guid.Empty)
          {
            attachment.Id = Guid.NewGuid().ToString();
            attachment.ReferanceId = meetingViewModel.Id;
            var savedAttachment = _meetingAttachmentRepository.Add(attachment, user.InstanceId, instanceConnectionString);
          }
          else
          {
            var updateAttachment =
              _meetingAttachmentRepository.Update(attachment, user.InstanceId, instanceConnectionString);
          }
        }

        // Update Notes
        foreach (var note in meetingViewModel.MeetingNoteCollection)
        {
          var savedNote =
            _meetingAttachmentRepository.Get(Guid.Parse(note.Id), user.InstanceId, instanceConnectionString);
          if (savedNote == null || Guid.Parse(savedNote.Id) == Guid.Empty)
          {
            note.Id = Guid.NewGuid().ToString();
            note.ReferanceId = meetingViewModel.Id;
            var noteSaved = _meetingNoteRepository.Add(note, user.InstanceId, instanceConnectionString);
          }
          else
          {
            var noteUpdate = _meetingNoteRepository.Update(note, user.InstanceId, instanceConnectionString);
          }
        }

        // Update Actions
        foreach (var action in meetingViewModel.MeetingActionCollection)
        {
          var actionAction =
            _meetingActionRepository.Get(Guid.Parse(action.Id), user.InstanceId, instanceConnectionString);
          if (actionAction == null || Guid.Parse(actionAction.Id) == Guid.Empty)
          {
            action.Id = Guid.NewGuid().ToString();
            action.ReferanceId = meetingViewModel.Id;
            var actionSaved = _meetingActionRepository.Add(action, user.InstanceId, instanceConnectionString);
          }
          else
          {
            var actionUpdate = _meetingActionRepository.Update(action, user.InstanceId,instanceConnectionString);
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

    public KeyValuePair<bool, string> DeleteMeeting(
      AuthRestModel user, Guid meetingId)
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

    public MeetingAttendee GetAttendee(
      AuthRestModel user, Guid attendeeId, Guid meetingId)
    {
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
      
      var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
      
//      var userInfo = _authenticationService.GetUserInfo(token);
//      var applicationUserProfile = _userValidationService.GetUser(userInfo.Sub);
//      var instance = _instanceRepository.GetByUsername(applicationUserProfile.InstanceId,
//        _applicationSetting.Schema,
//        _applicationSetting.CreateConnectionString(
//          _applicationSetting.Server,
//          _applicationSetting.Catalogue,
//          _applicationSetting.Username,
//          _applicationSetting.Password));
//      var userConnectionString = GetConnectionString(instance.Password, instance.Username);
      var data = _meetingAttendeeRepository.GetMeetingAttendees(meetingId, user.InstanceId, instanceConnectionString, masterConnectionString)
        .FirstOrDefault(i => i.Id == attendeeId);
      return data;
    }

    public IEnumerable<MinutzAction> GetMinutzActions(
      string referenceId, AuthRestModel user)
    {
      if (string.IsNullOrEmpty(referenceId))
      {
        throw new ArgumentNullException(nameof(referenceId), "Please provide a valid reference id.");
      }

//      if (string.IsNullOrEmpty(token))
//      {
//        throw new ArgumentNullException(nameof(token), "Please provide a valid user token unique identifier.");
//      }

//      var userInfo = _authenticationService.GetUserInfo(token);
//      var applicationUserProfile = _userValidationService.GetUser(userInfo.Sub);
//      var instance = _instanceRepository.GetByUsername(applicationUserProfile.InstanceId,
//        _applicationSetting.Schema,
//        _applicationSetting.CreateConnectionString(
//          _applicationSetting.Server,
//          _applicationSetting.Catalogue,
//          _applicationSetting.Username,
//          _applicationSetting.Password));
//      var userConnectionString = GetConnectionString(instance.Password, instance.Username);
      
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
          
      var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);

      if (referenceId != user.InstanceId)
      {
        var actions =
          _meetingActionRepository.GetMeetingActions(Guid.Parse(referenceId), user.InstanceId, instanceConnectionString);
        return actions;
      }

      // check if referenceId is a meetingViewModel id
      // if id is a meetingViewModel id then check if meetingViewModel has actions for user

      // if meetingViewModel is not a meetingViewModel id [referenceId] then use it as the user Id and check for actions - these become tasks
      return new List<Minutz.Models.Entities.MinutzAction>();
    }

    public bool InviteUser(
      AuthRestModel user, MeetingAttendee attendee, string referenceMeetingId, string inviteEmail)
    {
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
      
      var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
      
      return _meetingAttendeeRepository.AddInvitee(
        attendee, user.InstanceId, instanceConnectionString, masterConnectionString,  _applicationSetting.Schema, referenceMeetingId, inviteEmail);
    }

    public KeyValuePair<bool, string> SendMinutes(AuthRestModel user, Guid meetingId)
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

    public KeyValuePair<bool, string> SendInvatations(
      AuthRestModel user, Guid meetingId, IInvatationService invatationService)
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

    public IEnumerable<KeyValuePair<string, string>> ExtractQueries(
      string returnUri)
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
  }
}