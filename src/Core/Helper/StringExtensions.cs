using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Helper
{
  public static class StringExtensions
  {

    public static (string key, string value) TupleSplit(this string inputString)
    {
      if (string.IsNullOrEmpty(inputString)) throw new ArgumentException("inputString, was empty.");
      if (!inputString.Contains("|")) throw new ArgumentException("inputString, does not have a | in the string.");
      var inputs = inputString.Split('|');
      if (!string.IsNullOrEmpty(inputs[0]) && !string.IsNullOrEmpty(inputs[1]))
        return (inputs[0], inputs[1]);
      return (string.Empty, string.Empty);
    }

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
  }
}
