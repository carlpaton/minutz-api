using System.Collections.Generic;
using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;

namespace Api.Controllers
{
  public class MeetingAgendaController : Controller
  {
    private readonly IMeetingService _meetingService;
    public MeetingAgendaController(IMeetingService meetingService)
    {
      _meetingService = meetingService;
    }

    /// <summary>
    /// Get all agenda items for a meeting
    /// </summary>
    /// <returns>Collection of MeetingAgenda objects</returns>
    [HttpGet("api/meetingAgendaItems/{referenceId}")]
    [Authorize]
    public List<MeetingAgenda> GetMeetingAgendaItems(string referenceId)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return new List<MeetingAgenda>();
    }

    /// <summary>
    ///  Returns one instance of a meeting agenda
    /// </summary>
    /// <returns>MeetingAgenda object</returns>
    [HttpGet("api/meetingAgenda/{id}")]
    [Authorize]
    public List<MeetingAgenda> GetAgendaItem(string id)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return null;
    }

    /// <summary>
    /// Create a meeting Agenda
    /// </summary>
    /// <returns></returns>
    [HttpPut("api/meetingAgenda")]
    [Authorize]
    public MeetingAgenda Put([FromBody] MeetingAgenda agenda)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return new MeetingAgenda();
    }

    /// <summary>
    /// Update the MeetingViewModel Agenda
    /// </summary>
    /// <returns></returns>
    [HttpPost("api/meetingAgenda")]
    [Authorize]
    public MeetingAgenda Post([FromBody] MeetingAgenda agenda)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return new MeetingAgenda();
    }

    /// <summary>
    /// Delete the single instance of the agenda item.
    /// </summary>
    /// <returns>true if successful and false if failure.</returns>
    [HttpDelete("api/meetingAgenda/{id}")]
    [Authorize]
    public bool Delete(string id)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return true;
    }
  }
}