using System.Collections.Generic;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;
using System;

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
                          IMeetingNoteRepository meetingNoteRepository)
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
    }

    public Minutz.Models.ViewModels.MeetingViewModel GetMeeting(string token, string id)
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
      var meeting = _meetingRepository.Get(Guid.Parse(id), instance.Username, userConnectionString);
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
        ReacuranceType = meeting.ReacuranceType,
        Tag = meeting.Tag,
        Time = meeting.Time,
        TimeZone = meeting.TimeZone,
        UpdatedDate = DateTime.UtcNow,
        AvailableAttendeeCollection = _meetingAttendeeRepository.GetAvalibleAttendees(instance.Username, userConnectionString),
        MeetingAgendaCollection = _meetingAgendaRepository.GetMeetingAgenda(meeting.Id, instance.Username, userConnectionString),
        MeetingAttachmentCollection = _meetingAttachmentRepository.GetMeetingAttachments(meeting.Id, instance.Username, userConnectionString),
        MeetingAttendeeCollection = _meetingAttendeeRepository.GetMeetingAttendees(meeting.Id, instance.Username, userConnectionString),
        MeetingNoteCollection = _meetingNoteRepository.GetMeetingNotes(meeting.Id, instance.Username, userConnectionString)
      };
      return meetingViewModel;
    }

    public IEnumerable<Minutz.Models.ViewModels.MeetingViewModel> GetMeetings(string token)
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

      var result = new List<Minutz.Models.ViewModels.MeetingViewModel>();
      var meetings = _meetingRepository.List(instance.Username, userConnectionString);
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
          ReacuranceType = meeting.ReacuranceType,
          Tag = meeting.Tag,
          Time = meeting.Time,
          TimeZone = meeting.TimeZone,
          UpdatedDate = DateTime.UtcNow,
          AvailableAttendeeCollection = _meetingAttendeeRepository.GetAvalibleAttendees(instance.Username, userConnectionString),
          MeetingAgendaCollection = _meetingAgendaRepository.GetMeetingAgenda(meeting.Id, instance.Username, userConnectionString),
          MeetingAttachmentCollection = _meetingAttachmentRepository.GetMeetingAttachments(meeting.Id, instance.Username, userConnectionString),
          MeetingAttendeeCollection = _meetingAttendeeRepository.GetMeetingAttendees(meeting.Id, instance.Username, userConnectionString),
          MeetingNoteCollection = _meetingNoteRepository.GetMeetingNotes(meeting.Id, instance.Username, userConnectionString)
        };

        result.Add(meetingViewModel);
      }
      return result;
    }

    public KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel> CreateMeeting(string token,
                                                                                       Minutz.Models.Entities.Meeting meeting,
                                                                                       List<Minutz.Models.Entities.MeetingAttendee> attendees,
                                                                                       List<Minutz.Models.Entities.MeetingAgenda> agenda,
                                                                                       List<Minutz.Models.Entities.MeetingNote> notes,
                                                                                       List<Minutz.Models.Entities.MeetingAttachment> attachements,
                                                                                       List<Minutz.Models.Entities.MinutzAction> actions)
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

      var saveMeeting = _meetingRepository.Add(meeting, instance.Username, userConnectionString);
      if (saveMeeting)
      {
        //foreach (var agendaItem in agenda)
        //{
        //  agendaItem.Id = Guid.NewGuid().ToString();
        //  agendaItem.ReferenceId = meeting.Id.ToString();
        //  var saveAgenda = _meetingAgendaRepository.Add(agendaItem, instance.Username, userConnectionString);
        //  if (!saveAgenda)
        //  {
        //    return new KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel>(false,
        //      new Minutz.Models.ViewModels.MeetingViewModel
        //      {
        //        ResultMessage = $"There was a issue creating the meetingViewModel agenda item for meetingViewModel {meeting.Name}."
        //      });
        //  }
        //}

        //foreach (var attendee in attendees)
        //{
        //  attendee.Id = Guid.NewGuid().ToString();
        //  attendee.ReferenceId = meeting.Id.ToString();
        //  var savedAttendee = _meetingAttendeeRepository.Add(attendee, instance.Username, userConnectionString);
        //  if (!savedAttendee)
        //  {
        //    return new KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel>(false, new Minutz.Models.ViewModels.MeetingViewModel { ResultMessage = $"There was a issue creating the meetingViewModel attendee for meetingViewModel {meeting.Name}" });
        //  }
        //}

        //foreach (var attachment in attachements)
        //{
        //  attachment.Id = Guid.NewGuid().ToString();
        //  attachment.ReferanceId = meeting.Id.ToString();
        //  var savedAttachment = _meetingAttachmentRepository.Add(attachment, instance.Username, userConnectionString);
        //  if (!savedAttachment)
        //  {
        //    return new KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel>(false, new Minutz.Models.ViewModels.MeetingViewModel { ResultMessage = "There was a issue creating the meetingViewModel attachment for meetingViewModel {meetingViewModel.Name}" });
        //  }
        //}

        //foreach (var note in notes)
        //{
        //  note.Id = Guid.NewGuid().ToString();
        //  note.ReferanceId = meeting.Id.ToString();
        //  var noteSaved = _meetingNoteRepository.Add(note, instance.Username, userConnectionString);
        //  if (!noteSaved)
        //  {
        //    return new KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel>(false, new Minutz.Models.ViewModels.MeetingViewModel { ResultMessage = "There was a issue creating the meetingViewModel note for meetingViewModel {meetingViewModel.Name}" });
        //  }
        //}

        //foreach (var action in actions)
        //{
        //  action.Id = Guid.NewGuid().ToString();
        //  action.ReferanceId = meeting.Id.ToString();
        //  var actionSaved = _meetingActionRepository.Add(action, instance.Username, userConnectionString);
        //  if (!actionSaved)
        //  {
        //    return new KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel>(false, new Minutz.Models.ViewModels.MeetingViewModel { ResultMessage = "There was a issue creating the meetingViewModel action for meetingViewModel {meetingViewModel.Name}" });
        //  }
        //}

        var result = new Minutz.Models.ViewModels.MeetingViewModel
        {
          Id = meeting.Id.ToString(),
          MeetingAgendaCollection = agenda,
          Name = meeting.Name,
          MeetingAttendeeCollection = attendees,
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
          ReacuranceType = meeting.ReacuranceType,
          ResultMessage = "Successfully Created",
          Tag = meeting.Tag,
          Time = meeting.Time,
          TimeZone = meeting.TimeZone,
          UpdatedDate = DateTime.UtcNow
        };
        return new KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel>(true, result);
      }
      return new KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel>(false, new Minutz.Models.ViewModels.MeetingViewModel { ResultMessage = "There was a issue creating the meetingViewModel." });
    }

    public Minutz.Models.ViewModels.MeetingViewModel UpdateMeeting(string token, Minutz.Models.ViewModels.MeetingViewModel meetingViewModel)
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
        ReacuranceType = meetingViewModel.ReacuranceType,
        Tag = meetingViewModel.Tag,
        Time = meetingViewModel.Time,
        TimeZone = meetingViewModel.TimeZone,
        UpdatedDate = DateTime.UtcNow
      };

      var result = _meetingRepository.Update(meetingEntity, instance.Username, userConnectionString);
      foreach (var agendaItem in meetingViewModel.MeetingAgendaCollection)
      {
        var update = _meetingAgendaRepository.Get(Guid.Parse(agendaItem.Id), instance.Username, userConnectionString);
        if (update == null || Guid.Parse(update.Id) == Guid.Empty)
        {
          agendaItem.Id = Guid.NewGuid().ToString();
          agendaItem.ReferenceId = meetingViewModel.Id.ToString();
          var saveAgenda = _meetingAgendaRepository.Add(agendaItem, instance.Username, userConnectionString);
        }
        else
        {
          var updateAgenda = _meetingAgendaRepository.Update(agendaItem, instance.Username, userConnectionString);
        }
      }

      foreach (var attendee in meetingViewModel.MeetingAttendeeCollection)
      {
        var attendeeResult = _meetingAttendeeRepository.Get(Guid.Parse(attendee.Id), instance.Username, userConnectionString);
        if (attendeeResult == null || Guid.Parse(attendeeResult.Id) == Guid.Empty)
        {
          attendee.Id = Guid.NewGuid().ToString();
          attendee.ReferenceId = meetingViewModel.Id;
          var savedAttendee = _meetingAttendeeRepository.Add(attendee, instance.Username, userConnectionString);
        }
        else
        {
          var savedAttendee = _meetingAttendeeRepository.Update(attendee, instance.Username, userConnectionString);
        }
      }

      foreach (var attachment in meetingViewModel.MeetingAttachmentCollection)
      {
        var attachmentResult = _meetingAttachmentRepository.Get(Guid.Parse(attachment.Id), instance.Username, userConnectionString);
        if (attachmentResult == null || Guid.Parse(attachmentResult.Id) == Guid.Empty)
        {
          attachment.Id = Guid.NewGuid().ToString();
          attachment.ReferanceId = meetingViewModel.Id;
          var savedAttachment = _meetingAttachmentRepository.Add(attachment, instance.Username, userConnectionString);
        }
        else
        {
          var updateAttachment = _meetingAttachmentRepository.Update(attachment, instance.Username, userConnectionString);
        }
      }

      foreach (var note in meetingViewModel.MeetingNoteCollection)
      {
        var savedNote = _meetingAttachmentRepository.Get(Guid.Parse(note.Id), instance.Username, userConnectionString);
        if (savedNote == null || Guid.Parse(savedNote.Id) == Guid.Empty)
        {
          note.Id = Guid.NewGuid().ToString();
          note.ReferanceId = meetingViewModel.Id;
          var noteSaved = _meetingNoteRepository.Add(note, instance.Username, userConnectionString);
        }
        else
        {
          var noteUpdate = _meetingNoteRepository.Update(note, instance.Username, userConnectionString);
        }
      }

      foreach (var action in meetingViewModel.MeetingActionCollection)
      {
        var actionAction = _meetingActionRepository.Get(Guid.Parse(action.Id), instance.Username, userConnectionString);
        if (actionAction == null || Guid.Parse(actionAction.Id) == Guid.Empty)
        {
          action.Id = Guid.NewGuid().ToString();
          action.ReferanceId = meetingViewModel.Id;
          var actionSaved = _meetingActionRepository.Add(action, instance.Username, userConnectionString);
        }
        else
        {
          var actionUpdate = _meetingActionRepository.Update(action, instance.Username, userConnectionString);
        }
      }
      return meetingViewModel;
    }

    public KeyValuePair<bool, string> DeleteMeeting(string token, Guid meetingId)
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

      var meetingResult = _meetingRepository.Delete(meetingId, instance.Username, userConnectionString);
      if (!meetingResult)
        return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel.");

      var meetingAgenda = _meetingAgendaRepository.DeleteMeetingAgenda(meetingId, instance.Username, userConnectionString);
      if (!meetingAgenda)
        return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel agenda items.");

      var meetingAttendee = _meetingAttendeeRepository.DeleteMeetingAttendees(meetingId, instance.Username, userConnectionString);
      if (!meetingAttendee)
        return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel agenda attendee's.");

      var meetingAttachments =
        _meetingAttachmentRepository.DeleteMeetingAcchments(meetingId, instance.Username, userConnectionString);
      if (!meetingAttachments)
        return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel agenda attachments.");

      var notesResult = _meetingNoteRepository.DeleteMeetingNotes(meetingId, instance.Username, userConnectionString);
      if (!notesResult)
        return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel agenda notes.");

      var actionResult =
        _meetingActionRepository.DeleteMeetingActions(meetingId, instance.Username, userConnectionString);
      if (!actionResult)
        return new KeyValuePair<bool, string>(false, "There was a issue removing the meetingViewModel agenda actions.");
      return new KeyValuePair<bool, string>(true, "Successful.");
    }

    public IEnumerable<Minutz.Models.Entities.MinutzAction> GetMinutzActions(string referenceId,
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

    public KeyValuePair<bool, string> SendMinutes(string token, Guid meetingId)
    {
      var meeting = this.GetMeeting(token, meetingId.ToString());
      foreach (var attendee in meeting.MeetingAttendeeCollection)
      {

      }
      return new KeyValuePair<bool, string>(true, "");
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