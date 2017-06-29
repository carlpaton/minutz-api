using System.Collections.Generic;
using System.Linq;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models.Auth;
using System.Security.Claims;
using tzatziki.minutz.models;
using Newtonsoft.Json;
using tzatziki.minutz.interfaces.Repositories;
using System;

namespace tzatziki.minutz.core
{
  public class ProfileService : IProfileService
  {
    private readonly IPersonRepository _personRepository;

    public ProfileService(IPersonRepository personRepository)
    {
      _personRepository = personRepository;
    }

    public UserProfile GetFromClaims(
                                     IEnumerable<Claim> claims,
                                     ITokenStringHelper tokenStringHelper,
                                     AppSettings appsettings
                                     )
    {
      if (string.IsNullOrEmpty(claims.FirstOrDefault(c => c.Type == "user_id").Value))
      {
        throw new System.Exception("Claim user id is null.");
      }

      var user = Initilise(claims, tokenStringHelper, appsettings);
      ApplicationMetaData(claims, user);
      ProfilePicture(user, appsettings);
      return user;
    }

    internal UserProfile Initilise(IEnumerable<Claim> claims, ITokenStringHelper tokenStringHelper, AppSettings appsettings)
    {
      var user = _personRepository.Get(claims.FirstOrDefault(c => c.Type == "user_id").Value,
                                        claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value,
                                        claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value,
																				Environment.GetEnvironmentVariable("SQLCONNECTION"));
      user.ProfileImage = claims.FirstOrDefault(c => c.Type == "picture")?.Value;
      user.ClientID = claims.FirstOrDefault(c => c.Type == "clientID")?.Value;
      user.Created_At = tokenStringHelper.ConvertTokenStringToDate(claims.FirstOrDefault(c => c.Type == "created_at")?.Value);
      user.Updated_At = tokenStringHelper.ConvertTokenStringToDate(claims.FirstOrDefault(c => c.Type == "updated_at")?.Value);
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
        //create
      }
    }

    internal void ProfilePicture(UserProfile user, AppSettings appsettings)
    {
      if (user.ProfileImage == null)
      {
        user.ProfileImage = appsettings.DefaultProfilePicture;
      }
    }

    public UserProfile Update(UserProfile user, AppSettings appsettings)
    {
      return _personRepository.InsertInstanceIdForUser(user, Environment.GetEnvironmentVariable("SQLCONNECTION"));
    }

		public Guid GetInstanceIdForUser(string userIdentifier, string connectionString)
		{
			return _personRepository.GetInstanceIdForUser(userIdentifier, connectionString);
		}
  }
}