using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Interface.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.ExternalServices;
using Interface.Services;
using SqlRepository;
using System.Linq;
using Api.Auth0;
using Core;

namespace Api
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      //Repositories
      services.AddTransient<IUserRepository, UserRepository>();
      services.AddTransient<IApplicationSetupRepository, ApplicationSetupRepository>();
      services.AddTransient<IMeetingRespository, MeetingRespository>();
      services.AddTransient<IMeetingAgendaRepository, MeetingAgendaRepository>();
      services.AddTransient<IMeetingAttendeeRepository, MeetingAttendeeRepository>();
      services.AddTransient<IMeetingActionRepository, MeetingActionRepository>();
      services.AddTransient<IInstanceRepository, InstanceRepository>();
      
      //Services
      services.AddTransient<IApplicationSetting, ApplicationSetting>();
      services.AddTransient<IUserValidationService, UserValidationService>();
      services.AddTransient<IAuthenticationService, AuthenticationService>();
      services.AddTransient<IApplicationManagerService, ApplicationManagerService>();

      services.AddMvc();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "Minutz Api", Version = "v1" });
      });
      string domain = "https://dockerdurban.auth0.com/";
      services.AddAuthorization(options =>
      {
        options.AddPolicy("user:user",
            policy => policy.Requirements.Add(new HasScopeRequirement("user:user", domain)));
        options.AddPolicy("user:attendee",
            policy => policy.Requirements.Add(new HasScopeRequirement("user:attendee", domain)));
      });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();
      var options = new JwtBearerOptions
      {
        Audience = "https://minutz.net",
        Authority = "https://dockerdurban.auth0.com/",
        Events = new JwtBearerEvents
        {
          OnTokenValidated = context =>
         {
           var claimsIdentity = context.Ticket.Principal.Identity as ClaimsIdentity;
           if (claimsIdentity != null)
           {
             string userId = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
           }
           return Task.FromResult(0);
         }
        }
      };
      app.UseJwtBearerAuthentication(options);
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minutz Api V1");
      });
      app.UseMvc();
    }
  }
}
