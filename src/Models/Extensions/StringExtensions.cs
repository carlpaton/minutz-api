using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public static List<(string instanceId, string meetingId)> SplitToList (this string input, string split, string devider)
    {
      if (string.IsNullOrEmpty (input)) throw new ArgumentException ("The input string is empty, so there for cannot split.");
      if (!input.Contains (split))
        throw new FormatException ($"The input devider does not contain the characher : {split}, to allow a valid split");
      if (!input.Contains (devider))
      {
        var singleRecord = input.Split (Convert.ToChar (split));
        if (!string.IsNullOrEmpty (singleRecord[0]) && !string.IsNullOrEmpty (singleRecord[0]))
          return new List<(string instanceId, string meetingId)> ()
          {
            (singleRecord[0], singleRecord[1])
          };
        throw new FormatException ($"The split character {split}, was provided by now values were given.");
      }
      var multipleResult = new List<(string instanceId, string meetingId)> ();
      var many = input.Split (Convert.ToChar (devider)).ToList ();
      foreach (string item in many)
      {
        if (string.IsNullOrEmpty (item)) continue;
        var singleRecord = item.Split (Convert.ToChar (split));
        if (!string.IsNullOrEmpty (singleRecord[0]) && !string.IsNullOrEmpty (singleRecord[0]))
          multipleResult.Add ((singleRecord[0], singleRecord[1]));
      }
      return multipleResult;
    }

    public static (string key, string value) TupleSplit (this string inputString)
    {
      if (string.IsNullOrEmpty (inputString)) throw new ArgumentException ("inputString, was empty.");
      if (!inputString.Contains ("|")) throw new ArgumentException ("inputString, does not have a | in the string.");
      var inputs = inputString.Split ('|');
      if (!string.IsNullOrEmpty (inputs[0]) && !string.IsNullOrEmpty (inputs[1]))
        return (inputs[0], inputs[1]);
      return (string.Empty, string.Empty);
    }

    public static List<(string instanceId, string meetingId)> ToSplitTupleList (this string input)
    {
      var result = new List<(string instanceId, string meetingId)> ();
      if (input.Contains ("|") && !input.Contains (","))
      {
        var item = input.Split ('|');
        if (!string.IsNullOrEmpty (item[0]) && !string.IsNullOrEmpty (item[1]))
          result.Add ((item[0], item[1]));
      }
      if (input.Contains (",") && input.Contains ("|"))
      {
        var entries = input.Split (',').ToList ();
        foreach (string entry in entries)
        {
          var item = entry.Split ('|');
          if (!string.IsNullOrEmpty (item[0]) && !string.IsNullOrEmpty (item[1]))
            result.Add ((item[0], item[1]));
        }
      }
      return result;
    }

    public static string ToRelatedString (this List<(string instanceId, string meetingId)> instances)
    {
      StringBuilder result = new StringBuilder ();
      foreach((string instanceId, string meetingId) instance in instances)
      {
        result.Append($"{instance.instanceId}{Models.StringDeviders.InstanceStringDevider}{instance.meetingId}{Models.StringDeviders.MeetingStringDevider}");
      }
      return result.ToString ();
    }
  }
}