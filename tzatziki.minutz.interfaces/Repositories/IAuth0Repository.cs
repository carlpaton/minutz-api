using Microsoft.AspNetCore.Authentication;

namespace tzatziki.minutz.interfaces.Repositories
{
  public interface IAuth0Repository
  {
    TicketReceivedContext Getrole(TicketReceivedContext context, IPersonRepository personRepository);
  }
}
