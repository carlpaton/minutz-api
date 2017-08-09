using System.Collections.Generic;
using System;
using System.Security.Claims;

namespace minutz_core
{
	public class AuthService
	{
		internal readonly string _connectionString = Environment.GetEnvironmentVariable("MINUTZ_CONNECTIONSTRING");
		public void Getrole(
			ClaimsIdentity identity,
			//IPersonRepository personRepository,
			//IProfileService profileService,
			//ITokenStringHelper tokenStringHelper,
			List<KeyValuePair<string, string>> queries = null)
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
				//var userProfile = profileService.GetFromClaims(identity.Claims, tokenStringHelper, appsettings.Value);
				//identity.AddClaim(new Claim(ClaimTypes.Role, personRepository.GetRole(identity.FindFirst("user_id").Value,
				//																																			_connectionString,
				//																																			userProfile).ToString()));
			}
		}
	}
}
