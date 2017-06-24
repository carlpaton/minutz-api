using System.Collections.Generic;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.interfaces
{
  public interface IMeetingService
  {
    Meeting Get(string connectionString, string schema, Meeting meeting, bool read = false);
    IEnumerable<Meeting> Get(string connectionString, string schema, UserProfile user);
		void DeleteAgenda(string connectionString, string schema, string agendaItemId);
		void SaveFile(string connectionString, string schema, UserProfile user, string fileName, byte[] data, string meetingId);
	}
}
