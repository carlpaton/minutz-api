using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Interface.Services;
using Models.Entities;
using System.Linq;

namespace Api.Controllers
{
  [Route("api/[controller]")]
  public class MeetingController : Controller
  {
    private readonly IMeetingService _meetingService;
    public MeetingController(IMeetingService meetingService)
    {
      _meetingService = meetingService;
    }
    [HttpGet]
    [Authorize]
    public IEnumerable<Meeting> Get()
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return _meetingService.GetMeetings(token);
    }
    [HttpGet]
    [Authorize]
    public Meeting Get(string id)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return _meetingService.GetMeeting(token, id);
    }
  }
}
