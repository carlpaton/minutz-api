using System;
namespace SqlRepository.Extensions
{
  public static class TupleToStringExtensions
  {
    public static string ToFormattedString(this (string key, string reference) tuple, string devider)
    {
      if (string.IsNullOrEmpty(tuple.key) && string.IsNullOrEmpty(tuple.reference)) return string.Empty;
      if (string.IsNullOrEmpty(devider)) throw new ArgumentException("There was no devider provided to create a valid refernce string.");
      return $"{tuple.key}{devider}{tuple.reference}";
    }
  }
}
