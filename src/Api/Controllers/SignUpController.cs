using System;
using System.Linq;
using Api.Extensions;
using Api.Models;
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
    public IActionResult Post ([FromBody] CreateUserModel user)
    {
    
      _logService.Log(LogLevel.Info, JsonConvert.SerializeObject(user));

      if (string.IsNullOrEmpty(user.email))
      {
        return StatusCode(401, new { Message = "Please provide a email address" });
      }
      if (string.IsNullOrEmpty(user.email))
      {
        return StatusCode(401, new { Message = "Please provide a email address" });
      }

      if (!user.email.CheckEmail())
      {
        return StatusCode(401, new { Message = "Please provide a valid email address" });
      }
//      if (string.IsNullOrEmpty(username))
//      {
//        return StatusCode(401, new { Message = "Please provide a username" });
//      }
      if (string.IsNullOrEmpty(user.password))
      {
        return StatusCode(401, new { Message = "Please provide a password" });
      }
      

      // string role = "Guest";
      // if(string.IsNullOrEmpty(instanceId))
      // {
      //   role = "User";
      // }

      (bool condition, string message, AuthRestModel tokenResponse) result =
        _authenticationService.CreateUser (user.name, user.email, user.password, user.role, user.RefInstanceId, user.refMeetingId);
      
      _logService.Log(LogLevel.Info, JsonConvert.SerializeObject(result));
      _logService.Log(LogLevel.Info, JsonConvert.SerializeObject(result.tokenResponse));
      
      if (result.condition)
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