using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace Api.Controllers
{
  [Route ("api/[controller]")]
  public class UserController : Controller
  {
    private readonly IUserValidationService _userValidationService;
    private readonly IAuthenticationService _authenticationService;
    public UserController (
      IUserValidationService userValidationService,
      IAuthenticationService authenticationService)
    {
      _userValidationService = userValidationService;
      _authenticationService = authenticationService;
    }

    [Authorize]
    [HttpGet]
    public AuthRestModel Get ()
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      var userInfo = _authenticationService.GetUserInfo (token);
      if (!_userValidationService.IsNewUser (userInfo.sub))
        _userValidationService.CreateAttendee (userInfo);
      return _userValidationService.GetUser (userInfo.sub);
    }
  }
}