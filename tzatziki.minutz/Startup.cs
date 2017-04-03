using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using tzatziki.minutz.auth0.service;
using tzatziki.minutz.core;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.interfaces.Repositories;
using tzatziki.minutz.models;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.mysqlrepository;

namespace tzatziki.minutz
{
  public class Startup
  {
    public IConfigurationRoot Configuration { get; }

    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddAuthentication(options => options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
      services.AddMvc();
      services.AddOptions();
      services.Configure<Auth0Settings>(Configuration.GetSection("Auth0"));
      services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

      //db
      services.AddTransient<IInstanceRepository, InstanceRepository>();
      services.AddTransient<IPersonRepository, PersonRepository>();
      //services

      services.AddTransient<ITokenStringHelper, TokenStringHelper>();
      services.AddTransient<IProfileService, ProfileService>();

      services.AddTransient<IAuth0Repository, Repository>();
    }

    public void Configure(IApplicationBuilder app,
                          IHostingEnvironment env,
                          ILoggerFactory loggerFactory,
                          IOptions<Auth0Settings> auth0Settings,
                          IAuth0Repository auth0Repository,
                          IPersonRepository personRepository)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseBrowserLink();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseStaticFiles();

      app.UseCookieAuthentication(new CookieAuthenticationOptions
      {
        AutomaticAuthenticate = true,
        AutomaticChallenge = true,
      });

      // Add the OIDC middleware
      var options = new OpenIdConnectOptions("Auth0")
      {
        // Set the authority to your Auth0 domain
        Authority = $"https://{auth0Settings.Value.Domain}",

        // Configure the Auth0 Client ID and Client Secret
        ClientId = auth0Settings.Value.ClientId,
        ClientSecret = auth0Settings.Value.ClientSecret,
        // Do not automatically authenticate and challenge
        AutomaticAuthenticate = false,
        AutomaticChallenge = false,

        // Set response type to code
        ResponseType = "code",

        // Set the callback path, so Auth0 will call back to http://localhost:5000/signin-auth0 
        // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard 
        CallbackPath = new PathString("/Home"),

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
              // Add the Name ClaimType. This is required if we want User.Identity.Name to actually return something!
              if (!context.Principal.HasClaim(c => c.Type == ClaimTypes.Name) &&
                  identity.HasClaim(c => c.Type == "name"))
                identity.AddClaim(new Claim(ClaimTypes.Name, identity.FindFirst("name").Value));

              context = auth0Repository.Getrole(context, personRepository);
              //if (!context.Principal.HasClaim(c => c.Type == ClaimTypes.Role))
              //{
              //  identity.AddClaim(new Claim(ClaimTypes.Role, ));
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
            var logoutUri = $"https://{auth0Settings.Value.Domain}/v2/logout?client_id={auth0Settings.Value.ClientId}";

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
      options.Scope.Clear();
      options.Scope.Add("openid");
      options.Scope.Add("name");
      options.Scope.Add("email");
      options.Scope.Add("clientID");
      options.Scope.Add("updated_at");
      options.Scope.Add("created_at");
      options.Scope.Add("user_id");
      options.Scope.Add("nickname");
      options.Scope.Add("roles");
      options.Scope.Add("app_metadata");
      options.Scope.Add("user_metadata");

      app.UseOpenIdConnectAuthentication(options);

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
