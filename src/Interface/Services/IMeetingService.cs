using System;
using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Interface.Services
{
  public interface IMeetingService
  {
    Minutz.Models.ViewModels.MeetingViewModel GetMeeting(string token, string id);
    IEnumerable<Minutz.Models.ViewModels.MeetingViewModel> GetMeetings(string token);
    KeyValuePair<bool, string> DeleteMeeting(string token, Guid meetingId);

    List<Minutz.Models.Entities.MeetingAttendee> UpdateMeetingAttendees(
                                                    List<Minutz.Models.Entities.MeetingAttendee> data,
                                                    string token);

    List<Minutz.Models.Entities.MeetingAgenda> UpdateMeetingAgendaItems(
                                                List<Minutz.Models.Entities.MeetingAgenda> data,
                                                string token);
    KeyValuePair<bool, Minutz.Models.ViewModels.MeetingViewModel> CreateMeeting(string token,
                       Minutz.Models.Entities.Meeting meeting,
                       List<MeetingAttendee> attendees,
                       List<MeetingAgenda> agenda,
                       List<MeetingNote> notes,
                       List<MeetingAttachment> attachements,
                       List<MinutzAction> actions);
    Minutz.Models.ViewModels.MeetingViewModel UpdateMeeting(string token, Minutz.Models.ViewModels.MeetingViewModel meetingViewModel);

    IEnumerable<MinutzAction> GetMinutzActions(string referenceId,
      string userTokenUid);

    KeyValuePair<bool, string> SendMinutes(string token, Guid meetingId);

    IEnumerable<KeyValuePair<string, string>> ExtractQueries(string returnUri);

    Minutz.Models.Entities.MeetingAttendee GetAttendee(string token, Guid attendeeId ,Guid meetingId);
  }
}