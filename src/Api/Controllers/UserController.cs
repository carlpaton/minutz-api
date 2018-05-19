using System.Linq;
using System.Threading.Tasks;
using Api.Extensions;
using Interface;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models;
using Minutz.Models.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
  public class UserController : Controller
  {
    internal string _auth = "xAuthMinutz";
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
      this._userValidationService = userValidationService;
      this._authenticationService = authenticationService;
      this._validationService = validationService;
      this._logService = logService;
    }

    [HttpPost("api/user/login", Name = "Log in the user.")]
    public IActionResult Login ([FromBody]  dynamic user)

    { 
      var hasHeader = Request.Headers.ToList ().Any (i => i.Key == "xAuthHeader");
      if (!hasHeader)
      {
        this._logService.Log (Minutz.Models.LogLevel.Info, "The request did not have xAuthHeader header.");
        return StatusCode (404, "please provide a valid username or password");
      }
      var authHeaderValue = Request.Headers.ToList ().First (i => i.Key == "xAuthHeader").Value;
      if (authHeaderValue != this._auth)
      {
        this._logService.Log (Minutz.Models.LogLevel.Info, "The request had a xAuthHeader header, but the value did not match the configuration for the instance.");
        return StatusCode (404, "please provide a valid username or password");
      }

      var username = user.username.ToString ();
      var instanceId = user.instanceId.ToString ();
      var password = user.password.ToString ();

      if (string.IsNullOrEmpty (username))
      {
        this._logService.Log (Minutz.Models.LogLevel.Info, "The request did not have a username.");
        return StatusCode (404, "please provide a valid username or password");
      }

      if (string.IsNullOrEmpty (password))
      {
        this._logService.Log (Minutz.Models.LogLevel.Info, "The request did not have a password.");
        return StatusCode (404, "please provide a valid username or password");
      }

      var loginResult =
        _authenticationService.LoginFromLoginForm ((string) username, (string) password, (string) instanceId);
      if (loginResult.Condition)
      {
        return Ok (loginResult.InfoResponse);
      }
      return StatusCode (404, loginResult.Message);
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