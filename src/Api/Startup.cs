using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Interface.Repositories;
using Core.ExternalServices;
using Interface.Services;
using SqlRepository;
using Core;
using Notifications;

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
      services.AddTransient<IMeetingRepository, MeetingRepository>();
      services.AddTransient<IMeetingAgendaRepository, MeetingAgendaRepository>();
      services.AddTransient<IMeetingAttendeeRepository, MeetingAttendeeRepository>();
      services.AddTransient<IMeetingActionRepository, MeetingActionRepository>();
      services.AddTransient<IMeetingAttachmentRepository, MeetingAttachmentRepository>();
      services.AddTransient<IMeetingNoteRepository, MeetingNoteRepository>();
      services.AddTransient<IInstanceRepository, InstanceRepository>();

      //Services
      services.AddTransient<IApplicationSetting, ApplicationSetting>();
      services.AddTransient<IUserValidationService, UserValidationService>();
      services.AddTransient<IAuthenticationService, AuthenticationService>();
      services.AddTransient<IApplicationManagerService, ApplicationManagerService>();
      services.AddTransient<IMeetingService, MeetingService>();
      services.AddTransient<IInvatationService, InvatationService>();

      services.AddTransient<INotify, Notify>();
      services.AddTransient<IStartupService, StartupService>();


      services.AddMemoryCache();
      services.AddMvc();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "Minutz Api", Version = "V1.3" });
      });
      string domain = "https://dockerdurban.auth0.com/";
      services.AddCors(options =>
      {
        options.AddPolicy("AllowAllOrigins",
          builder =>
          {
            builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
          });
      });
      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

      }).AddJwtBearer(options =>
      {
        options.Authority = domain;
        options.Audience = "https://dockerdurban.auth0.com/api/v2/";
      });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // when an exception occurs, route to /Home/Error
        app.UseExceptionHandler("/Home/Error");
      }

      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Error);
      loggerFactory.AddDebug();
      app.UseAuthentication();
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minutz Api V1.3");
      });

      app.UseCors("AllowAllOrigins");
      app.UseMvc();
    }
  }
}
