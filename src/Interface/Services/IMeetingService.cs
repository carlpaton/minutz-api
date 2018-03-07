using System;
using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Services
{
  public interface IMeetingService
  {
    MeetingAgenda CreateMeetingAgendaItem(MeetingAgenda agenda, string token);

    Minutz.Models.ViewModels.MeetingViewModel GetMeeting(
      string token,
      string id);

    (bool condition,
     int statusCode,
     string message,
     IEnumerable<Minutz.Models.ViewModels.MeetingViewModel> value) GetMeetings(string token, string reference);

    KeyValuePair<bool, string> DeleteMeeting(
      string token,
      Guid meetingId);

    List<MeetingAttendee> UpdateMeetingAttendees(
      List<MeetingAttendee> data,
      string token);

    List<MeetingAgenda> UpdateMeetingAgendaItems(
      string meetingid,
      List<MeetingAgenda> data,
      string token);

    KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel> CreateMeeting(
      AuthRestModel token,
      Meeting meeting,
      List<MeetingAttendee> attendees,
      List<MeetingAgenda> agenda,
      List<MeetingNote> notes,
      List<MeetingAttachment> attachements,
      List<MinutzAction> actions, string instanceId = "");

    Minutz.Models.ViewModels.MeetingViewModel UpdateMeeting(
      string token,
      Minutz.Models.ViewModels.MeetingViewModel meetingViewModel);

    IEnumerable<MinutzAction> GetMinutzActions(
      string referenceId,
      string userTokenUid);

    KeyValuePair<bool, string> SendMinutes(
      string token,
      Guid meetingId);

    KeyValuePair<bool, string> SendInvatations(
      string token,
      Guid meetingId,
      IInvatationService invatationService);

    IEnumerable<KeyValuePair<string, string>> ExtractQueries(string returnUri);

    MeetingAttendee GetAttendee(
      string token,
      Guid attendeeId,
      Guid meetingId);


    bool InviteUser(
      string token,
      MeetingAttendee attendee,
      string referenceMeetingId,
      string inviteEmail);
    
    Instance GetInstance(string token);
  }
}