using System;
using System.Diagnostics;
using Interface.Helper;
using Interface.Repositories;
using Interface.Repositories.Feature.Meeting;
using Interface.Services;
using Interface.Services.Feature.Invite;
using Minutz.Models.Entities;
using Minutz.Models.Message;
using Minutz.Models.ViewModels;

namespace Core.Feature.Invite
{
  public class InvitationService: IInvitationService
  {
    private readonly IEmailValidationService _emailValidationService;
    private readonly IMinutzAvailabilityRepository _availabilityRepository;
    private readonly IMinutzAttendeeRepository _attendeeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IApplicationSetting _applicationSetting;

    public InvitationService(IEmailValidationService emailValidationService,
                             IMinutzAvailabilityRepository availabilityRepository,
                             IMinutzAttendeeRepository attendeeRepository,
                             IUserRepository userRepository,
                             IApplicationSetting applicationSetting)
    {
      _emailValidationService = emailValidationService;
      _availabilityRepository = availabilityRepository;
      _attendeeRepository = attendeeRepository;
      _userRepository = userRepository;
      _applicationSetting = applicationSetting;
    }

    /// <summary>
    /// Invite a attendee
    /// Validate the attendee is new, alternatively update reference to the new instance if required
    /// </summary>
    /// <param name="user" typeof="AuthRestModel">The current user / owner</param>
    /// <param name="invitee" typeof="MeetingAttendee">The new attendee</param>
    /// <param name="meeting" typeof="Minutz.Models.Entities.Meeting">The meeting entity for the invitation</param>
    /// <returns typeof="MessageBase">Result if was successful</returns>
    public MessageBase InviteUser(AuthRestModel user,MeetingAttendee invitee, Minutz.Models.Entities.Meeting meeting)
    {
      var validEmail = _emailValidationService.Valid(invitee.Email);
      if (!validEmail) return new MessageBase{Condition = false, Message = $"{invitee.Email} is a invalid email address."};
      if(meeting.Id == Guid.Empty || string.IsNullOrEmpty(meeting.Name)) return new MessageBase
                                                                                {
                                                                                  Condition = false,
                                                                                  Message = "Meeting is invalid."
                                                                                };
      
      var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
        _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));

      var masterConnectionString = _applicationSetting.CreateConnectionString();

      var userValidation = _userRepository.CheckIfNewUser(invitee.Email, invitee.ReferenceId.ToString(),user.InstanceId, instanceConnectionString, masterConnectionString);
      switch (userValidation.Code)
      {
        case 1:
          //create person
          var createPersonResult = _userRepository.CreatePerson(invitee, masterConnectionString);
          if (!createPersonResult.Condition)
          {
            return new MessageBase{Condition = createPersonResult.Condition, Message = createPersonResult.Message, Code = createPersonResult.Code};
          }

          var createPersonAvailableResult  = _availabilityRepository.CreateAvailableAttendee(invitee, user.InstanceId, instanceConnectionString);
          if (!createPersonAvailableResult.Condition)
          {
            var message = $"Person was created but there was a issue with creating available attendee. \n {createPersonAvailableResult.Message}"; 
            return new MessageBase{
                                    Condition = createPersonAvailableResult.Condition,
                                    Message = message,
                                    Code = createPersonAvailableResult.Code};
          }

          var createPersonAttendeeResult =  _attendeeRepository.AddAttendee(invitee.ReferenceId, invitee, user.InstanceId, instanceConnectionString);
          if (!createPersonAttendeeResult.Condition)
          {
            var message = $"Person was created, and available attendee was created, but there was a issue with adding to meeting attendee. \n {createPersonAttendeeResult.Message}"; 
            return new MessageBase{
                                    Condition = createPersonAttendeeResult.Condition,
                                    Message = message,
                                    Code = createPersonAttendeeResult.Code};
          }

          break;
        case 2:
          //create available
          var createAvailableResult = _availabilityRepository.CreateAvailableAttendee(invitee, user.InstanceId, instanceConnectionString);
          if (!createAvailableResult.Condition)
          {
            return new MessageBase{Condition = createAvailableResult.Condition, Message = createAvailableResult.Message, Code = createAvailableResult.Code};
          }
          
          var createAvailableAttendeeResult = _attendeeRepository.AddAttendee(invitee.ReferenceId, invitee, user.InstanceId, instanceConnectionString);
          if (!createAvailableAttendeeResult.Condition)
          {
            var message = $"Available attendee was created, but there was a issue with adding to meeting attendee. \n {createAvailableAttendeeResult.Message}"; 
            return new MessageBase{
                                    Condition = createAvailableAttendeeResult.Condition,
                                    Message = message,
                                    Code = createAvailableAttendeeResult.Code};
          }
          break;
        case  3:
          //create meeting attendee
          var createAttendeeResult = _attendeeRepository.AddAttendee(invitee.ReferenceId, invitee, user.InstanceId, instanceConnectionString);
          if (!createAttendeeResult.Condition)
          {
            var message = $"There was a issue with adding to meeting attendee. \n {createAttendeeResult.Message}"; 
            return new MessageBase{
                                    Condition = createAttendeeResult.Condition,
                                    Message = message,
                                    Code = createAttendeeResult.Code};
          }
          break;
      }

      return new MessageBase { Condition = true, Message = "Successful"};
    }

    public bool SendMeetingInvitation(MeetingAttendee attendee, MeetingViewModel meeting, string instanceId)
    {
      throw new System.NotImplementedException();
    }
  }
}


