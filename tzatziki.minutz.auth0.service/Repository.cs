using tzatziki.minutz.interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using tzatziki.minutz.models;
using Microsoft.Extensions.Options;
using tzatziki.minutz.interfaces;

namespace tzatziki.minutz.auth0.service
{
  public class Repository : IAuth0Repository
  {
    public TicketReceivedContext Getrole(TicketReceivedContext context, 
                                         IPersonRepository personRepository, 
                                         IOptions<AppSettings> appsettings, 
                                         IProfileService profileService, 
                                         ITokenStringHelper tokenStringHelper)
    {
      var identity = context.Principal.Identity as ClaimsIdentity;
      if (identity != null)
      {
        if (!context.Principal.HasClaim(c => c.Type == ClaimTypes.Role))
        {
          var userProfile = profileService.GetFromClaims(identity.Claims, tokenStringHelper, appsettings.Value);
          identity.AddClaim(new Claim(ClaimTypes.Role, personRepository.GetRole(identity.FindFirst("user_id").Value,
                                                                                appsettings.Value.ConnectionStrings.LiveConnection,
                                                                                userProfile).ToString()));
        }
      }
      return context;
    }

  }

}
