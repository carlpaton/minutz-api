using System;
using System.Collections.Generic;
using System.Linq;

namespace Minutz.Models.Extensions
{
  public static class StringExtensions
  {
    /// <summary>
    /// extension that devides a string by two char characters and returns a list of (string, string)
    /// split - devides the first item into the key value pair
    /// devider - the character the indicated multiple records
    /// </summary>
    /// <returns>The reference extension.</returns>
    /// <param name="input">Input.</param>
    /// <param name="split">Split.</param>
    /// <param name="devider">Devider.</param>
    public static List<(string key, string value)> SplitToList(this string input, string split, string devider)
    {
      if (string.IsNullOrEmpty(input)) throw new ArgumentException("The input string is empty, so there for cannot split.");
      if (!input.Contains(split))
        throw new FormatException($"The input devider does not contain the characher : {split}, to allow a valid split");
      if (!input.Contains(devider))
      {
        var singleRecord = input.Split(Convert.ToChar(split));
        if (!string.IsNullOrEmpty(singleRecord[0]) && !string.IsNullOrEmpty(singleRecord[0]))
          return new List<(string instanceId, string meetingId)>() { (singleRecord[0], singleRecord[1]) };
        throw new FormatException($"The split character {split}, was provided by now values were given.");
      }
      var multipleResult = new List<(string instanceId, string meetingId)>();
      var many = input.Split(Convert.ToChar(devider)).ToList();
      foreach (string item in many)
      {
        if (string.IsNullOrEmpty(item)) continue;
        var singleRecord = item.Split(Convert.ToChar(split));
        if (!string.IsNullOrEmpty(singleRecord[0]) && !string.IsNullOrEmpty(singleRecord[0]))
          multipleResult.Add((singleRecord[0], singleRecord[1]));
      }
      return multipleResult;
    }

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
