using minutz_interface.Repositories;
using System.Collections.Generic;
using minutz_interface.Services;
using System.Security.Claims;
using System;

namespace minutz_core
{
	public class AuthService: IAuthService
	{
		private IProfileService _profileService;
		private IPersonRepository _personRepository;
		public AuthService(IProfileService profileService, 
											 IPersonRepository personRepository)
		{
			_profileService = profileService;
			_personRepository = personRepository;
		}

		internal readonly string _connectionString = Environment.GetEnvironmentVariable("MINUTZ_CONNECTIONSTRING");
		public void Getrole(
			ClaimsIdentity identity,
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
				var userProfile = _profileService.GetFromClaims(identity.Claims);
				identity.AddClaim(new Claim(ClaimTypes.Role, _personRepository.GetRole(identity.FindFirst("user_id").Value,
																																							 _connectionString,
																																							 userProfile).ToString()));
			}
		}
	}
}
