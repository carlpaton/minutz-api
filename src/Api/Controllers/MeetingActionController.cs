using System.Collections.Generic;
using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
  public class MeetingActionController : Controller
  {
    private readonly IMeetingService _meetingService;
    public MeetingActionController (IMeetingService meetingService)
    {
      _meetingService = meetingService;
    }

    /// <summary>
    /// Get all agenda items for a meeting
    /// </summary>
    /// <returns>Collection of MeetingAgenda objects</returns>
    [Authorize]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(List<MinutzAction>), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(List<MinutzAction>))]
    [HttpGet("api/meeting/{referenceId}/actions", Name = "Get Machine by Id")]
    public IActionResult Get (string referenceId)
    {
      if (string.IsNullOrEmpty(referenceId))
      {
        return BadRequest("Please provide a valid referenceId [meeting id]");
      }
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return Ok(new List<MinutzAction> ());
    }

    /// <summary>
    ///  Returns one instance of a meeting agenda
    /// </summary>
    /// <returns>MeetingAgenda object</returns>
    [HttpGet ("api/meeting/{referenceId}/action/{id}")]
    [Authorize]
    public MinutzAction Get (string referenceId, string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new MinutzAction ();
    }

    /// <summary>
    /// Create a meeting Agenda
    /// </summary>
    /// <returns></returns>
    [HttpPut ("api/meeting/{referenceId}/action")]
    [Authorize]
    public MinutzAction Put (MinutzAction action)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new MinutzAction ();
    }

    /// <summary>
    /// Update the Meeting Action
    /// </summary>
    /// <returns></returns>
    [HttpPost ("api/meeting/{referenceId}/action/{id}")]
    [Authorize]
    public MinutzAction Post (MinutzAction action)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new MinutzAction ();
    }

    /// <summary>
    /// Delete the single instance of the action item.
    /// </summary>
    /// <returns>true if successful and false if failure.</returns>
    [HttpDelete ("api/meeting/{referenceId}/action/{id}")]
    [Authorize]
    public bool Delete (string referenceId, string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return true;
    }
  }
}