using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using minutz_core;
using minutz_interface.Entities;
using minutz_interface.Repositories;
using minutz_interface.Services;
using minutz_interface.ViewModels;
using minutz_models;
using minutz_models.Entities;
using minutz_models.ViewModels;
using minutz_sapi;
using minutz_sqlrepository;

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
			services.AddMvc();
			services.AddSwaggerGen();
			//MODELS
			services.AddTransient<IInstance, Instance>();
			services.AddTransient<IAppMetadata, AppMetadata>();
			services.AddTransient<IUserProfile, UserProfile>();

			//REPOSITORIES
			services.AddTransient<IInstanceRepository, InstanceRepository>();

			//SERVICES
			services.AddTransient<ITokenStringService, TokenStringService>(); 
			services.AddTransient<IMeetingService, MeetingService>();
			services.AddTransient<IAuth0OptionsService, Auth0OptionsService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			app.UseMvc();
			app.UseSwagger();
			app.UseSwaggerUi();
		}
	}
}
