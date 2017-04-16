using tzatziki.minutz.interfaces.Repositories;
using System.Security.Claims;
using tzatziki.minutz.models;
using Microsoft.Extensions.Options;
using tzatziki.minutz.interfaces;

namespace tzatziki.minutz.auth0.service
{
  public class Repository : IAuth0Repository
  {
    public void Getrole(ClaimsIdentity identity,
                                         IPersonRepository personRepository,
                                         IOptions<AppSettings> appsettings,
                                         IProfileService profileService,
                                         ITokenStringHelper tokenStringHelper)
    {
      if (identity != null)
      {
        if (!identity.HasClaim(c => c.Type == ClaimTypes.Role))
        {
          var userProfile = profileService.GetFromClaims(identity.Claims, tokenStringHelper, appsettings.Value);
          identity.AddClaim(new Claim(ClaimTypes.Role, personRepository.GetRole(identity.FindFirst("user_id").Value,
                                                                                appsettings.Value.ConnectionStrings.AzureConnection,
                                                                                userProfile).ToString()));
        }
      }
    }
  }
}