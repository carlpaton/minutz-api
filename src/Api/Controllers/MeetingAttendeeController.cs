using System.Collections.Generic;
using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace Api.Controllers
{
  public class MeetingAttendeeController : Controller
  {
    private readonly IMeetingService _meetingService;
    public MeetingAttendeeController (IMeetingService meetingService)
    {
      _meetingService = meetingService;
    }

    /// <summary>
    /// Get all a attendees for a meeting
    /// </summary>
    /// <returns>Collection of MeetingAttendee</returns>
    [HttpGet("api/meeting/{referenceId}/attendees")]
    [Authorize]
    public List<MeetingAttendee> Get (string referenceId)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new List<MeetingAttendee>();
    }

    /// <summary>
    /// Get a meeting attendee for a meeting.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet ("api/meeting/{referenceId}/attendee/{id}")]
    [Authorize]
    public MeetingAttendee Get (string referenceId, string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new MeetingAttendee();
    }

    /// <summary>
    /// Get a meeting attendee for a meeting.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut ("api/meeting/{referenceId}/invite")]
    [Authorize]
    public MeetingAttendee Invite (MeetingAttendee invitee)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return new MeetingAttendee();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attendee"></param>
    /// <returns></returns>
    [HttpPost ("api/meeting/{ReferenceId}/attendee/{id}")]
    [Authorize]
    public MeetingAttendee Post (MeetingAttendee attendee)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return attendee;
    }

    [HttpPut ("api/meeting/{ReferenceId}/attendee")]
    [Authorize]
    public MeetingAttendee Put (MeetingAttendee attendee)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return attendee;
    }

    [HttpDelete ("api/meeting/{ReferenceId}/attendee/{id}")]
    [Authorize]
    public bool Delete (MeetingAttendee attendee)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return true;
    }
  }
}