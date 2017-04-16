using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models;
using tzatziki.minutz.core;

namespace tzatziki.minutz.Controllers
{
  [Authorize]
  public class MeetingController : BaseController
  {
    public MeetingController(
      ITokenStringHelper tokenStringHelper,
      IProfileService profileService,
      IInstanceService instanceService,
      IOptions<AppSettings> settings,
      IUserService userService) : base(settings, profileService, tokenStringHelper, instanceService, userService)
    {
    }

    public IActionResult Index()
    {
      this.UserProfile = User.ToProfile(ProfileService, TokenStringHelper, AppSettings);
      return View(new MeetngModel { });
    }
  }
}