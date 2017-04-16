using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.core;

namespace tzatziki.minutz.Controllers
{
  public class BaseController : Controller
  {
    public AppSettings AppSettings { get; set; }

    public UserProfile UserProfile { get; set; }

    public readonly ITokenStringHelper TokenStringHelper;
    public readonly IProfileService ProfileService;
    public readonly IInstanceService Instanceservice;
    public readonly IUserService UserService;

    public BaseController(IOptions<AppSettings> settings,
                          IProfileService profileService,
                          ITokenStringHelper tokenStringHelper,
                          IInstanceService instanceService,
                          IUserService userService)
    {
      AppSettings = settings.Value;
      TokenStringHelper = tokenStringHelper;
      ProfileService = profileService;
      Instanceservice = instanceService;
      UserService = userService;
    }
  }
}