using System;
using System.Collections.Generic;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.core
{
  public class MeetingService : IMeetingService
  {
    private readonly IMeetingRepository _meetingRepository;
    public MeetingService(IMeetingRepository meetingRepository)
    {
      _meetingRepository = meetingRepository;
    }

    public Meeting Get( string schema, Meeting meeting, string callingUserId ,bool read = false)
    {
      return _meetingRepository.Get(Environment.GetEnvironmentVariable("SQLCONNECTION"), schema, meeting, callingUserId,read);
    }

    public IEnumerable<Meeting> Get(string schema, UserProfile user)
    {
      return _meetingRepository.Get(Environment.GetEnvironmentVariable("SQLCONNECTION"), schema, user);
    }

		public void DeleteAgenda(string schema, string agendaItemId)
		{
			_meetingRepository.DeleteMeetingAgendaItem(Environment.GetEnvironmentVariable("SQLCONNECTION"), schema, agendaItemId);
		}

		public void SaveFile(string schema, UserProfile user, string fileName, byte[] data, string meetingId)
		{
			_meetingRepository.SaveFile(Environment.GetEnvironmentVariable("SQLCONNECTION"), schema, user, fileName, data,meetingId);
		}
	}
}
