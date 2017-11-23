using System.Collections.Generic;
using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace Api.Controllers
{
  public class MeetingActionController : Controller
  {
    public MeetingActionController ()
    {

    }

    /// <summary>
    /// Get all agenda items for a meeting
    /// </summary>
    /// <returns>Collection of MeetingAgenda objects</returns>
    [HttpGet ("api/meeting/{referenceId}/action")]
    [Authorize]
    public List<MeetingAction> Get (string referenceId)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new List<MeetingAction> ();
    }

    /// <summary>
    ///  Returns one instance of a meeting agenda
    /// </summary>
    /// <returns>MeetingAgenda object</returns>
    [HttpGet ("api/meeting/{referenceId}/action/{id}")]
    [Authorize]
    public MeetingAction Get (string referenceId, string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new MeetingAction ();
    }

    /// <summary>
    /// Create a meeting Agenda
    /// </summary>
    /// <returns></returns>
    [HttpPut ("api/meeting/{referenceId}/action")]
    [Authorize]
    public MeetingAction Put (MeetingAction action)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new MeetingAction ();
    }

    /// <summary>
    /// Update the Meeting Action
    /// </summary>
    /// <returns></returns>
    [HttpPost ("api/meeting/{referenceId}/action/{id}")]
    [Authorize]
    public MeetingAction Post (MeetingAction action)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new MeetingAction ();
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