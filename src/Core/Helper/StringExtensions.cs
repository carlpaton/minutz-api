using System;
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
  }
}
