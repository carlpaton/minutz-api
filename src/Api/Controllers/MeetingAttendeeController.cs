using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Interface.Services;
using Models.Entities;
using System.Linq;

namespace Api.Controllers
{
  [Route("api/[controller]")]
  public class MeetingAttendeeController : Controller
  {
    private readonly IMeetingService _meetingService;
    public MeetingAttendeeController(IMeetingService meetingService)
    {
      _meetingService = meetingService;
    }
    [HttpGet]
    [Authorize]
    public MeetingAttendee Get()
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return null;
    }
    [HttpGet("{id}")]
    [Authorize]
    public IEnumerable<MeetingAttendee> Get(string id)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return null;
    }

    [HttpPost("invite/{id}")]
    [Authorize]
    public MeetingAttendee InviteAttendee([FromBody] MeetingAttendee attendee){
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return attendee;
    }
  }
}
