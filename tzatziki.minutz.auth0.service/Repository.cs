using tzatziki.minutz.interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace tzatziki.minutz.auth0.service
{
  public class Repository : IAuth0Repository
  {
    public TicketReceivedContext Getrole(TicketReceivedContext context, IPersonRepository personRepository)
    {
      var identity = context.Principal.Identity as ClaimsIdentity;
      if (identity != null)
      {
        if (!context.Principal.HasClaim(c => c.Type == ClaimTypes.Role))
        {
          identity.AddClaim(new Claim(ClaimTypes.Role, personRepository.GetRole(identity.FindFirst("user_id").Value).ToString()));
        }
      }
      else
      {

      }
      return context;
    }

  }

}
