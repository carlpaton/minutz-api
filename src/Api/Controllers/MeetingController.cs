using System;
using System.Collections.Generic;
using System.Linq;
using Api.Extensions;
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
    /// Get all the meetings for a user, it uses the token that is,
    /// passed to get the instance id to call the correct schema in the database.
    /// </summary>
    /// <returns>Collection of MeetingViewModel objects</returns>
    [Authorize]
    [Produces("application/json")]
    [HttpGet ("api/meetings", Name = "Get all meetings for a user")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(List<Models.ViewModels.MeetingViewModel>), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(List<Models.ViewModels.MeetingViewModel>))]
    public IActionResult GetMeetings ()
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return Ok( _meetingService.GetMeetings (token));
    }

    /// <summary>
    /// Get the meetingViewModel object based on identifier.
    /// </summary>
    /// <param name="id">MeetingViewModel identifier [Guid]</param>
    /// <returns>The meetingViewModel object.</returns>
    [Authorize]
    [HttpGet("api/meeting/{id}", Name = "Get a meeting for a user by id")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(Models.ViewModels.MeetingViewModel), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(Models.ViewModels.MeetingViewModel))]
    public IActionResult GetMeeting (string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return Ok(_meetingService.GetMeeting (token, id));
    }

    /// <summary>
    /// Create a meetingViewModel
    /// </summary>
    /// <param name="meeting"></param>
    /// <returns>The created meetingViewModel object.</returns>
    [Authorize]
    [HttpPut("api/meeting", Name="Create a meeting")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(Models.ViewModels.MeetingViewModel), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(Models.ViewModels.MeetingViewModel))]
    public IActionResult  CreateMeeting ([FromBody] Models.ViewModels.MeetingViewModel meeting)
    {
      if (string.IsNullOrEmpty(meeting.Name))
      {
        return BadRequest("Please provide a meetingViewModel name.");
      }
      _defaultValues(meeting);

      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
 
      var result = _meetingService.CreateMeeting(token, meeting.ToEntity(), meeting.Attendees, meeting.Agenda,
        meeting.Notes, meeting.Attachments, meeting.Actions);
      if (result.Key)
        return Ok(result.Value);

      return BadRequest(result.Value.ResultMessage);
    }

    /// <summary>
    /// update a meetingViewModel
    /// </summary>
    /// <param name="id">meeting identifier.</param>
    /// <param name="meeting"></param>
    /// <returns>The saved meeting object.</returns>
    [Authorize]
    [HttpPost("api/meeting/{id}", Name = "Update Meeting")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(Models.ViewModels.MeetingViewModel), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(Models.ViewModels.MeetingViewModel))]
    public IActionResult UpdateMeeting([FromBody] Models.ViewModels.MeetingViewModel meeting)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      var result = _meetingService.UpdateMeeting(token, meeting);
      return Ok(result);
    }

    /// <summary>
    /// Delete a meetingViewModel
    /// </summary>
    /// <param name="id">meetingViewModel identifier.</param>
    /// <returns>True if Successful or False if unsuccessful.</returns>
    [Authorize]
    [HttpDelete ("api/meeting/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(string))]
    public IActionResult DeleteMeeting (string id)
    {
      if (string.IsNullOrEmpty(id))
        return BadRequest("Please provide a valid id");
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      var result = _meetingService.DeleteMeeting(token, Guid.Parse(id));
      if (result.Key)
        return Ok(result.Value);
      return BadRequest(result.Value);
    }

    internal static void _defaultValues(Models.ViewModels.MeetingViewModel meetingViewModel)
    {
      if (meetingViewModel.Id == Guid.Empty)
      {
        meetingViewModel.Id = Guid.NewGuid();
      }
      if (meetingViewModel.Agenda == null)
        meetingViewModel.Agenda = new List<MeetingAgenda>();

      if (meetingViewModel.Attendees == null)
        meetingViewModel.Attendees = new List<MeetingAttendee>();

      if (meetingViewModel.AvailibleAttendees == null)
        meetingViewModel.AvailibleAttendees = new List<MeetingAttendee>();

      if (meetingViewModel.Notes == null)
        meetingViewModel.Notes = new List<MeetingNote>();

      if (meetingViewModel.Attachments == null)
        meetingViewModel.Attachments = new List<MeetingAttachment>();
    }

  }
}