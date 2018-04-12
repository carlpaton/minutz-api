using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Minutz.Models.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
  public class MeetingAttendeeController : Controller
  {
    private readonly IMeetingService _meetingService;
    private readonly IInvatationService _invationService;
    private readonly IAuthenticationService _authenticationService;

    public MeetingAttendeeController(
      IMeetingService meetingService, IInvatationService invatationService, IAuthenticationService authenticationService)
    {
      _meetingService = meetingService;
      _invationService = invatationService;
      _authenticationService = authenticationService;
    }

    /// <summary>
    /// Get all a attendees for a meeting
    /// </summary>
    /// <returns>Collection of MeetingAttendee</returns>
     [HttpGet("api/meeting/{referenceId}/attendees")]
     [Authorize]
     public List<MeetingAttendee> Get(string referenceId)
     {
       var userInfo = ExtractAuth();
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
       var userInfo = ExtractAuth();
       var result = _meetingService.GetAttendee(userInfo.infoResponse, Guid.Parse(referenceId), Guid.Parse(id));
       return result;
     }

     [HttpPost("api/updateMeetingAttendees")]
     [Authorize]
     public List<MeetingAttendee> UpdateMeetingAttendees(List<MeetingAttendee> attendees)
     {
       var userInfo = ExtractAuth();
       var result = _meetingService.UpdateMeetingAttendees(attendees, userInfo.infoResponse);
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
    [ProducesResponseType(typeof(MeetingAttendee),200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(MeetingAttendee))]
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
       var userInfo = ExtractAuth();
       var meeting = _meetingService.GetMeeting(userInfo.infoResponse, invitee.ReferenceId.ToString());
       //var instance = _meetingService.GetInstance(token);
       invitee.PersonIdentity = invitee.Email;
       
       invitee.Role = "Invited";
       invitee.Status = "Invited";
       var result = _invationService.SendMeetingInvatation(invitee, meeting,userInfo.infoResponse.InstanceId);
       if (result)
       {
         var savedUser = _meetingService.InviteUser(userInfo.infoResponse, invitee,meeting.Id,invitee.Email);
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
       var userInfo = ExtractAuth();
       return attendee;
     }

     [HttpPut("api/meeting/{ReferenceId}/attendee")]
     [Authorize]
     public MeetingAttendee Put([FromBody] MeetingAttendee attendee)
     {
       var userInfo = ExtractAuth();
       return attendee;
     }

     [HttpDelete("api/meeting/{referenceId}/attendee/{id}")]
     [Authorize]
     public bool Delete(string referenceId, string id)
     {
       var userInfo = ExtractAuth();
       return true;
     }
    
    private (bool condition, string message, AuthRestModel infoResponse) ExtractAuth()
    {
      (bool condition, string message, AuthRestModel infoResponse) userInfo =
        _authenticationService.LoginFromFromToken(
          Request.Headers.First(i => i.Key == "access_token").Value,
          Request.Headers.First(i => i.Key == "Authorization").Value,
          User.Claims.ToList().First(i => i.Type == "exp").Value, "");
      return userInfo;
    }
  }
}