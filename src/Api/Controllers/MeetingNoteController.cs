using System.Collections.Generic;
using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;

namespace Api.Controllers
{
  public class MeetingNoteController : Controller
  {
    public MeetingNoteController()
    {

    }

    /// <summary>
    /// Get all agenda items for a meeting
    /// </summary>
    /// <returns>Collection of MeetingNote objects</returns>
    [HttpGet("api/meeting/{referenceId}/note")]
    [Authorize]
    public List<MeetingNote> Get(string referenceId)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return new List<MeetingNote>();
    }

    /// <summary>
    ///  Returns one instance of a meeting agenda
    /// </summary>
    /// <returns>MeetingAgenda object</returns>
    [HttpGet("api/meeting/{referenceId}/note/{id}")]
    [Authorize]
    public MeetingNote Get(string referenceId, string id)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return new MeetingNote();
    }

    /// <summary>
    /// Create a meeting note
    /// </summary>
    /// <returns></returns>
    [HttpPut("api/meeting/{referenceId}/note")]
    [Authorize]
    public MeetingNote Put([FromBody] MeetingNote note)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return new MeetingNote();
    }

    /// <summary>
    /// Update the MeetingViewModel note
    /// </summary>
    /// <returns></returns>
    [HttpPost("api/meeting/{referenceId}/note/{id}")]
    [Authorize]
    public MeetingNote Post([FromBody] MeetingNote note)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return new MeetingNote();
    }

    /// <summary>
    /// Delete the single instance of the action item.
    /// </summary>
    /// <returns>true if successful and false if failure.</returns>
    [HttpDelete("api/meeting/{referenceId}/note/{id}")]
    [Authorize]
    public bool Delete(string referenceId, string id)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return true;
    }
  }
}