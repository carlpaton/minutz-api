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
    private readonly IApplicationSetupRepository _applicationSetupRepository;
    private readonly IUserRepository _userRepository;
    private readonly IApplicationSetting _applicationSetting;
    private readonly IInstanceRepository _instanceRepository;

    private readonly Microsoft.Extensions.Logging.ILogger _logger;

    public MeetingService(IMeetingRepository meetingRepository,
      IMeetingAgendaRepository meetingAgendaRepository,
      IMeetingAttendeeRepository meetingAttendeeRepository,
      IMeetingActionRepository meetingActionRepository,
      IAuthenticationService authenticationService,
      IUserValidationService userValidationService,
      IApplicationSetupRepository applicationSetupRepository,
      IUserRepository userRepository,
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
      _applicationSetupRepository = applicationSetupRepository;
      _userRepository = userRepository;
      _applicationSetting = applicationSetting;
      _instanceRepository = instanceRepository;
      this._logger = logger.CreateLogger("MeetingService");
    }


    public MeetingAgenda CreateMeetingAgendaItem(MeetingAgenda agenda, string token)
    {
      if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token), "Please provide a user token.");
      if (agenda == null) throw new ArgumentNullException(nameof(agenda), "Please provide a agenda model");
      if (string.IsNullOrEmpty(agenda.ReferenceId)) throw new ArgumentNullException(nameof(agenda.ReferenceId), "Please provide a meeting id for the agenda item.");

      var auth = new AuthenticationHelper(token,
        _authenticationService,
        _instanceRepository,
        _applicationSetting,
        _userValidationService);
      var result = _meetingAgendaRepository.Add(agenda, auth.Instance.Username, auth.ConnectionString);
      return result ? agenda : new MeetingAgenda();
    }

    public List<Minutz.Models.Entities.MeetingAgenda> UpdateMeetingAgendaItems(
      string meetingId,
      List<Minutz.Models.Entities.MeetingAgenda> data,
      string token)
    {
      var userInfo = _authenticationService.GetUserInfo(token);
      var applicationUserProfile = _userValidationService.GetUser(userInfo.Sub);
      var instance = _instanceRepository.GetByUsername(applicationUserProfile.InstanceId,
                                                        _applicationSetting.Schema,
                                                        _applicationSetting.CreateConnectionString(
                                                          _applicationSetting.Server,
                                                          _applicationSetting.Catalogue,
                                                          _applicationSetting.Username,
                                                          _applicationSetting.Password));
      var userConnectionString = GetConnectionString(instance.Password, instance.Username);
      if (data.Any())
      {
        var meetingAgendaItems = _meetingAgendaRepository.GetMeetingAgenda(
          Guid.Parse(data.FirstOrDefault().ReferenceId),
          instance.Username,
          userConnectionString);
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
            _meetingAgendaRepository.Update(updateAgendaItem, instance.Username, userConnectionString);
          }
          else
          {
            _meetingAgendaRepository.Add(updateAgendaItem, instance.Username, userConnectionString);
          }
        }
        return _meetingAgendaRepository.GetMeetingAgenda(
          Guid.Parse(data.FirstOrDefault().ReferenceId),
          instance.Username,
          userConnectionString);
      }
      foreach (var agendaitem in data)
      {
        _meetingAgendaRepository.Add(agendaitem, instance.Username, userConnectionString);
      }
      return _meetingAgendaRepository.GetMeetingAgenda(
        Guid.Parse(data.FirstOrDefault().ReferenceId),
        instance.Username,
        userConnectionString);
    }

    public List<Minutz.Models.Entities.MeetingAttendee> UpdateMeetingAttendees(
      List<Minutz.Models.Entities.MeetingAttendee> data,
      string token)
    {
      var userInfo = _authenticationService.GetUserInfo(token);
      var applicationUserProfile = _userValidationService.GetUser(userInfo.Sub);
      var instance = _instanceRepository.GetByUsername(applicationUserProfile.InstanceId,
        _applicationSetting.Schema,
        _applicationSetting.CreateConnectionString(
          _applicationSetting.Server,
          _applicationSetting.Catalogue,
          _applicationSetting.Username,
          _applicationSetting.Password));
      var userConnectionString = GetConnectionString(instance.Password, instance.Username);
      if (data.Any())
      {
        var attendees = _meetingAttendeeRepository.GetMeetingAttendees(
          data.FirstOrDefault().ReferenceId,
          instance.Username,
          userConnectionString);
        foreach (var attendee in data)
        {
          var q = attendees.FirstOrDefault(i => i.Id == attendee.Id);
          if (q != null)
          {
            _meetingAttendeeRepository.Update(attendee, instance.Username, userConnectionString);
          }
          else
          {
            _meetingAttendeeRepository.Add(attendee, instance.Username, userConnectionString);
          }
        }
        return _meetingAttendeeRepository.GetMeetingAttendees(
          data.FirstOrDefault().ReferenceId,
          instance.Username,
          userConnectionString);
        ;
      }
      foreach (var newAttendee in data)
      {
        _meetingAttendeeRepository.Add(newAttendee, instance.Username, userConnectionString);
      }
      return _meetingAttendeeRepository.GetMeetingAttendees(
        data.FirstOrDefault().ReferenceId,
        instance.Username,
        userConnectionString);
      ;
    }

    public Instance GetInstance(string token)
    {
        return new AuthenticationHelper(
          token,
          _authenticationService,
          _instanceRepository,
          _applicationSetting,
          _userValidationService).Instance;
    }

    /// <summary>
    /// Gets the meeting.
    /// </summary>
    /// <returns>The meeting.</returns>
    /// <param name="token">Token.</param>
    /// <param name="id">Identifier.</param>
    public Minutz.Models.ViewModels.MeetingViewModel GetMeeting(
      string token,
      string id)
    {
      var auth = new AuthenticationHelper(token, _authenticationService, _instanceRepository, _applicationSetting,
        _userValidationService);

      // if (auth.UserInfo.Role == AuthenticationHelper.Guest)
      // {
      //   if (string.IsNullOrEmpty(auth.UserInfo.Related))
      //   {
      //     return new Minutz.Models.ViewModels.MeetingViewModel();
      //   }
      //   var relatedTuple = auth.UserInfo.Related.TupleSplit();
      //   var relatedItems = relatedTuple.value.SplitToList("&", ";");
      //   var meetingIds = new List<string>();
      //   var instanceId = string.Empty;
      //   foreach(var relatedItem in relatedItems)
      //   {
      //     if(relatedItem.value == id)
      //     {
      //       instanceId = relatedItem.key;
      //       meetingIds.Add(relatedItem.value);
      //     }
      //   }
      //   if(!meetingIds.Any()) return new Minutz.Models.ViewModels.MeetingViewModel();
        
      // }
      var meeting = _meetingRepository.Get(Guid.Parse(id), auth.Instance.Username, auth.ConnectionString);
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
        ReacuranceType = int.Parse(meeting.ReacuranceType),
        Tag = meeting.Tag.Split(',').ToList(),
        Time = meeting.Time,
        TimeZone = meeting.TimeZone,
        UpdatedDate = DateTime.UtcNow,
        AvailableAttendeeCollection =
          _meetingAttendeeRepository.GetAvalibleAttendees(auth.Instance.Username, auth.ConnectionString),
        MeetingAgendaCollection =
          _meetingAgendaRepository.GetMeetingAgenda(meeting.Id, auth.Instance.Username, auth.ConnectionString),
        MeetingAttachmentCollection =
          _meetingAttachmentRepository.GetMeetingAttachments(meeting.Id, auth.Instance.Username, auth.ConnectionString),
        MeetingAttendeeCollection =
          _meetingAttendeeRepository.GetMeetingAttendees(meeting.Id, auth.Instance.Username, auth.ConnectionString),
        MeetingNoteCollection =
          _meetingNoteRepository.GetMeetingNotes(meeting.Id, auth.Instance.Username, auth.ConnectionString)
      };
      return meetingViewModel;
    }

    /// <summary>
    /// Gets the meetings.
    /// </summary>
    /// <returns>The meetings.</returns>
    /// <param name="token">Token.</param>
    public (bool condition,
            int statusCode,
            string message,
            IEnumerable<Minutz.Models.ViewModels.MeetingViewModel> value) GetMeetings(string token, string referenceKey)
    {
      (string key, string reference) reference = (string.Empty, string.Empty);
      if (!string.IsNullOrEmpty(referenceKey))
        reference = referenceKey.TupleSplit();
      try
      {
        var auth = new AuthenticationHelper(
          token,
          _authenticationService,
          _instanceRepository,
          _applicationSetting,
          _userValidationService);

        if (auth.Instance == null)
          return (true, 404, "There are no meetings.", new List<Minutz.Models.ViewModels.MeetingViewModel>());

        var result = new List<Minutz.Models.ViewModels.MeetingViewModel>();
        if (auth.UserInfo.Role == AuthenticationHelper.Guest)
        {
          if (string.IsNullOrEmpty(auth.UserInfo.Related))
          {
            return (true, 200, "Success", new List<Minutz.Models.ViewModels.MeetingViewModel>());
          }
          var relatedTuple = auth.UserInfo.Related.TupleSplit();
          var relatedItems = relatedTuple.value.SplitToList("&", ";");
          var meetingIds = new List<string>();
          foreach (var relatedItem in relatedItems)
          {
            meetingIds.Add(relatedItem.value);
          }
          var filtered = _meetingRepository.List(auth.Instance.Username, auth.ConnectionString, meetingIds);
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
              ReacuranceType = int.Parse(meeting.ReacuranceType),
              Tag = meeting.Tag.Split(',').ToList(),
              Time = meeting.Time,
              TimeZone = meeting.TimeZone,
              UpdatedDate = DateTime.UtcNow,
              AvailableAttendeeCollection =
                _meetingAttendeeRepository.GetAvalibleAttendees(auth.Instance.Username, auth.ConnectionString),
              MeetingAgendaCollection =
                _meetingAgendaRepository.GetMeetingAgenda(meeting.Id, auth.Instance.Username, auth.ConnectionString),
              MeetingAttachmentCollection =
                _meetingAttachmentRepository.GetMeetingAttachments(meeting.Id, auth.Instance.Username, auth.ConnectionString),
              MeetingAttendeeCollection =
                _meetingAttendeeRepository.GetMeetingAttendees(meeting.Id, auth.Instance.Username, auth.ConnectionString),
              MeetingNoteCollection =
                _meetingNoteRepository.GetMeetingNotes(meeting.Id, auth.Instance.Username, auth.ConnectionString)
            };

            result.Add(meetingViewModel);
          }
          return (true, 200, "Success", result);
        }
        var meetings = _meetingRepository.List(auth.Instance.Username, auth.ConnectionString);
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
            ReacuranceType = int.Parse(meeting.ReacuranceType),
            Tag = meeting.Tag.Split(',').ToList(),
            Time = meeting.Time,
            TimeZone = meeting.TimeZone,
            UpdatedDate = DateTime.UtcNow,
            AvailableAttendeeCollection =
              _meetingAttendeeRepository.GetAvalibleAttendees(auth.Instance.Username, auth.ConnectionString),
            MeetingAgendaCollection =
              _meetingAgendaRepository.GetMeetingAgenda(meeting.Id, auth.Instance.Username, auth.ConnectionString),
            MeetingAttachmentCollection =
              _meetingAttachmentRepository.GetMeetingAttachments(meeting.Id, auth.Instance.Username, auth.ConnectionString),
            MeetingAttendeeCollection =
              _meetingAttendeeRepository.GetMeetingAttendees(meeting.Id, auth.Instance.Username, auth.ConnectionString),
            MeetingNoteCollection =
              _meetingNoteRepository.GetMeetingNotes(meeting.Id, auth.Instance.Username, auth.ConnectionString)
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

    /// <summary>
    /// Creates the meeting.
    /// </summary>
    /// <returns>The meeting.</returns>
    /// <param name="token">Token.</param>
    /// <param name="meeting">Meeting.</param>
    /// <param name="attendees">Attendees.</param>
    /// <param name="agenda">Agenda.</param>
    /// <param name="notes">Notes.</param>
    /// <param name="attachements">Attachements.</param>
    /// <param name="actions">Actions.</param>
    public KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel> CreateMeeting(
      AuthRestModel user,
      Meeting meeting,
      List<MeetingAttendee> attendees,
      List<MeetingAgenda> agenda,
      List<MeetingNote> notes,
      List<MeetingAttachment> attachements,
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
      
      
      //var auth = new AuthenticationHelper(token, _authenticationService, _instanceRepository, _applicationSetting, _userValidationService);
      
      if (!string.IsNullOrEmpty(user.InstanceId))
      {
        this._logger.LogInformation(Core.LogProvider.LoggingEvents.InsertItem, "CreateMeeting - Service - auth ", user);
        meeting.Name = " demo";
        meeting.MeetingOwnerId = user.Sub;
        this._logger.LogInformation(Core.LogProvider.LoggingEvents.InsertItem, "CreateMeeting - Service - auth ", user);
        var availibleAttendees =
          _meetingAttendeeRepository.GetAvalibleAttendees(instanceId, _applicationSetting.CreateConnectionString(
            _applicationSetting.Server,_applicationSetting.Catalogue,instanceId,_applicationSetting.GetInstancePassword(instanceId)));

        attendees.Add(new MeetingAttendee
        {
          Name = user.Name,
          Email = user.Email,
          Id = Guid.NewGuid(),
          PersonIdentity = user.Sub,
          Role = "Meeting Owner",
          Status = "Pending"
        });
        _logger.LogInformation(Core.LogProvider.LoggingEvents.InsertItem,
          "CreateMeeting - Service - added owner to attendees ", user);
        _logger.LogInformation(Core.LogProvider.LoggingEvents.InsertItem, "CreateMeeting - Service - auth ",
          _applicationSetting.CreateConnectionString());
        
        var saveMeeting = _meetingRepository.Add(meeting, instanceId,  _applicationSetting.CreateConnectionString(
          _applicationSetting.Server,_applicationSetting.Catalogue,instanceId,_applicationSetting.GetInstancePassword(instanceId)));
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

    /// <summary>
    /// Updates the meeting.
    /// </summary>
    /// <returns>The meeting.</returns>
    /// <param name="token">Token.</param>
    /// <param name="meetingViewModel">Meeting view model.</param>
    public Minutz.Models.ViewModels.MeetingViewModel UpdateMeeting(
      string token,
      Minutz.Models.ViewModels.MeetingViewModel meetingViewModel)
    {
      var auth = new AuthenticationHelper(token, _authenticationService, _instanceRepository, _applicationSetting,
        _userValidationService);
      if (!string.IsNullOrEmpty(auth.UserInfo.InstanceId))
      {
        if (string.IsNullOrEmpty(meetingViewModel.MeetingOwnerId))
        {
          meetingViewModel.MeetingOwnerId = auth.UserInfo.Sub;
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
        var result = _meetingRepository.Update(meetingEntity, auth.Instance.Username, auth.ConnectionString);

        // Update the agenda items
        foreach (var agendaItem in meetingViewModel.MeetingAgendaCollection)
        {
          var update = _meetingAgendaRepository.Get(agendaItem.Id, auth.Instance.Username, auth.ConnectionString);
          if (update == null || update.Id == Guid.Empty)
          {
            agendaItem.Id = Guid.NewGuid();
            agendaItem.ReferenceId = meetingViewModel.Id.ToString();
            var saveAgenda = _meetingAgendaRepository.Add(agendaItem, auth.Instance.Username, auth.ConnectionString);
          }
          else
          {
            if (agendaItem.ReferenceId == null)
            {
              agendaItem.ReferenceId = meetingEntity.Id.ToString();
            }
            var updateAgenda = _meetingAgendaRepository.Update(agendaItem, auth.Instance.Username, auth.ConnectionString);
          }
        }

        // Update the attendees
        foreach (var attendee in meetingViewModel.MeetingAttendeeCollection)
        {
          var attendeeResult = _meetingAttendeeRepository.Get(attendee.Id, auth.Instance.Username, auth.ConnectionString);
          if (attendeeResult == null || attendeeResult.Id == Guid.Empty)
          {
            attendee.Id = Guid.NewGuid();
            attendee.ReferenceId = Guid.Parse(meetingViewModel.Id);
            var savedAttendee = _meetingAttendeeRepository.Add(attendee, auth.Instance.Username, auth.ConnectionString);
          }
          else
          {
            var savedAttendee = _meetingAttendeeRepository.Update(attendee, auth.Instance.Username, auth.ConnectionString);
          }
        }

        // Update attachments
        foreach (var attachment in meetingViewModel.MeetingAttachmentCollection)
        {
          var attachmentResult =
            _meetingAttachmentRepository.Get(Guid.Parse(attachment.Id), auth.Instance.Username, auth.ConnectionString);
          if (attachmentResult == null || Guid.Parse(attachmentResult.Id) == Guid.Empty)
          {
            attachment.Id = Guid.NewGuid().ToString();
            attachment.ReferanceId = meetingViewModel.Id;
            var savedAttachment = _meetingAttachmentRepository.Add(attachment, auth.Instance.Username, auth.ConnectionString);
          }
          else
          {
            var updateAttachment =
              _meetingAttachmentRepository.Update(attachment, auth.Instance.Username, auth.ConnectionString);
          }
        }

        // Update Notes
        foreach (var note in meetingViewModel.MeetingNoteCollection)
        {
          var savedNote =
            _meetingAttachmentRepository.Get(Guid.Parse(note.Id), auth.Instance.Username, auth.ConnectionString);
          if (savedNote == null || Guid.Parse(savedNote.Id) == Guid.Empty)
          {
            note.Id = Guid.NewGuid().ToString();
            note.ReferanceId = meetingViewModel.Id;
            var noteSaved = _meetingNoteRepository.Add(note, auth.Instance.Username, auth.ConnectionString);
          }
          else
          {
            var noteUpdate = _meetingNoteRepository.Update(note, auth.Instance.Username, auth.ConnectionString);
          }
        }

        // Update Actions
        foreach (var action in meetingViewModel.MeetingActionCollection)
        {
          var actionAction =
            _meetingActionRepository.Get(Guid.Parse(action.Id), auth.Instance.Username, auth.ConnectionString);
          if (actionAction == null || Guid.Parse(actionAction.Id) == Guid.Empty)
          {
            action.Id = Guid.NewGuid().ToString();
            action.ReferanceId = meetingViewModel.Id;
            var actionSaved = _meetingActionRepository.Add(action, auth.Instance.Username, auth.ConnectionString);
          }
          else
          {
            var actionUpdate = _meetingActionRepository.Update(action, auth.Instance.Username, auth.ConnectionString);
          }
        }

        var agendaItems =
          _meetingAgendaRepository.GetMeetingAgenda(meetingEntity.Id, auth.Instance.Username, auth.ConnectionString);
        var availibeAttendees =
          _meetingAttendeeRepository.GetAvalibleAttendees(auth.Instance.Username, auth.ConnectionString);
        var attendees =
          _meetingAttendeeRepository.GetMeetingAttendees(meetingEntity.Id, auth.Instance.Username, auth.ConnectionString);
        var attachments =
          _meetingAttachmentRepository.GetMeetingAttachments(meetingEntity.Id, auth.Instance.Username, auth.ConnectionString);
        var notes = _meetingNoteRepository.GetMeetingNotes(meetingEntity.Id, auth.Instance.Username, auth.ConnectionString);
        var actions =
          _meetingActionRepository.GetMeetingActions(meetingEntity.Id, auth.Instance.Username, auth.ConnectionString);

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

    /// <summary>
    /// Deletes the meeting.
    /// </summary>
    /// <returns>The meeting.</returns>
    /// <param name="token">Token.</param>
    /// <param name="meetingId">Meeting identifier.</param>
    public KeyValuePair<bool, string> DeleteMeeting(string token, Guid meetingId)
    {
      var auth = new AuthenticationHelper(token, _authenticationService, _instanceRepository, _applicationSetting,
        _userValidationService);

      var meetingResult = _meetingRepository.Delete(meetingId, auth.Instance.Username, auth.ConnectionString);
      if (!meetingResult)
      {
        return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel.");
      }

      if (_meetingAgendaRepository.GetMeetingAgenda(meetingId, auth.Instance.Username, auth.ConnectionString).Any())
      {
        var meetingAgenda =
          _meetingAgendaRepository.DeleteMeetingAgenda(meetingId, auth.Instance.Username, auth.ConnectionString);
        if (!meetingAgenda)
        {
          return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel agenda items.");
        }
      }

      if (_meetingAttendeeRepository.GetMeetingAttendees(meetingId, auth.Instance.Username, auth.ConnectionString).Any())
      {
        var meetingAttendee =
          _meetingAttendeeRepository.DeleteMeetingAttendees(meetingId, auth.Instance.Username, auth.ConnectionString);
        if (!meetingAttendee)
        {
          return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel agenda attendee's.");
        }
      }

      if (_meetingAttachmentRepository.GetMeetingAttachments(meetingId, auth.Instance.Username, auth.ConnectionString)
        .Any())
      {
        var meetingAttachments =
          _meetingAttachmentRepository.DeleteMeetingAcchments(meetingId, auth.Instance.Username, auth.ConnectionString);
        if (!meetingAttachments)
        {
          return new KeyValuePair<bool, string>(false,
            "There was a issue removing the meetingViewModel agenda attachments.");
        }
      }

      if (_meetingNoteRepository.GetMeetingNotes(meetingId, auth.Instance.Username, auth.ConnectionString).Any())
      {
        var notesResult =
          _meetingNoteRepository.DeleteMeetingNotes(meetingId, auth.Instance.Username, auth.ConnectionString);
        if (!notesResult)
        {
          return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel agenda notes.");
        }
      }

      if (_meetingActionRepository.GetMeetingActions(meetingId, auth.Instance.Username, auth.ConnectionString).Any())
      {
        var actionResult =
          _meetingActionRepository.DeleteMeetingActions(meetingId, auth.Instance.Username, auth.ConnectionString);
        if (!actionResult)
        {
          return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel agenda actions.");
        }
      }

      return new KeyValuePair<bool, string>(true, "Successful.");
    }

    public MeetingAttendee GetAttendee(string token, Guid attendeeId, Guid meetingId)
    {
      var userInfo = _authenticationService.GetUserInfo(token);
      var applicationUserProfile = _userValidationService.GetUser(userInfo.Sub);
      var instance = _instanceRepository.GetByUsername(applicationUserProfile.InstanceId,
        _applicationSetting.Schema,
        _applicationSetting.CreateConnectionString(
          _applicationSetting.Server,
          _applicationSetting.Catalogue,
          _applicationSetting.Username,
          _applicationSetting.Password));
      var userConnectionString = GetConnectionString(instance.Password, instance.Username);
      var data = _meetingAttendeeRepository.GetMeetingAttendees(meetingId, instance.Username, userConnectionString)
        .FirstOrDefault(i => i.Id == attendeeId);
      return data;
    }

    public IEnumerable<MinutzAction> GetMinutzActions(string referenceId,
      string token)
    {
      if (string.IsNullOrEmpty(referenceId))
      {
        throw new ArgumentNullException(nameof(referenceId), "Please provide a valid reference id.");
      }

      if (string.IsNullOrEmpty(token))
      {
        throw new ArgumentNullException(nameof(token), "Please provide a valid user token unique identifier.");
      }

      var userInfo = _authenticationService.GetUserInfo(token);
      var applicationUserProfile = _userValidationService.GetUser(userInfo.Sub);
      var instance = _instanceRepository.GetByUsername(applicationUserProfile.InstanceId,
        _applicationSetting.Schema,
        _applicationSetting.CreateConnectionString(
          _applicationSetting.Server,
          _applicationSetting.Catalogue,
          _applicationSetting.Username,
          _applicationSetting.Password));
      var userConnectionString = GetConnectionString(instance.Password, instance.Username);

      if (referenceId != applicationUserProfile.InstanceId)
      {
        var actions =
          _meetingActionRepository.GetMeetingActions(Guid.Parse(referenceId), instance.Username, userConnectionString);
        return actions;
      }

      // check if referenceId is a meetingViewModel id
      // if id is a meetingViewModel id then check if meetingViewModel has actions for user

      // if meetingViewModel is not a meetingViewModel id [referenceId] then use it as the user Id and check for actions - these become tasks
      return new List<Minutz.Models.Entities.MinutzAction>();
    }

    public bool InviteUser(
      string token,
      MeetingAttendee attendee,
      string referenceMeetingId,
      string inviteEmail)
    {
      var auth = new AuthenticationHelper(token, _authenticationService, _instanceRepository, _applicationSetting,
        _userValidationService);
      string defaultConnectionString = _applicationSetting.CreateConnectionString();
      return _meetingAttendeeRepository.AddInvitee(
        attendee,
        auth.Instance.Username,
        auth.ConnectionString,
        defaultConnectionString,
        _applicationSetting.Schema,
        referenceMeetingId,
        inviteEmail);
    }

    public KeyValuePair<bool, string> SendMinutes(string token, Guid meetingId)
    {
      var meeting = this.GetMeeting(token, meetingId.ToString());
      foreach (var attendee in meeting.MeetingAttendeeCollection)
      {
      }
      return new KeyValuePair<bool, string>(true, "");
    }

    public KeyValuePair<bool, string> SendInvatations(
      string token,
      Guid meetingId,
      IInvatationService invatationService)
    {
      var meeting = this.GetMeeting(token, meetingId.ToString());
      foreach (var attendee in meeting.MeetingAttendeeCollection)
      {
        var invatation = invatationService.SendMeetingInvatation(attendee, meeting,"instanceId");
      }
      return new KeyValuePair<bool, string>(true, "successful");
    }

    public IEnumerable<KeyValuePair<string, string>> ExtractQueries(string returnUri)
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

    internal string GetConnectionString(string password, string username)
    {
      return _applicationSetting.CreateConnectionString(
        _applicationSetting.Server,
        _applicationSetting.Catalogue,
        username,
        password);
    }
  }
}