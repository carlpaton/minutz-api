using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using tzatziki.minutz.models;

namespace tzatziki.minutz.interfaces.Repositories
{
  public interface IAuth0Repository
  {
    void Getrole(ClaimsIdentity context, IPersonRepository personRepository, IOptions<AppSettings> appsettings, IProfileService profileService, ITokenStringHelper tokenStringHelper, List<KeyValuePair<string, string>> queries = null);
  }
}