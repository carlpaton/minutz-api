using tzatziki.minutz.interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;

namespace tzatziki.minutz.auth0.service
{
  public class Repository : IAuth0Repository
  {
    public string Getrole(TicketReceivedContext context)
    {
      return "user";
    }

  }

}
