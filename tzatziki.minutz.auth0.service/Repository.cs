using tzatziki.minutz.interfaces.Repositories;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using tzatziki.minutz.interfaces;
using System.Security.Claims;
using tzatziki.minutz.models;
using System;


namespace tzatziki.minutz.auth0.service
{
  public class Repository : IAuth0Repository
  {
		internal readonly string _connectionString = Environment.GetEnvironmentVariable("SQLCONNECTION");

		public void Getrole(ClaimsIdentity identity,
                                         IPersonRepository personRepository,
                                         IOptions<AppSettings> appsettings,
                                         IProfileService profileService,
                                         ITokenStringHelper tokenStringHelper, 
																				 List<KeyValuePair<string,string>> queries = null)
    {
      if (identity != null)
      {
				if (queries != null)
				{
					foreach (var query in queries)
					{
						identity.AddClaim(new Claim(query.Key, query.Value));
					}
				}
				var userProfile = profileService.GetFromClaims(identity.Claims, tokenStringHelper, appsettings.Value);
				identity.AddClaim(new Claim(ClaimTypes.Role, personRepository.GetRole(identity.FindFirst("user_id").Value,
																																							_connectionString,
																																							userProfile).ToString()));
			}
    }
  }
}