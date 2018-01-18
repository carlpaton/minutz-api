using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Api.Extensions
{
  public static class RequestExtensions
  {
    public static string Token(this HttpRequest request)
    {
      if (!request.Headers.Any())
        throw new NullReferenceException("There was a problem looking for the token header, as there are no headers for this request");
      if (!request.Headers.Any(i => i.Key == "Authorization"))
        throw new NullReferenceException("There was a problem looking for the Authorization header.");
      if (string.IsNullOrEmpty(request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value))
        throw new NullReferenceException("There was a problem with the token provided, it seems empty.");
      return request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
    }
  }
}
