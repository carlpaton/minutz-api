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

namespace Minutz.Api
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

    private string _domain = "https://dockerdurban.auth0.com/";
    public const string Version = "3.0.3";
    public const string Title = "Minutz Api";

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMemoryCache();

      services.AddTransient<IUserRepository, UserRepository>();
      services.AddTransient<IApplicationSetupRepository, ApplicationSetupRepository>();
      services.AddTransient<IMeetingRepository, MeetingRepository>();
      services.AddTransient<IMeetingAgendaRepository, MeetingAgendaRepository>();
      services.AddTransient<IMeetingAttendeeRepository, MeetingAttendeeRepository>();
      services.AddTransient<IMeetingActionRepository, MeetingActionRepository>();
      services.AddTransient<IMeetingAttachmentRepository, MeetingAttachmentRepository>();
      services.AddTransient<IMeetingNoteRepository, MeetingNoteRepository>();
      services.AddTransient<IInstanceRepository, InstanceRepository>();
      services.AddTransient<ILogRepository, LogRepository>();
      services.AddTransient<IReminderRepository, ReminderRepository>();
      services.AddTransient<INotificationRoleRepository, NotificationRoleRepository>();
      services.AddTransient<INotificationTypeRepository, NotificationTypeRepository>();
      services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();



      //Services
      services.AddTransient<IApplicationSetting, ApplicationSetting>();
      services.AddTransient<INotify, Notify>();
      services.AddTransient<ILogService, LogService>();

      //Features
      services.AddTransient<IUserValidationService, UserValidationService>();
      services.AddTransient<IAuthenticationService, AuthenticationService>();
      services.AddTransient<IApplicationManagerService, ApplicationManagerService>();
      services.AddTransient<IMeetingService, MeetingService>();

      services.AddTransient<IInvatationService, InvatationService>();
      services.AddTransient<IStartupService, StartupService>();
      services.AddTransient<IReminderService, ReminderService>();
      services.AddTransient<INotificationRoleService, NotificationRoleService>();
      services.AddTransient<INotificationTypeService, NotificationTypeService>();
      services.AddTransient<ISubscriptionService, SubscriptionService>();
      services.AddTransient<IInstanceService, InstanceService>();

      services.AddMemoryCache();
      services.AddMvc();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = Startup.Title, Version = Startup.Version });
      });
      string domain = this._domain;// "https://dockerdurban.auth0.com/";
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

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      //loggerFactory.AddProvider (new LoggerDBProvider ());

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseAuthentication();
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", Startup.Version);
      });
      app.UseCors("AllowAllOrigins");
      app.UseMvc();
    }
  }
}
