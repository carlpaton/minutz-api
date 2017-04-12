using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using tzatziki.minutz.models;

namespace tzatziki.minutz.interfaces.Repositories
{
  public interface IAuth0Repository
  {
    TicketReceivedContext Getrole(TicketReceivedContext context, IPersonRepository personRepository, IOptions<AppSettings> appsettings, IProfileService profileService, ITokenStringHelper tokenStringHelper);
  }
}
