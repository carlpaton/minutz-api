using System.Collections.Generic;
using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace Api.Controllers
{
  public class MeetingAttachmentsController : Controller
  {
    public MeetingAttachmentsController ()
    {

    }

    /// <summary>
    /// Get all agenda items for a meeting
    /// </summary>
    /// <returns>Collection of MeetingAgenda objects</returns>
    [HttpGet ("api/meeting/{referenceId}/attachments")]
    [Authorize]
    public List<MeetingAttachment> Get (string referenceId)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new List<MeetingAttachment> ();
    }

    /// <summary>
    ///  Returns one instance of a meeting agenda
    /// </summary>
    /// <returns>MeetingAgenda object</returns>
    [HttpGet ("api/meeting/{referenceId}/attachment/{id}")]
    [Authorize]
    public MeetingAttachment Get (string referenceId, string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new MeetingAttachment ();
    }

    /// <summary>
    /// Create a meeting Agenda
    /// </summary>
    /// <returns></returns>
    [HttpPut ("api/meeting/{referenceId}/attachment")]
    [Authorize]
    public MeetingAttachment Put (MeetingAttachment attachment)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new MeetingAttachment ();
    }

    /// <summary>
    /// Update the Meeting Agenda
    /// </summary>
    /// <returns></returns>
    [HttpPost ("api/meeting/{referenceId}/attachment/{id}")]
    [Authorize]
    public MeetingAttachment Post (MeetingAttachment attachment)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new MeetingAttachment ();
    }

    /// <summary>
    /// Delete the single instance of the agenda item.
    /// </summary>
    /// <returns>true if successful and false if failure.</returns>
    [HttpDelete ("api/meeting/{referenceId}/attachment/{id}")]
    [Authorize]
    public bool Delete (string referenceId, string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return true;
    }
  }
}