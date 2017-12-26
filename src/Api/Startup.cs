using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.ExternalServices;
using Core.LogProvider;
using Interface.Repositories;
using Interface.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notifications;
using SqlRepository;
using Swashbuckle.AspNetCore.Swagger;

namespace Minutz.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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
            
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Minutz Api", Version = "v1.11", TermsOfService = "TOU"});
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minutz Api V1.11");
            });
            app.UseMvc();
        }
    }
}
