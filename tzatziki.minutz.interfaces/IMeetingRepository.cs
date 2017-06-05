using System.Collections.Generic;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.interfaces
{
  public interface IMeetingRepository
  {
    /// <summary>
    /// Get by Id and return meeting object, if does not exist create the instance
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="schema"></param>
    /// <param name="meeting"></param>
    /// <returns></returns>
    Meeting Get(string connectionString, string schema, Meeting meeting, bool read = false);

    IEnumerable<Meeting> Get(string connectionString, string schema, User user);
  }
}