using System.Collections.Generic;
using Models.Entities;

namespace Interface.Services
{
	public interface IMeetingService
	{
	  Meeting GetMeeting(string token, string id);
	  IEnumerable<Meeting> GetMeetings(string token);
	  KeyValuePair<bool, Models.ViewModels.Meeting> CreateMeeting(string token,
	                     Meeting meeting,
	                     List<MeetingAttendee> attendees,
	                     List<MeetingAgenda> agenda,
	                     List<MeetingNote> notes,
	                     List<MeetingAttachment> attachements);
	  bool UpdateMeeting(string token, Meeting meeting);
    IEnumerable<KeyValuePair<string, string>> ExtractQueries(string returnUri);
	}
}