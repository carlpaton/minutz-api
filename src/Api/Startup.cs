using System.Collections.Generic;
using Core;
using Core.ExternalServices;
using Core.LogProvider;
using Interface.Repositories;
using Interface.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Notifications;
using SqlRepository;
using Swashbuckle.AspNetCore.Swagger;

namespace Api {
  public class Startup {
    public static string _version = "V1.10";
    public Startup (IHostingEnvironment env) {
      var builder = new ConfigurationBuilder ()
        .SetBasePath (env.ContentRootPath)
        .AddJsonFile ("appsettings.json", optional : false, reloadOnChange : true)
        .AddJsonFile ($"appsettings.{env.EnvironmentName}.json", optional : true)
        .AddEnvironmentVariables ();
      Configuration = builder.Build ();
    }

    public IConfigurationRoot Configuration { get; }

    public void ConfigureServices (IServiceCollection services) {
      //Repositories
      services.AddTransient<IUserRepository, UserRepository> ();
      services.AddTransient<IApplicationSetupRepository, ApplicationSetupRepository> ();
      services.AddTransient<IMeetingRepository, MeetingRepository> ();
      services.AddTransient<IMeetingAgendaRepository, MeetingAgendaRepository> ();
      services.AddTransient<IMeetingAttendeeRepository, MeetingAttendeeRepository> ();
      services.AddTransient<IMeetingActionRepository, MeetingActionRepository> ();
      services.AddTransient<IMeetingAttachmentRepository, MeetingAttachmentRepository> ();
      services.AddTransient<IMeetingNoteRepository, MeetingNoteRepository> ();
      services.AddTransient<IInstanceRepository, InstanceRepository> ();
      services.AddTransient<ILogRepository, LogRepository> ();

      //Services
      services.AddTransient<IApplicationSetting, ApplicationSetting> ();
      services.AddTransient<INotify, Notify> ();
      services.AddTransient<ILogService, LogService> ();

      //Features
      services.AddTransient<IUserValidationService, UserValidationService> ();
      services.AddTransient<IAuthenticationService, AuthenticationService> ();
      services.AddTransient<IApplicationManagerService, ApplicationManagerService> ();
      services.AddTransient<IMeetingService, MeetingService> ();

      services.AddTransient<IInvatationService, InvatationService> ();
      services.AddTransient<IStartupService, StartupService> ();

      services.AddMemoryCache ();
      services.AddMvc ();
      services.AddSwaggerGen (c => {
        c.SwaggerDoc ("v1", new Info { Title = "Minutz Api", Version = Startup._version });
        //c.AddSecurityDefinition("oauth2", new OAuth2Scheme
        //{
        //  Type = "oauth2",
        //  Flow = "implicit",
        //  AuthorizationUrl = "http://localhost:65510/swagger/",
        //  Scopes = new Dictionary<string, string>
        //            {
        //                { "readAccess", "Access read operations" },
        //                { "writeAccess", "Access write operations" }
        //            }
        //});
      });
      string domain = "https://dockerdurban.auth0.com/";
      services.AddCors (options => {
        options.AddPolicy ("AllowAllOrigins",
          builder => {
            builder.AllowAnyMethod ().AllowAnyHeader ().AllowAnyOrigin ();
          });
      });
      services.AddAuthentication (options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

      }).AddJwtBearer (options => {
        options.Authority = domain;
        options.Audience = "https://dockerdurban.auth0.com/api/v2/";
      });
    }

    public void Configure (
      IApplicationBuilder app,
      IHostingEnvironment env,
      ILoggerFactory loggerFactory) {

      loggerFactory.AddConsole (Configuration.GetSection ("Logging"));
      //loggerFactory.AddApplicationInsights (app.ApplicationServices, LogLevel.Error);
      loggerFactory.AddProvider (new LoggerDBProvider ());
      loggerFactory.AddDebug ();
      app.UseAuthentication ();
      app.UseSwagger ();
      app.UseSwaggerUI (c => {
        c.SwaggerEndpoint ("/swagger/v1/swagger.json", Startup._version);
      });

      app.UseCors ("AllowAllOrigins");
      app.UseMvc ();
    }
  }
}