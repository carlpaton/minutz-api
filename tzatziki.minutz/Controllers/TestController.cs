using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.Interfaces;
using Microsoft.Extensions.Options;
using tzatziki.minutz.models;
using tzatziki.minutz.core;
using tzatziki.minutz.models.Auth;

namespace tzatziki.minutz.Controllers
{
	[Produces("application/json")]
	[Route("api/Test")]
	public class TestController : BaseController
	{
		private readonly IPersonService _personService;
		private readonly IViewRenderService _viewRenderService;
		private readonly IMeetingService _meetingService;
		private readonly string _connectionString = Environment.GetEnvironmentVariable("SQLCONNECTION");

		public TestController(ITokenStringHelper tokenStringHelper,
													IProfileService profileService,
													IInstanceService instanceService,
													IPersonService personService,
													IOptions<AppSettings> settings,
													IUserService userService,
													IViewRenderService viewRenderService,
													IMeetingService meetingService) : base(settings, profileService, tokenStringHelper, instanceService, userService)
		{
			_personService = personService;
			_viewRenderService = viewRenderService;
			_meetingService = meetingService;
		}

		[HttpGet]
		[Route("api/Testmail")]
		public HttpResponseMessage Get()
		{
			var html =_viewRenderService.RenderToStringAsync("EmailMessage/InvitePerson", new UserProfile {FirstName = "Lee-Roy", LastName = "Ashworth" }).Result;
			return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
		}
	}
}