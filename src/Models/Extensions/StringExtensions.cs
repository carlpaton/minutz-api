using System.Collections.Generic;
using System.Linq;

namespace Minutz.Models.Extensions
{
  public static class StringExtensions
  {
    public static List<(string instanceId, string meetingId)> ToSplitTupleList(this string input)
    {
      var result = new List<(string instanceId, string meetingId)>();
      if (input.Contains("|") && !input.Contains(","))
      {
        var item = input.Split('|');
        if (!string.IsNullOrEmpty(item[0]) && !string.IsNullOrEmpty(item[1]))
          result.Add((item[0], item[1]));
      }
      if (input.Contains(",") && input.Contains("|"))
      {
        var entries = input.Split(',').ToList();
        foreach (string entry in entries)
        {
          var item = entry.Split('|');
          if (!string.IsNullOrEmpty(item[0]) && !string.IsNullOrEmpty(item[1]))
            result.Add((item[0], item[1]));
        }
      }
      return result;
    }
  }
}
