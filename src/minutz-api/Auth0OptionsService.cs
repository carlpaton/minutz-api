using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using minutz_interface.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace minutz_sapi
{
	public class Auth0OptionsService : IAuth0OptionsService
	{
		private readonly IMeetingService _meetingService;
		public Auth0OptionsService(IMeetingService meetingService)
		{
			_meetingService = meetingService;
		}

		public OpenIdConnectOptions GetOptions()
		{
			return new OpenIdConnectOptions("Auth0")
			{
				// Set the authority to your Auth0 domain
				Authority = $"https://{Environment.GetEnvironmentVariable("AUTH0DOMAIN")}",

				// Configure the Auth0 Client ID and Client Secret
				ClientId = Environment.GetEnvironmentVariable("AUTH0CLIENTID"),
				ClientSecret = Environment.GetEnvironmentVariable("AUTH0CLIENTSECRET"),
				// Do not automatically authenticate and challenge
				AutomaticAuthenticate = false,
				AutomaticChallenge = false,

				// Set response type to code
				ResponseType = "code",

				// Set the callback path, so Auth0 will call back to http://localhost:5000/signin-auth0 Also
				// ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard
				CallbackPath = new PathString("/Dashboard"),

				// Configure the Claims Issuer to be Auth0
				ClaimsIssuer = "Auth0",

				Events = new OpenIdConnectEvents
				{
					OnTicketReceived = context =>
					{
						// Get the ClaimsIdentity
						var identity = context.Principal.Identity as ClaimsIdentity;
						if (identity != null)
						{
							// Add the Name ClaimType. This is required if we want User.Identity.Name to actually
							// return something!
							if (!context.Principal.HasClaim(c => c.Type == ClaimTypes.Name) &&
									identity.HasClaim(c => c.Type == "name"))
								identity.AddClaim(new Claim(ClaimTypes.Name, identity.FindFirst("name").Value));

							//if (context.ReturnUri.Contains(minutz_core.QueryString.referal.ToString()))
							//{
							//	var queries = _meetingService.ExtractQueries(context.ReturnUri);
							//	auth0Repository.Getrole(identity, personRepository, appsettings, profileService, tokenStringHelper, queries.ToList());
							//}
							//else
							//{
							//	auth0Repository.Getrole(identity, personRepository, appsettings, profileService, tokenStringHelper);
							//}
							// Check if token names are stored in Properties
							if (context.Properties.Items.ContainsKey(".TokenNames"))
							{
								// Token names a semicolon separated
								string[] tokenNames = context.Properties.Items[".TokenNames"].Split(';');

								// Add each token value as Claim
								foreach (var tokenName in tokenNames)
								{
									// Tokens are stored in a Dictionary with the Key ".Token.<token name>"
									string tokenValue = context.Properties.Items[$".Token.{tokenName}"];

									identity.AddClaim(new Claim(tokenName, tokenValue));
								}
							}
						}

						return Task.CompletedTask;
					},
					// handle the logout redirection
					OnRedirectToIdentityProviderForSignOut = (context) =>
					{
						var logoutUri = $"https://{Environment.GetEnvironmentVariable("AUTH0DOMAIN")}/v2/logout?client_id={Environment.GetEnvironmentVariable("AUTH0CLIENTID")}";

						var postLogoutUri = context.Properties.RedirectUri;
						if (!string.IsNullOrEmpty(postLogoutUri))
						{
							if (postLogoutUri.StartsWith("/"))
							{
								// transform to absolute
								var request = context.Request;
								postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
							}
							logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
						}

						context.Response.Redirect(logoutUri);
						context.HandleResponse();

						return Task.CompletedTask;
					}
				}
			};
		}
	}
}
