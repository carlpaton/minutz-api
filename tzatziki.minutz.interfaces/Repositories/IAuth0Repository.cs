using Microsoft.AspNetCore.Authentication;

namespace tzatziki.minutz.interfaces.Repositories
{
  public interface IAuth0Repository
  {
    string Getrole(TicketReceivedContext context);
  }
}
