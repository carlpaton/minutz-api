using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Interface.Services;
using Models.Entities;
using System.Linq;

namespace Api.Controllers
{
  [Route("api/[controller]")]
  public class MeetingAgendaController : Controller
  {
    private readonly IMeetingService _meetingService;
    public MeetingAgendaController(IMeetingService meetingService)
    {
      _meetingService = meetingService;
    }
    [HttpGet]
    [Authorize]
    public MeetingAgenda Get()
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return null;
    }
    [HttpGet]
    [Authorize]
    public IEnumerable<MeetingAgenda> Get(string id)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return null;
    }
  }
}
