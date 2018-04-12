using System;
using System.Collections.Generic;
using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
  public class MeetingActionController : Controller
  {
    private readonly IMeetingActionService _meetingActionService;
    private readonly IAuthenticationService _authenticationService;

    public MeetingActionController(
      IMeetingActionService meetingActionService, IAuthenticationService authenticationService)
    {
      _meetingActionService = meetingActionService;
      _authenticationService = authenticationService;
    }

    /// <summary>
    /// Get all agenda items for a meeting
    /// </summary>
    /// <returns>Collection of MeetingAgenda objects</returns>
     [Authorize]
     [ProducesResponseType(typeof(string), 400)]
     [ProducesResponseType(typeof(List<MinutzAction>), 200)]
     [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(List<MinutzAction>))]
     [HttpGet("api/meetingActions/{referenceId}", Name = "Get meeting actions")]
     public IActionResult Get(string referenceId)
     {
       if (string.IsNullOrEmpty(referenceId))
       {
         return BadRequest("Please provide a valid referenceId [meeting id]");
       }
       var userInfo = ExtractAuth();
       var result = _meetingActionService.GetMinutzActions(referenceId, userInfo.infoResponse);
       return Ok(result);
     }

    /// <summary>
    /// Create a meeting Agenda
    /// </summary>
    /// <returns></returns>
     [HttpPut("api/meetingAction/{referenceId}/action")]
     [Authorize]
     public IActionResult Put([FromBody] MinutzAction action)
     {
       var userInfo = ExtractAuth();
       var result =
         _meetingActionService.CreateMinutzAction(action.ReferanceId.ToString(), action, userInfo.infoResponse);
       return result.condition ? Ok(action) : StatusCode(500 ,result.message);
     }

    /// <summary>
    /// Update the MeetingViewModel Action
    /// </summary>
    /// <returns></returns>
     [HttpPost("api/meetingAction/{referenceId}/action/{id}")]
     [Authorize]
     public IActionResult Post([FromBody] MinutzAction action)
     {
       var userInfo = ExtractAuth();
       var result = _meetingActionService.UpdateMinutzAction(action.ReferanceId.ToString(), action, userInfo.infoResponse);
       return result.condition ? Ok(action) : StatusCode(500, result.message);
     }

    /// <summary>
    /// Delete the single instance of the action item.
    /// </summary>
    /// <returns>true if successful and false if failure.</returns>
     [HttpDelete("api/meetingAction/{referenceId}/action/{id}")]
     [Authorize]
     public bool Delete(string referenceId, string id)
     {
       var userInfo = ExtractAuth();
       return true;
     }
    
    private (bool condition, string message, AuthRestModel infoResponse) ExtractAuth()
    {
      (bool condition, string message, AuthRestModel infoResponse) userInfo =
        _authenticationService.LoginFromFromToken(
          Request.Headers.First(i => i.Key == "access_token").Value,
          Request.Headers.First(i => i.Key == "Authorization").Value,
          User.Claims.ToList().First(i => i.Type == "exp").Value, "");
      return userInfo;
    }
  }
}