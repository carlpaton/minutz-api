using System.Collections.Generic;
using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace Api.Controllers
{
  [Route ("api/[controller]")]
  public class MeetingAgendaController : Controller
  {
    private readonly IMeetingService _meetingService;
    public MeetingAgendaController (IMeetingService meetingService)
    {
      _meetingService = meetingService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public List<MeetingAgenda> Get ()
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new List<MeetingAgenda>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet ("{id}")]
    [Authorize]
    public IEnumerable<MeetingAgenda> Get (string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    [Authorize]
    public MeetingAgenda Put (MeetingAgenda agenda)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new MeetingAgenda();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public MeetingAgenda Post (MeetingAgenda agenda)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return null;
    }

    /// <summary>
    /// Delete the single instance of the agenda item.
    /// </summary>
    /// <returns>true if successful and false if failure.</returns>
    [HttpDelete("{id}")]
    [Authorize]
    public bool Delete (string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return true;
    }
  }
}