using System.Collections.Generic;
using tzatziki.minutz.interfaces;
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

    public Meeting Get(string connectionString, string schema, Meeting meeting, bool read = false)
    {
      return _meetingRepository.Get(connectionString, schema, meeting, read);
    }

    public IEnumerable<Meeting> Get(string connectionString, string schema, User user)
    {
      return _meetingRepository.Get(connectionString, schema, user);
    }
  }
}
