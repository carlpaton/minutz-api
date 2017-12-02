using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
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
    [HttpGet("api/user", Name = "Get the user profile")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(AuthRestModel), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(AuthRestModel))]
    public IActionResult Get ()
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      var userInfo = _authenticationService.GetUserInfo (token);
      if (!_userValidationService.IsNewUser (userInfo.Sub))
        _userValidationService.CreateAttendee (userInfo);
      var result = _userValidationService.GetUser(userInfo.Sub);
      return Ok(result);
    }
  }
}