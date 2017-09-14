using System.Collections.Generic;
using Models.Entities;

namespace Interface.Services
{
	public interface IMeetingService
	{
	  Meeting GetMeeting(string token, string id);
	  IEnumerable<Meeting> GetMeetings(string token);
	  bool CreateMeeting(string token, Meeting meeting);
	  bool UpdateMeeting(string token, Meeting meeting);
    IEnumerable<KeyValuePair<string, string>> ExtractQueries(string returnUri);
	}
}