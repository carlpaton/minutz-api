using System.Collections.Generic;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.interfaces
{
  public interface IMeetingService
  {
    Meeting Get(string connectionString, string schema, Meeting meeting);
    IEnumerable<Meeting> Get(string connectionString, string schema, User user);
  }
}
