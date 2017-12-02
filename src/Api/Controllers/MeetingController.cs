using System;
using System.Collections.Generic;
using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
  public class MeetingController : Controller
  {
    private readonly IMeetingService _meetingService;
    public MeetingController (IMeetingService meetingService)
    {
      _meetingService = meetingService;
    }

    /// <summary>
    /// Get all the meetings for a user
    /// </summary>
    /// <returns>Collection of Meeting objects</returns>
    [Authorize]
    [HttpGet ("api/meeting", Name = "Get all meetings for a user")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(List<Models.ViewModels.Meeting>), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(List<Models.ViewModels.Meeting>))]
    public IActionResult Get ()
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return Ok( _meetingService.GetMeetings (token));
    }

    /// <summary>
    /// Get the meeting object based on identifier.
    /// </summary>
    /// <param name="id">Meeting identifier [Guid]</param>
    /// <returns>The meeting object.</returns>
    [HttpGet ("api/meeting/{id}")]
    [Authorize]
    [HttpGet("api/meeting", Name = "Get 1 meeting for a user by id")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(Models.ViewModels.Meeting), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(Models.ViewModels.Meeting))]
    public IActionResult Get (string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return Ok(_meetingService.GetMeeting (token, id));
    }

    /// <summary>
    /// Create a meeting
    /// </summary>
    /// <param name="meeting"></param>
    /// <returns>The created meeting object.</returns>
    [Authorize]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(Models.ViewModels.Meeting), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(Models.ViewModels.Meeting))]
    [HttpPut("api/meeting")]
    public IActionResult  Put ([FromBody] Models.ViewModels.Meeting meeting)
    {
      if (string.IsNullOrEmpty(meeting.Name))
      {
        return BadRequest("Please provide a meeting name.");
      }
      _defaultValues(meeting);

      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      var meetingEntitie = new Models.Entities.Meeting
      {
        Id = meeting.Id,
        Name = meeting.Name,
        Date = meeting.Date,
        Duration = meeting.Duration,
        IsFormal = meeting.IsFormal,
        IsLocked = meeting.IsLocked,
        IsPrivate = meeting.IsPrivate,
        IsReacurance = meeting.IsReacurance,
        MeetingOwnerId = meeting.MeetingOwnerId,
        Outcome = meeting.Outcome,
        Purpose = meeting.Purpose,
        ReacuranceType = meeting.ReacuranceType,
        Tag = meeting.Tag,
        Time = meeting.Time,
        TimeZone = meeting.TimeZone,
        UpdatedDate = DateTime.UtcNow
      };
      var result = _meetingService.CreateMeeting(token, meetingEntitie, meeting.Attendees, meeting.Agenda,
        meeting.Notes, meeting.Attachments, meeting.Actions);
      if (result.Key)
        return Ok(result.Value);

      return BadRequest(result.Value.ResultMessage);
    }

    internal static void _defaultValues(Models.ViewModels.Meeting meeting)
    {
      if (meeting.Id == Guid.Empty)
      {
        meeting.Id = Guid.NewGuid();
      }
      if (meeting.Agenda == null)
        meeting.Agenda = new List<MeetingAgenda>();

      if (meeting.Attendees == null)
        meeting.Attendees = new List<MeetingAttendee>();

      if (meeting.AvailibleAttendees == null)
        meeting.AvailibleAttendees = new List<MeetingAttendee>();

      if (meeting.Notes == null)
        meeting.Notes = new List<MeetingNote>();

      if (meeting.Attachments == null)
        meeting.Attachments = new List<MeetingAttachment>();
    }

    /// <summary>
    /// update a meeting
    /// </summary>
    /// <param name="id">meeting identifier.</param>
    /// <param name="meeting"></param>
    /// <returns>The saved meeting object.</returns>
    [HttpPost ("api/meeting/{id}")]
    [Authorize]
    public IActionResult Post ([FromBody] Models.ViewModels.Meeting meeting)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      var result = _meetingService.UpdateMeeting(token, meeting);
      return Ok( result);
    }

    /// <summary>
    /// Delete a meeting
    /// </summary>
    /// <param name="id">meeting identifier.</param>
    /// <returns>True if Successful or False if unsuccessful.</returns>
    [HttpDelete ("api/meeting/{id}")]
    [Authorize]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(string))]
    public IActionResult Delete (string id)
    {
      if (string.IsNullOrEmpty(id))
        return BadRequest("Please provide a valid id");
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      var result = _meetingService.DeleteMeeting(token, Guid.Parse(id));
      if (result.Key)
        return Ok(result.Value);
      return BadRequest(result.Value);
    }
  }
}