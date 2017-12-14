﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.Extensions;
using Api.Models;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.ViewModels;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace Api.Controllers
{
  public class MeetingController : Controller
  {
    private readonly IMeetingService _meetingService;
    private readonly IInvatationService _invatationService;

    public MeetingController(IMeetingService meetingService,
                             IInvatationService invatationService)
    {
      _meetingService = meetingService;
      _invatationService = invatationService;
    }

    /// <summary>
    /// Get all the meetings for a user, it uses the token that is,
    /// passed to get the instance id to call the correct schema in the database.
    /// </summary>
    /// <returns>Collection of MeetingViewModel objects</returns>
    [Authorize]
    [Produces("application/json")]
    [HttpGet("api/meetings", Name = "Get all meetings for a user")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(List<Minutz.Models.ViewModels.MeetingViewModel>), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(List<Minutz.Models.ViewModels.MeetingViewModel>))]
    public IActionResult GetMeetings()
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return Ok(_meetingService.GetMeetings(token));
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
    [ProducesResponseType(typeof(Minutz.Models.ViewModels.MeetingViewModel), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(Minutz.Models.ViewModels.MeetingViewModel))]
    public IActionResult GetMeeting(string id)
    {
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      return Ok(_meetingService.GetMeeting(token, id));
    }

    /// <summary>
    /// Create a meetingViewModel
    /// </summary>
    /// <param name="meeting"></param>
    /// <returns>The created meetingViewModel object.</returns>
    [Authorize]
    [HttpPut("api/meeting/create", Name = "Create a meeting")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(MeetingViewModel), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(MeetingViewModel))]
    public IActionResult CreateMeeting([FromBody] MeetingViewModel data)
    {
      if (data == null)
      {
        return StatusCode(500);
      }

      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;

      var result = _meetingService.CreateMeeting(token, data.ToEntity(),
                                                 data.MeetingAttendeeCollection,
                                                 data.MeetingAgendaCollection,
                                                 data.MeetingNoteCollection,
                                                 data.MeetingAttachmentCollection,
                                                 data.MeetingActionCollection);
      if (result.Key)
      {
        return new ObjectResult(result.Value);
      }
      return new BadRequestResult();
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
    [ProducesResponseType(typeof(MeetingViewModel), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(MeetingViewModel))]
    public IActionResult UpdateMeeting([FromBody] MeetingViewModel meeting)
    {
      if (meeting == null)
      {
        return StatusCode(500);
      }
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      var result = _meetingService.UpdateMeeting(token, meeting);
      return new ObjectResult(result);
    }

    /// <summary>
    /// Delete a meetingViewModel
    /// </summary>
    /// <param name="id">meetingViewModel identifier.</param>
    /// <returns>True if Successful or False if unsuccessful.</returns>
    [Authorize]
    [HttpDelete("api/meeting/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(string))]
    public IActionResult DeleteMeeting(string id)
    {
      if (string.IsNullOrEmpty(id))
        return BadRequest("Please provide a valid id");
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      var result = _meetingService.DeleteMeeting(token, Guid.Parse(id));
      if (result.Key)
        return Ok(result.Value);
      return BadRequest(result.Value);
    }

    [Authorize]
    [HttpPost("api/sendmeetinginvatations/{meetingId}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(string))]
    public IActionResult SendMeetingInvatations(string meetingId)
    {
      if (string.IsNullOrEmpty(meetingId))
        return BadRequest("Please provide a valid id");
      var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
      var meeting = _meetingService.GetMeeting(token, meetingId);
      foreach (var attendee in meeting.MeetingAttendeeCollection)
      {
        var result = _invatationService.SendMeetingInvatation(attendee, meeting);
      }
      return Ok();
    }

    [HttpPost("api/UploadMeertingFiles")]
    public async Task<IActionResult> Post(List<IFormFile> files)
    {
      long size = files.Sum(f => f.Length);
      string meetingId = HttpContext.Request.Query["id"].ToString();
      // full path to file in temp location
      var filePath = Path.GetTempFileName();

      foreach (var formFile in files)
      {
        if (formFile.Length > 0)
        {
          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await formFile.CopyToAsync(stream);
          }
        }
      }

      // process uploaded files
      // Don't rely on or trust the FileName property without validation.

      return Ok(new { count = files.Count, size, filePath });
    }

  }

}