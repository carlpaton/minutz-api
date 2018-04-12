using System;
using System.Linq;
using Api.Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
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
      var email = string.Empty;
      var name = string.Empty;
      var password = string.Empty;
      //var username = string.Empty;
      var instanceId = string.Empty;
      var meetingId = string.Empty;
      var role = string.Empty;
    
      _logService.Log(LogLevel.Info, JsonConvert.SerializeObject(user));
      try
      {
         email = user.email.ToString ();
         name = user.name.ToString ();
         password = user.password.ToString ();
       // username = user.username.ToString ();
         instanceId = user.RefInstanceId.ToString();
         meetingId = user.refMeetingId.ToString();
         role = user.role.ToString();
      }
      catch (Exception e)
      {
        _logService.Log(LogLevel.Info, e.Message);
        return StatusCode(500, "Could not user some/any imputs provided.");
      }
      

      if (string.IsNullOrEmpty(email))
      {
        return StatusCode(401, new { Message = "Please provide a email address" });
      }
      if (string.IsNullOrEmpty(email))
      {
        return StatusCode(401, new { Message = "Please provide a email address" });
      }

      if (!email.CheckEmail())
      {
        return StatusCode(401, new { Message = "Please provide a valid email address" });
      }
//      if (string.IsNullOrEmpty(username))
//      {
//        return StatusCode(401, new { Message = "Please provide a username" });
//      }
      if (string.IsNullOrEmpty(password))
      {
        return StatusCode(401, new { Message = "Please provide a password" });
      }
      

      // string role = "Guest";
      // if(string.IsNullOrEmpty(instanceId))
      // {
      //   role = "User";
      // }

      (bool condition, string message, AuthRestModel tokenResponse) result =
        this._authenticationService.CreateUser (name, email, password,role, instanceId, meetingId);
      
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