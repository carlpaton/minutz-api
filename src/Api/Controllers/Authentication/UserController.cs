using Api.Extensions;
using Api.Models;
using Interface;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers.Authentication
{
  public class UserController : Controller
  {
    
    private readonly IUserValidationService _userValidationService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IValidationService _validationService;

    private readonly ILogService _logService;
    public UserController (
      IUserValidationService userValidationService,
      IAuthenticationService authenticationService,
      IValidationService validationService,
      ILogService logService)
    {
      _userValidationService = userValidationService;
      _authenticationService = authenticationService;
      _validationService = validationService;
      _logService = logService;
    }

    [HttpPost("api/user/login", Name = "Log in the user.")]
    public IActionResult Login ([FromBody]  LoginModel user)

    {
      var validation = Request.HasAuthHeaders(_logService);
      if (!validation.Condition)
        return StatusCode(validation.Code, validation.Message);

      var loginValidation = Request.Validate(user, _logService);
      if (!loginValidation.Condition)
        return StatusCode(validation.Code, validation.Message);

      var loginResult =
        _authenticationService.LoginFromLoginForm (user.username, user.password, user.instanceId);
      return loginResult.Condition ? Ok (loginResult.InfoResponse) : StatusCode (loginResult.Code, loginResult.Message);
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