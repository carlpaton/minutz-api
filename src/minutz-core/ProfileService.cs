using minutz_interface.Repositories;
using minutz_interface.Services;
using minutz_interface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace minutz_core
{
	public class ProfileService
	{
		private readonly ITokenStringService _tokenProfileService;
		private readonly IPersonRepository _personRepository;
		public ProfileService(ITokenStringService tokenStringService, IPersonRepository personRepository)
		{
			_tokenProfileService = tokenStringService;
			_personRepository = personRepository;
		}

		internal IUserProfile Initilise(IEnumerable<Claim> claims)
		{
			if (claims.FirstOrDefault(c => c.Type == "picture") == null)
			{
				claims.ToList().Add(new Claim("picture", Environment.GetEnvironmentVariable("DEFAULTPROFILEPIC")));
			}
			var user = _personRepository.Get(claims.FirstOrDefault(c => c.Type == "user_id").Value,
																				claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value,
																				claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value,
																				claims.FirstOrDefault(c => c.Type == "picture").Value,
																				Environment.GetEnvironmentVariable("SQLCONNECTION"));
			user.ProfileImage = claims.FirstOrDefault(c => c.Type == "picture")?.Value;
			user.ClientID = claims.FirstOrDefault(c => c.Type == "clientID")?.Value;
			user.Created_At = _tokenProfileService.ConvertTokenStringToDate(claims.FirstOrDefault(c => c.Type == "created_at")?.Value);
			user.Updated_At = _tokenProfileService.ConvertTokenStringToDate(claims.FirstOrDefault(c => c.Type == "updated_at")?.Value);
			if (string.IsNullOrEmpty(user.FirstName))
			{
				var split = user.Name.Split(' ');
				if (split.Length > 1)
				{
					user.FirstName = split[0];
					user.LastName = split[1];
				}
				else
				{
					user.FirstName = split[0];
				}
			}
			return user;
		}
	}
}
