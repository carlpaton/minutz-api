using System.Collections.Generic;
using Interface.Repositories;
using Interface.Services;
using Models.Entities;
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
                          IMeetingAttachmentRepository meetingAttachmentRepository)
    {
      _meetingRepository = meetingRepository;
      _meetingAgendaRepository = meetingAgendaRepository;
      _meetingAttendeeRepository = meetingAttendeeRepository;
      _meetingActionRepository = meetingActionRepository;
      _meetingAttachmentRepository = meetingAttachmentRepository;
      _userValidationService = userValidationService;
      _authenticationService = authenticationService;
      _applicationSetupRepository = applicationSetupRepository;
      _userRepository = userRepository;
      _applicationSetting = applicationSetting;
      _instanceRepository = instanceRepository;
    }

    public Meeting GetMeeting(string token, string id)
    {
      var userInfo = _authenticationService.GetUserInfo(token);
      var applicationUserProfile = _userValidationService.GetUser(userInfo.sub);
      var instance = _instanceRepository.GetByUsername(applicationUserProfile.InstanceId,
                                                       _applicationSetting.Schema,
                                                       _applicationSetting.CreateConnectionString(
                                                          _applicationSetting.Server,
                                                          _applicationSetting.Catalogue,
                                                          _applicationSetting.Username,
                                                          _applicationSetting.Password));
      var userConnectionString = GetConnectionString(instance.Password, instance.Username);
      return _meetingRepository.Get(Guid.Parse(id), instance.Username, userConnectionString);
    }

    public IEnumerable<Meeting> GetMeetings(string token)
    {
      var userInfo = _authenticationService.GetUserInfo(token);
      var applicationUserProfile = _userValidationService.GetUser(userInfo.sub);
      var instance = _instanceRepository.GetByUsername(applicationUserProfile.InstanceId,
        _applicationSetting.Schema,
        _applicationSetting.CreateConnectionString(
          _applicationSetting.Server,
          _applicationSetting.Catalogue,
          _applicationSetting.Username,
          _applicationSetting.Password));
      var userConnectionString = GetConnectionString(instance.Password, instance.Username);
      return _meetingRepository.List(instance.Username, userConnectionString);
    }

    public KeyValuePair<bool, Models.ViewModels.Meeting> CreateMeeting(string token,
                                                                      Meeting meeting,
                                                                      List<MeetingAttendee> attendees,
                                                                      List<MeetingAgenda> agenda,
                                                                      List<MeetingNote> notes,
                                                                      List<MeetingAttachment> attachements)
    {
      var userInfo = _authenticationService.GetUserInfo(token);
      var applicationUserProfile = _userValidationService.GetUser(userInfo.sub);

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
        foreach (var agendaItem in agenda)
        {
          agendaItem.ReferenceId = meeting.Id;
          var saveAgenda = _meetingAgendaRepository.Add(agendaItem, instance.Username, userConnectionString);
          if (!saveAgenda)
          {
            return new KeyValuePair<bool, Models.ViewModels.Meeting>(false,
              new Models.ViewModels.Meeting
              {
                ResultMessage = $"There was a issue creating the meeting agenda item for meeting {meeting.Name}."
              });
          }
        }
        foreach (var attendee in attendees)
        {
          attendee.ReferenceId = meeting.Id;
          var savedAttendee = _meetingAttendeeRepository.Add(attendee, instance.Username, userConnectionString);
          if (!savedAttendee)
          {
            return new KeyValuePair<bool, Models.ViewModels.Meeting>(false, new Models.ViewModels.Meeting { ResultMessage = "There was a issue creating the meeting." });
          }
        }

        foreach (var attachment in attachements)
        {
          attachment.ReferanceId = meeting.Id;
          //var savedAttachment = 
        }

        return new KeyValuePair<bool, Models.ViewModels.Meeting>(false, new Models.ViewModels.Meeting { ResultMessage = "There was a issue creating the meeting." });
      }
      else
      {
        return new KeyValuePair<bool, Models.ViewModels.Meeting>(false, new Models.ViewModels.Meeting { ResultMessage = "There was a issue creating the meeting." });
      }
    }

    public bool UpdateMeeting(string token, Meeting meeting)
    {
      var userInfo = _authenticationService.GetUserInfo(token);
      var applicationUserProfile = _userValidationService.GetUser(userInfo.sub);
      var instance = _instanceRepository.GetByUsername(applicationUserProfile.InstanceId,
        _applicationSetting.Schema,
        _applicationSetting.CreateConnectionString(
          _applicationSetting.Server,
          _applicationSetting.Catalogue,
          _applicationSetting.Username,
          _applicationSetting.Password));
      var userConnectionString = GetConnectionString(instance.Password, instance.Username);
      return _meetingRepository.Update(meeting, instance.Username, userConnectionString);
    }

    public IEnumerable<MinutzAction> GetMinutzActions(string referenceId,
                                                      string userTokenUid)
    {
      if (string.IsNullOrEmpty(referenceId))
      {
        throw new ArgumentNullException(nameof(referenceId), "Please provide a valid reference id.");
      }

      if (string.IsNullOrEmpty(userTokenUid))
      {
        throw new ArgumentNullException(nameof(userTokenUid), "Please provide a valid user token unique identifier.");
      }
      // check if referenceId is a meeting id
      // if id is a meeting id then check if meeting has actions for user

      // if meeting is not a meeting id [referenceId] then use it as the user Id and check for actions - these become tasks
      return new List<MinutzAction>();
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