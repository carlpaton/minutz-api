using System.Linq;
using Api.Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
  public class UserController : Controller
  {
    private readonly IUserValidationService _userValidationService;
    private readonly IAuthenticationService _authenticationService;
    public UserController(
      IUserValidationService userValidationService,
      IAuthenticationService authenticationService)
    {
      _userValidationService = userValidationService;
      _authenticationService = authenticationService;
    }

    [Authorize]
    [HttpGet("api/user", Name = "Get the user profile")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(AuthRestModel), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(AuthRestModel))]
    public IActionResult Get()
    {
      var userInfo = _authenticationService.GetUserInfo(Request.Token());
      if (!_userValidationService.IsNewUser(userInfo.Sub))
        _userValidationService.CreateAttendee(userInfo);
      var result = _userValidationService.GetUser(userInfo.Sub);
      return Ok(result);
    }
  }
}