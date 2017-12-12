using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Minutz.Models.Entities;
//using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
  public class MeetingAttendeeController : Controller
  {
    private readonly IMeetingService _meetingService;
    private readonly IStartupService _startupService;
    public MeetingAttendeeController(IMeetingService meetingService, IStartupService startupService)
    {
      _meetingService = meetingService;
      _startupService = startupService;
    }

    /// <summary>
    /// Get all a attendees for a meeting
    /// </summary>
    /// <returns>Collection of MeetingAttendee</returns>
    [HttpGet("api/meeting/{referenceId}/attendees")]
    [Authorize]
    public List<MeetingAttendee> Get(string referenceId)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return new List<MeetingAttendee>();
    }

    /// <summary>
    /// Get a meeting attendee for a meeting.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("api/meeting/{referenceId}/attendee/{id}")]
    [Authorize]
    public MeetingAttendee GetMeetingAttendee(string referenceId, string id)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      var result = _meetingService.GetAttendee(token, Guid.Parse(referenceId), Guid.Parse(id));
      return result;
    }

    [HttpPost("api/updateMeetingAttendees")]
    [Authorize]
    public List<MeetingAttendee> UpdateMeetingAttendees(List<MeetingAttendee> attendees)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      var result = _meetingService.UpdateMeetingAttendees(attendees, token);
      return result;
    }

    /// <summary>
    /// Invite Attendee.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /Invite
    ///     {
    ///        "id": 1,
    ///        "name": "Item1",
    ///        "isComplete": true
    ///     }
    ///
    /// </remarks>
    /// <param name="item"></param>
    /// <returns>A newly-created TodoItem</returns>
    /// <response code="201">Returns the newly-created item</response>
    /// <response code="400">If the item is null</response>
    [HttpPut("api/meetingAttendee/invite", Name = "Invite")]
    //[ProducesResponseType(typeof(MeetingAttendee),200)]
    //[SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(MeetingAttendee))]
    [Authorize]
    public IActionResult Invite([FromBody] MeetingAttendee invitee)
    {
      if (string.IsNullOrEmpty(invitee.Email))
      {
        return BadRequest("Please provide a valid email address");
      }
      if (string.IsNullOrEmpty(invitee.Name))
      {
        return BadRequest("Please provide a valid name.");
      }
      System.Guid meetingId;
      if (invitee.ReferenceId == null || !System.Guid.TryParse(invitee.ReferenceId.ToString(), out meetingId))
      {
        return BadRequest("Please provide a valid meetingId");
      }
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      var meeting = _meetingService.GetMeeting(token, invitee.ReferenceId);

      invitee.PersonIdentity = invitee.Email;
      invitee.Role = "invited";
      invitee.Status = "invited";
      var result = _startupService.SendInvitationMessage(invitee, meeting);
      if (result)
      {
        var savedUser = _meetingService.InviteUser(token, invitee);
        if (savedUser)
        {
          return new ObjectResult(invitee);
        }
        return BadRequest("There was a issue saving the invited user.");
      }
      return BadRequest("There was a issue inviting the user.");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attendee"></param>
    /// <returns></returns>
    [HttpPost("api/meeting/{ReferenceId}/attendee/{id}")]
    [Authorize]
    public MeetingAttendee Post([FromBody] MeetingAttendee attendee)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return attendee;
    }

    [HttpPut("api/meeting/{ReferenceId}/attendee")]
    [Authorize]
    public MeetingAttendee Put([FromBody] MeetingAttendee attendee)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return attendee;
    }

    [HttpDelete("api/meeting/{referenceId}/attendee/{id}")]
    [Authorize]
    public bool Delete(string referenceId, string id)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return true;
    }
  }
}