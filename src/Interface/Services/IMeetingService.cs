using System;
using System.Collections.Generic;
using Models.Entities;

namespace Interface.Services
{
	public interface IMeetingService
	{
	  Models.ViewModels.Meeting GetMeeting(string token, string id);
	  IEnumerable<Models.ViewModels.Meeting> GetMeetings(string token);
	  KeyValuePair<bool, string> DeleteMeeting(string token, Guid meetingId);

    KeyValuePair<bool, Models.ViewModels.Meeting> CreateMeeting(string token,
	                     Models.Entities.Meeting meeting,
	                     List<MeetingAttendee> attendees,
	                     List<MeetingAgenda> agenda,
	                     List<MeetingNote> notes,
	                     List<MeetingAttachment> attachements,
	                     List<MinutzAction> actions);
	  Models.ViewModels.Meeting UpdateMeeting(string token, Models.ViewModels.Meeting meeting);
    IEnumerable<KeyValuePair<string, string>> ExtractQueries(string returnUri);
	}
}