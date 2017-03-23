using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models;
using tzatziki.minutz.models.Auth;

namespace tzatziki.minutz.Controllers
{
 
  public class BaseController : Controller
  {
    public AppSettings AppSettings { get; set; }

    public UserProfile UserProfile { get; set; }

    public readonly ITokenStringHelper TokenStringHelper;
    public readonly IProfileService ProfileService;

    public BaseController(IOptions<AppSettings> settings, 
                          IProfileService profileService, 
                          ITokenStringHelper tokenStringHelper)
    {
      AppSettings = settings.Value;
      TokenStringHelper = tokenStringHelper;
      ProfileService = profileService;
    }

  }
}