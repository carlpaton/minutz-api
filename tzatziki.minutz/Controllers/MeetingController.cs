using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models;

namespace tzatziki.minutz.Controllers
{
	public class MeetingController : BaseController
	{
    public MeetingController(
                             ITokenStringHelper tokenStringHelper, 
                             IProfileService profileService, 
                             IOptions<AppSettings> settings) : base(settings, profileService, tokenStringHelper)
    {
    }

		public IActionResult Index()
		{
      
      return View(new CalenderModel { User =  User.ToProfile(ProfileService, TokenStringHelper, AppSettings) });
		}
	}
}
