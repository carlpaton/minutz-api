using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.interfaces.Repositories;
using tzatziki.minutz.models;

namespace tzatziki.minutz.Controllers
{
	[Authorize]
	public class DashboardController : BaseController
	{
		
		public DashboardController(
													IOptions<AppSettings> settings,
													IProfileService profileService,
													ITokenStringHelper tokenStringHelper)
					 :base(settings, profileService, tokenStringHelper)
		{

	
		}

		public IActionResult Index()
		{
			this.UserProfile = User.ToProfile(ProfileService, TokenStringHelper, AppSettings);
			return View(new CalenderModel { User = this.UserProfile });
		}
	}
}
