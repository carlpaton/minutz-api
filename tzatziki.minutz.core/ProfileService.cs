using System.Collections.Generic;
using System.Linq;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models.Auth;
using System.Security.Claims;
using tzatziki.minutz.models;

namespace tzatziki.minutz.core
{
	public class ProfileService : IProfileService
	{
		public UserProfile GetFromClaims(
																		 IEnumerable<Claim> claims, 
																		 ITokenStringHelper tokenStringHelper, 
																		 AppSettings appsettings)
		{
      var app_data = claims.FirstOrDefault(c => c.Type == "app_metadata").Value;
      if (app_data == null)
      {
        //create attendee role into the user
      }
      else
      {
      }

      var model = new UserProfile
			{
				Name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
				EmailAddress = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
				ProfileImage = claims.FirstOrDefault(c => c.Type == "picture")?.Value,
				UserId = claims.FirstOrDefault(c => c.Type == "user_id")?.Value,
				ClientID = claims.FirstOrDefault(c => c.Type == "clientID")?.Value,
				Created_At = tokenStringHelper.ConvertTokenStringToDate(claims.FirstOrDefault(c => c.Type == "created_at")?.Value),
				Updated_At = tokenStringHelper.ConvertTokenStringToDate(claims.FirstOrDefault(c => c.Type == "updated_at")?.Value)
			};
			if (model.ProfileImage == null)
			{
				model.ProfileImage = appsettings.DefaultProfilePicture;
			}
			return model;
		}
	}
}
