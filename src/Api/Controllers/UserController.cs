using Api.Extensions;
using Interface;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Api.Controllers
{
  public class UserController : Controller
  {
    private readonly IUserValidationService _userValidationService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IValidationService _validationService;
    public UserController (
      IUserValidationService userValidationService,
      IAuthenticationService authenticationService,
      IValidationService validationService)
    {
      this._userValidationService = userValidationService;
      this._authenticationService = authenticationService;
      this._validationService = validationService;
    }

    [HttpPost("api/user/login", Name = "Log in the user.")]
    public IActionResult Login (string username, string password)
    {
      //var emailValidation = this._validationService.ValidEmail(email);
      //var passwordValidation = this._validationService.ValidPassword(password);
      if(string.IsNullOrEmpty(username))
        return StatusCode(404,"please provide a valid username or password");

      if(string.IsNullOrEmpty(password))
        return StatusCode(404,"please provide a valid username or password");

      var loginResult = this._authenticationService.Login(username,password);
      if(loginResult.condition)
      {
       return Ok (loginResult.tokenResponse);
      }
      return StatusCode(404, new { Message = loginResult.message });
    }

    [Authorize]
    [HttpGet ("api/user", Name = "Get the user profile")]
    [ProducesResponseType (typeof (string), 400)]
    [ProducesResponseType (typeof (AuthRestModel), 200)]
    [SwaggerResponse ((int) System.Net.HttpStatusCode.OK, Type = typeof (AuthRestModel))]
    public IActionResult Get (string reference)
    {
      if (!string.IsNullOrEmpty (reference))
      {
        if (reference.ToLower () == "none") reference = string.Empty;
      }

      var userInfo = _authenticationService.GetUserInfo (Request.Token ());
      if (!_userValidationService.IsNewUser (userInfo.Sub, reference))
        _userValidationService.CreateAttendee (userInfo, reference);

      var result = _userValidationService.GetUser (userInfo.Sub);
      return Ok (result);
    }
  }
}