using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models;
using tzatziki.minutz.core;

namespace tzatziki.minutz.Controllers
{
  [Authorize]
  public class HomeController : BaseController
  {
		//private readonly IInstanceRepository _instanceRepository;
		private IProfileService _profileService;


		public HomeController(
      ITokenStringHelper tokenStringHelper,
      IProfileService profileService,
      IInstanceService instanceService,
      IOptions<AppSettings> settings,
      IUserService userService) : base(settings, profileService, tokenStringHelper, instanceService, userService)
    {
			_profileService = profileService;
      //_instanceRepository = instanceRepository;
    }

    public IActionResult Index()
    {
      //var identity = User.Identity as ClaimsIdentity;
      // identity.AddClaim(new Claim("role", "user"));

      this.UserProfile = User.ToProfile(ProfileService, TokenStringHelper, AppSettings);

      var model = new CalenderModel { };

      return View(model);
    }


    public IActionResult Error()
    {
      return View();
    }

    public ActionResult Notifications()
    {
      var model = new Test { Me = "Hello Model" };
      return PartialView("~/Views/Shared/_notifications.cshtml", model);
    }

    public ActionResult UserSwitcher()
    {
      var model = User.ToProfile(_profileService);
      return PartialView("~/Views/Shared/_userSwitcher.cshtml", model);
    }

    public ActionResult MainMenu()
    {
      var model = User.ToProfile(_profileService);
      return PartialView("~/Views/Shared/_mainSideMenu.cshtml", model);
    }
  }

  public class Test
  {
    public string Me { get; set; }
  }
}