using System.Collections.Generic;
using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace Api.Controllers
{
  public class MeetingAgendaController : Controller
  {
    private readonly IMeetingService _meetingService;
    public MeetingAgendaController (IMeetingService meetingService)
    {
      _meetingService = meetingService;
    }

    /// <summary>
    /// Get all agenda items for a meeting
    /// </summary>
    /// <returns>Collection of MeetingAgenda objects</returns>
    [HttpGet ("api/meeting/{ReferenceId}/agendas")]
    [Authorize]
    public List<MeetingAgenda> Get (string ReferenceId)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new List<MeetingAgenda> ();
    }

    /// <summary>
    ///  Returns one instance of a meeting agenda
    /// </summary>
    /// <returns>MeetingAgenda object</returns>
    [HttpGet ("api/meeting/{ReferenceId}/agenda/{id}")]
    [Authorize]
    public List<MeetingAgenda> Get (string ReferenceId, string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return null;
    }

    /// <summary>
    /// Create a meeting Agenda
    /// </summary>
    /// <returns></returns>
    [HttpPut ("api/meeting/{ReferenceId}/agenda")]
    [Authorize]
    public MeetingAgenda Put ([FromBody] MeetingAgenda agenda)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new MeetingAgenda ();
    }

    /// <summary>
    /// Update the Meeting Agenda
    /// </summary>
    /// <returns></returns>
    [HttpPost ("api/meeting/{ReferenceId}/agenda/{id}")]
    [Authorize]
    public MeetingAgenda Post ([FromBody] MeetingAgenda agenda)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new MeetingAgenda ();
    }

    /// <summary>
    /// Delete the single instance of the agenda item.
    /// </summary>
    /// <returns>true if successful and false if failure.</returns>
    [HttpDelete ("api/meeting/{ReferenceId}/agenda/{id}")]
    [Authorize]
    public bool Delete ([FromRoute] string ReferenceId, string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return true;
    }
  }
}