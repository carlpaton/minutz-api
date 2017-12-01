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
    [HttpGet ("api/meeting")]
    [Authorize]
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
    public IActionResult Get (string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      //Models.ViewModels.Meeting
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
    public IActionResult  Put (Models.ViewModels.Meeting meeting)
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


      return Ok(_meetingService.GetMeeting(token, meeting.Id.ToString()));
      
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
    /// <returns>The saved meeting object.</returns>
    [HttpPost ("api/meeting/{id}")]
    [Authorize]
    public IActionResult Post (Meeting meeting)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return Ok( _meetingService.GetMeeting (token, meeting.Id.ToString()));
    }

    /// <summary>
    /// Delete a meeting
    /// </summary>
    /// <param name="id">meeting identifier.</param>
    /// <returns>True if Successful or False if unsuccessful.</returns>
    [HttpDelete ("api/meeting/{id}")]
    [Authorize]
    public bool Delete (string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return true; //_meetingService.GetMeeting (token, id);
    }
  }
}