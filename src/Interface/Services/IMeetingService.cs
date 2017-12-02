using System;
using System.Collections.Generic;
using Models.Entities;

namespace Interface.Services
{
	public interface IMeetingService
	{
	  Models.ViewModels.MeetingViewModel GetMeeting(string token, string id);
	  IEnumerable<Models.ViewModels.MeetingViewModel> GetMeetings(string token);
	  KeyValuePair<bool, string> DeleteMeeting(string token, Guid meetingId);

    KeyValuePair<bool, Models.ViewModels.MeetingViewModel> CreateMeeting(string token,
	                     Models.Entities.Meeting meeting,
	                     List<MeetingAttendee> attendees,
	                     List<MeetingAgenda> agenda,
	                     List<MeetingNote> notes,
	                     List<MeetingAttachment> attachements,
	                     List<MinutzAction> actions);
	  Models.ViewModels.MeetingViewModel UpdateMeeting(string token, Models.ViewModels.MeetingViewModel meetingViewModel);

	  IEnumerable<MinutzAction> GetMinutzActions(string referenceId,
	    string userTokenUid);

	  KeyValuePair<bool,string> SendMinutes(string token, Guid meetingId);

    IEnumerable<KeyValuePair<string, string>> ExtractQueries(string returnUri);
	}
}