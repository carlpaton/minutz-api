using System;
using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Services
{
  public interface IMeetingService
  {
    MeetingAgenda CreateMeetingAgendaItem(MeetingAgenda agenda, AuthRestModel user);

    Minutz.Models.ViewModels.MeetingViewModel GetMeeting(
      AuthRestModel user, string id);

    (bool condition, int statusCode, string message,  IEnumerable<Minutz.Models.ViewModels.MeetingViewModel> value) GetMeetings(
      AuthRestModel user, string reference);

    KeyValuePair<bool, string> DeleteMeeting(
      AuthRestModel user, Guid meetingId);

    List<MeetingAttendee> UpdateMeetingAttendees(
      List<MeetingAttendee> data, AuthRestModel user);

    List<MeetingAgenda> UpdateMeetingAgendaItems(
      string meetingid, List<MeetingAgenda> data, AuthRestModel user);

    KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel> CreateMeeting(
      AuthRestModel user,  Meeting meeting, List<MeetingAttendee> attendees, List<MeetingAgenda> agenda, List<MeetingNote> notes,
      List<MeetingAttachment> attachements, List<MinutzAction> actions, string instanceId = "");

    Minutz.Models.ViewModels.MeetingViewModel UpdateMeeting(
      AuthRestModel user, Minutz.Models.ViewModels.MeetingViewModel meetingViewModel);

    IEnumerable<MinutzAction> GetMinutzActions(
      string referenceId, AuthRestModel user);

    KeyValuePair<bool, string> SendMinutes(
      AuthRestModel user, Guid meetingId);

    KeyValuePair<bool, string> SendInvatations(
      AuthRestModel user, Guid meetingId, IInvatationService invatationService);

    KeyValuePair<bool, string> GetMinutesPreview
      (AuthRestModel user, Guid meetingId,string folderPath);
    
    IEnumerable<KeyValuePair<string, string>> ExtractQueries(
      string returnUri);

    MeetingAttendee GetAttendee(
      AuthRestModel user, Guid attendeeId, Guid meetingId);


    bool InviteUser(
      AuthRestModel user, MeetingAttendee attendee, string referenceMeetingId, string inviteEmail);
    
    // Instance GetInstance(AuthRestModel user);
  }
}