﻿using System;
using System.Linq;
using System.Security.Claims;
using tzatziki.minutz.core;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models;
using tzatziki.minutz.models.Auth;

namespace tzatziki.minutz
{
	public static class UserExtentions
	{

		public static UserProfile ToProfile(this ClaimsPrincipal User,
															IProfileService profileService,
															ITokenStringHelper tokenStringHelper,
															AppSettings settings)
		{
			var claims = User.Claims.ToList();
			return profileService.GetFromClaims(claims, tokenStringHelper, settings);
		}

		public static UserProfile ToProfile(this ClaimsPrincipal User, IProfileService profileService)
		{

			var claims = User.Claims.ToList();
			var claimVerified = claims.FirstOrDefault(c => c.Type == "email_verified")?.Value;
			var role = User.Claims.FirstOrDefault(i => i.Type == System.Security.Claims.ClaimTypes.Role);
			if (role == null)
			{
				claims.Add(new Claim(System.Security.Claims.ClaimTypes.Role, "Attendee"));
			}
			//if (!string.IsNullOrEmpty(claimVerified))
			//{
			//if (Boolean.Parse(claimVerified))
			//{
			var tokenStringHelper = new TokenStringHelper();
			var model = new UserProfile
			{
				Name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
				EmailAddress = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
				ProfileImage = claims.FirstOrDefault(c => c.Type == "picture")?.Value,
				UserId = claims.FirstOrDefault(c => c.Type == "user_id")?.Value,
				ClientID = claims.FirstOrDefault(c => c.Type == "clientID")?.Value,
				Created_At = tokenStringHelper.ConvertTokenStringToDate(claims.FirstOrDefault(c => c.Type == "created_at")?.Value),
				Updated_At = tokenStringHelper.ConvertTokenStringToDate(claims.FirstOrDefault(c => c.Type == "updated_at")?.Value),
				Role = claims.FirstOrDefault(i => i.Type == System.Security.Claims.ClaimTypes.Role).Value
			};

			var connectionString = Environment.GetEnvironmentVariable("SQLCONNECTION");
			var instanceId = profileService.GetInstanceIdForUser(model.UserId, connectionString);
			model.InstanceId = instanceId;
			if (model.ProfileImage == null)
			{
				model.ProfileImage = "img/defaultuser.jpg";
			}
			return model;
			//}
			//}
			//throw new NullReferenceException("The account has not been verified");
		}
	}
}