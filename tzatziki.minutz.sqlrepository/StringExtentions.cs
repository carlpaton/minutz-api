using System;

namespace tzatziki.minutz.sqlrepository
{
  public static class StringExtentions
  {
    public static string ToUserNameString(this string email)
    {
      return email.Split('@')[0];
    }

    public static string ToSchemaString(this Guid instanceId)
    {
      return instanceId.ToString().Replace("-", string.Empty);
    }

    public static string GeneratePassword()
    {
      return "@nathan001";
    }
  }
}