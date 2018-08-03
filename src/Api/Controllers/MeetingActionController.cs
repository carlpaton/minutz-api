using System.Collections.Generic;
using Api.Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
  public class MeetingActionController : Controller
  {
    private readonly IMeetingActionService _meetingActionService;
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger _logger;
    private readonly ILogService _logService;

    public MeetingActionController(
      IMeetingActionService meetingActionService,
      IAuthenticationService authenticationService,
      ILogService logService,
      ILoggerFactory logger)
    {
      _meetingActionService = meetingActionService;
      _authenticationService = authenticationService;
      _logService = logService;
      _logger = logger.CreateLogger("MeetingActionController");
    }

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
       var userInfo = Request.ExtractAuth(User, _authenticationService);
       var result = _meetingActionService.GetMinutzActions(referenceId, userInfo.InfoResponse);
       return Ok(result);
     }


     [HttpPut("api/meetingAction/{referenceId}/action")]
     [Authorize]
     public IActionResult Create([FromBody] MinutzAction action)
     {
       _logger.LogInformation(Core.LogProvider.LoggingEvents.ListItems, "GetMeetings {ID}", 1);
       _logService.Log(Minutz.Models.LogLevel.Info, "GetMeetings called.");
       var userInfo = Request.ExtractAuth(User, _authenticationService);
       var result = _meetingActionService.CreateMinutzAction
         (action.referanceId.ToString(), action, userInfo.InfoResponse);
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
       var userInfo = Request.ExtractAuth(User, _authenticationService);
       var result = _meetingActionService.UpdateMinutzAction(action.referanceId.ToString(), action, userInfo.InfoResponse);
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
       var userInfo = Request.ExtractAuth(User, _authenticationService);
       return true;
     }
  }
}