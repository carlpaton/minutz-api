using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models;
using Minutz.Models.Entities;
using Newtonsoft.Json;

namespace Api.Controllers
{
  public class SignUpController : Controller
  {
    private readonly IUserValidationService _userValidationService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IApplicationManagerService _applicationManagerService;
    private readonly ILogService _logService;
    
    public SignUpController (
      IUserValidationService userValidationService,
      IAuthenticationService authenticationService,
      IApplicationManagerService applicationManagerService,
      ILogService logService)
    {
      _userValidationService = userValidationService;
      _authenticationService = authenticationService;
      _applicationManagerService = applicationManagerService;
      this._logService = logService;
    }
    /// <summary>
    /// Use this to start the full version.
    /// </summary>
    /// <returns>If it was successful or not</returns>
    //[Authorize]
    [HttpPost ("api/Signup")]
    public IActionResult Post ([FromBody] dynamic user)
    {
      _logService.Log(LogLevel.Info, JsonConvert.SerializeObject(user));
      var email = user.email.ToString ();
      var name = user.name.ToString ();
      var password = user.password.ToString ();
      var username = user.username.ToString ();
      var instanceId = user.RefInstanceId.ToString();
      var meetingId = user.refMeetingId.ToString();
      var role = user.role.ToString(); 
      
      // string role = "Guest";
      // if(string.IsNullOrEmpty(instanceId))
      // {
      //   role = "User";
      // }

      (bool condition, string message, AuthRestModel tokenResponse) result =
        this._authenticationService.CreateUser (name, username, email, password,role, instanceId, meetingId);
      
      _logService.Log(LogLevel.Info, JsonConvert.SerializeObject(result));
      _logService.Log(LogLevel.Info, JsonConvert.SerializeObject(result.tokenResponse));
      
      if (result.condition || result.message == "successfull")
        return Ok (result.tokenResponse);
      return StatusCode (500, result.message);
    }

    [Authorize]
    [HttpPost ("api/reset")]
    public IActionResult ResetAccount ()
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      var userInfo = _authenticationService.GetUserInfo (token);
      var person = _userValidationService.GetUser (userInfo.Sub);

      var result = _applicationManagerService.ResetAcccount (person);
      if (result.condition)
        return Ok ();
      return StatusCode (500, result.message);
    }
  }
}