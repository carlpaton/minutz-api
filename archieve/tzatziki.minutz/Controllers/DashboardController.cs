using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.interfaces.Repositories;
using tzatziki.minutz.models;
using tzatziki.minutz.core;

namespace tzatziki.minutz.Controllers
{
  [Authorize]
  public class DashboardController : BaseController
  {
    public DashboardController(
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
      return View(new DashboardModel { });
    }
  }
}