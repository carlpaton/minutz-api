using Interface.Repositories;
using Interface.ViewModels;
using System.Collections.Generic;
using Interface.Services;
using Models.ViewModels;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace Core
{
	public class ProfileService: IProfileService
	{
		private readonly ITokenStringService _tokenProfileService;
		private readonly IPersonRepository _personRepository;
		public ProfileService(ITokenStringService tokenStringService, IPersonRepository personRepository)
		{
			_tokenProfileService = tokenStringService;
			_personRepository = personRepository;
		}

		public UserProfile GetFromClaims(IEnumerable<Claim> claims)
		{
			if (string.IsNullOrEmpty(claims.FirstOrDefault(c => c.Type == "user_id").Value))
			{
				throw new System.Exception("Claim user id is null.");
			}

			var user = Initilise(claims);
			ApplicationMetaData(claims, user);
			ProfilePicture(user);
			return user;
		}

		internal UserProfile Initilise(IEnumerable<Claim> claims)
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

		internal void ProfilePicture(UserProfile user)
		{
			if (user.ProfileImage == null)
				user.ProfileImage = Environment.GetEnvironmentVariable("DEFAULTPROFILEPIC");
		}

		internal void ApplicationMetaData(IEnumerable<Claim> claims, UserProfile user)
		{
			if (claims.FirstOrDefault(c => c.Type == "app_metadata") != null)
			{
				var app_data = claims.FirstOrDefault(c => c.Type == "app_metadata").Value;
				var app_metaData = JsonConvert.DeserializeObject<AppMetadata>(app_data);
				user.App_Metadata = app_metaData;
			}
			else
			{
				user.App_Metadata = new AppMetadata {Role = Interface.RoleEnum.Attendee.ToString() };
			}
		}

	}
}