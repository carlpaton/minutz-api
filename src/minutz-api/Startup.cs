using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using minutz_core;
using minutz_interface.Entities;
using minutz_interface.Repositories;
using minutz_interface.Services;
using minutz_interface.ViewModels;
using minutz_models;
using minutz_models.Entities;
using minutz_models.ViewModels;
using minutz_sapi;
using SqlRepository;
using System;
using System.Text;

namespace minutz_api
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
            string domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
            });

            services.AddMvc();
			services.AddSwaggerGen();

			//MODELS
			services.AddTransient<IInstance, Instance>();
			services.AddTransient<IAppMetadata, AppMetadata>();
			services.AddTransient<IUserProfile, UserProfile>();

			//REPOSITORIES
			services.AddTransient<IInstanceRepository, InstanceRepository>();
			services.AddTransient<IPersonRepository, PersonRepository>();

			//SERVICES
			services.AddTransient<ITokenStringService, TokenStringService>(); 
			services.AddTransient<IMeetingService, MeetingService>();
			//services.AddTransient<IAuth0OptionsService, Auth0OptionsService>();
			services.AddTransient<IProfileService, ProfileService>();
			services.AddTransient<IAuthService, AuthService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, 
													IHostingEnvironment env, 
													ILoggerFactory loggerFactory 
													//,IAuth0OptionsService auth0OptionsService
            )
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();
            app.UseAuthentication();
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //	AutomaticAuthenticate = true,
            //	AutomaticChallenge = true,
            //});
            //var options = new JwtBearerOptions
            //{
            //	TokenValidationParameters =
            //			{
            //					ValidIssuer = $"https://{Environment.GetEnvironmentVariable("AUTH0DOMAIN")}/",
            //					ValidAudience = Environment.GetEnvironmentVariable("AUTH0CLIENTID"),
            //					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("AUTH0CLIENTSECRET")))
            //			}
            //};
            //app.UseJwtBearerAuthentication(options);

            //var options = auth0OptionsService.GetOptions();
            //options.Scope.Clear();
            //options.Scope.Add("openid");
            //options.Scope.Add("picture");
            //options.Scope.Add("name");
            //options.Scope.Add("email");
            //options.Scope.Add("clientID");
            //options.Scope.Add("updated_at");
            //options.Scope.Add("created_at");
            //options.Scope.Add("user_id");
            //options.Scope.Add("nickname");
            //options.Scope.Add("roles");
            //options.Scope.Add("app_metadata");
            //options.Scope.Add("user_metadata");
            //app.UseOpenIdConnectAuthentication(options);



            app.UseMvc();
			app.UseSwagger();
			app.UseSwaggerUi();
		}
	}
}
