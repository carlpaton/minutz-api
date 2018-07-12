using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Api.Extensions;
using Api.Models;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minutz.Models.Entities;
using Minutz.Models.ViewModels;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
  public class MeetingController : Controller
  {
    private readonly IMeetingService _meetingService;
    private readonly IInstanceService _invatationService;
    private readonly ILogService _logService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IMeetingAttachmentService _meetingAttachmentService;
    private readonly ILogger _logger;

    public MeetingController(
      IMeetingService meetingService,
      IInstanceService invatationService,
      ILogService logService,
      ILoggerFactory logger,
      IAuthenticationService authenticationService,
      IMeetingAttachmentService meetingAttachmentService)
    {
      _meetingService = meetingService;
      _invatationService = invatationService;
      _logService = logService;
      _authenticationService = authenticationService;
      _meetingAttachmentService = meetingAttachmentService;
      _logger = logger.CreateLogger("MeetingController");
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
    [ProducesResponseType(typeof(List<MeetingViewModel>), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(List<MeetingViewModel>))]
    public IActionResult GetMeetings
      (string reference)
    {
      if (!string.IsNullOrEmpty(reference))
      {
        if (reference.ToLower() == "none") reference = string.Empty;
      }

      _logger.LogInformation(Core.LogProvider.LoggingEvents.ListItems, "GetMeetings {ID}", 1);
      _logService.Log(Minutz.Models.LogLevel.Info, "GetMeetings called.");
      var userInfo = User.ToRest(); //Request.ExtractAuth(User, _authenticationService);
      var meetingsResult = _meetingService.GetMeetings(userInfo, reference);
      if (meetingsResult.condition == true && meetingsResult.statusCode == 200)
        return Ok(meetingsResult.value);
      if (meetingsResult.condition == true && meetingsResult.statusCode == 404)
        return Ok(meetingsResult.value);
      _logger.LogInformation("Get all meetings called.");
      return StatusCode(meetingsResult.statusCode, meetingsResult.message);
    }



    /// <summary>
    /// Get the meetingViewModel object based on identifier.
    /// </summary>
    /// <param name="id">MeetingViewModel identifier [Guid]</param>
    /// <returns>The meetingViewModel object.</returns>
    [Authorize]
    [HttpGet("api/meeting", Name = "Get a meeting for a user by id")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(MeetingViewModel), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(MeetingViewModel))]
    public IActionResult GetMeeting
      (string id, string related)
    {
      var userInfo = User.ToRest(); // Request.ExtractAuth(User, _authenticationService);
      var meeting = _meetingService.GetMeeting(userInfo, id);
      if (meeting == null)
      {
        return StatusCode(404);
      }
      return Ok(meeting);
    }

    /// <summary>
    /// Create a meetingViewModel, id is the acces_token and the instanceId is the related instance that the user selected
    /// </summary>
    /// <returns>The created meetingViewModel object.</returns>
    [Authorize]
    [HttpPut("api/meeting/create", Name = "Create a meeting")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(MeetingViewModel), 200)]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(MeetingViewModel))]
    public IActionResult CreateMeeting()
    {
      
      var userInfo = User.ToRest();

      _logger.LogInformation(Core.LogProvider.LoggingEvents.InsertItem, "CreateMeeting - entry point {ID}", 1);
      
      _logger.LogInformation(Core.LogProvider.LoggingEvents.InsertItem, "CreateMeeting - token {ID}", 1);
      var data = new MeetingViewModel
      {
        Id = Guid.NewGuid().ToString(),
        AvailableAttendeeCollection = new List<MeetingAttendee>(),
        Date = DateTime.UtcNow,
        Duration = 60,
        IsFormal = false,
        IsLocked = false,
        IsPrivate = true,
        Time = $"{DateTime.UtcNow.Hour.ToString()}:00",
        TimeZoneOffSet = 2,
        IsRecurrence = false,
        Location = "Durban",
        Status =  "create",
        MeetingActionCollection = new List<MinutzAction>(),
        MeetingAgendaCollection = new List<MeetingAgenda>(),
        MeetingAttachmentCollection = new List<MeetingAttachment>(),
        MeetingAttendeeCollection = new List<MeetingAttendee>(),
        MeetingNoteCollection = new List<MeetingNote>(),
        MeetingDecisionCollection = new List<MinutzDecision>(),
        Outcome = string.Empty,
        Purpose = string.Empty,
        RecurrenceType = 0,
        UpdatedDate = DateTime.UtcNow
      };

      _logger.LogInformation(Core.LogProvider.LoggingEvents.InsertItem, "CreateMeeting - created viewmodel {ID}", 1);
      
      var result = _meetingService.CreateMeeting(
        userInfo,
        data.ToEntity(),
        data.MeetingAttendeeCollection,
        data.MeetingAgendaCollection,
        data.MeetingNoteCollection,
        data.MeetingAttachmentCollection,
        data.MeetingActionCollection, userInfo.InstanceId);
      result.Value.Status = "create";
      if (result.Key)
      {
        return new OkObjectResult(result.Value);
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
    public IActionResult UpdateMeeting
      ([FromBody] MeetingViewModel meeting)
    {
      if (meeting == null)
      {
        return StatusCode(500);
      }
      foreach (var agenda in meeting.MeetingAgendaCollection)
      {
        if (agenda.Id == Guid.Parse("e38b69b3-8f2a-4979-9323-1819db4331f8"))
        {
          agenda.Id = Guid.NewGuid();
        }
      }
      
      var userInfo = User.ToRest(); //Request.ExtractAuth(User, _authenticationService);
      if (string.IsNullOrEmpty(meeting.Status))
      {
        meeting.Status = "edit";
      }

      _logger.LogInformation(Core.LogProvider.LoggingEvents.InsertItem, "UpdateMeeting - entry point {ID}", 1);

      var result = _meetingService.UpdateMeeting(userInfo, meeting);
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
    public IActionResult DeleteMeeting
      (string id)
    {
      if (string.IsNullOrEmpty(id))
        return BadRequest("Please provide a valid id");
      var userInfo = User.ToRest(); //Request.ExtractAuth(User, _authenticationService);
      var result = _meetingService.DeleteMeeting(userInfo, Guid.Parse(id));
      if (result.Key)
        return Ok(result.Value);
      return BadRequest(result.Value);
    }

    [Authorize]
    [HttpPost("api/sendmeetinginvatations")]
    public IActionResult SendMeetingInvatations([FromBody]MeetingInviteModel model)
    {
      if (string.IsNullOrEmpty(model.meetingId))
        return BadRequest("Please provide a valid id");
      var userInfo = User.ToRest(); //Request.ExtractAuth(User, _authenticationService);
      var meeting = _meetingService.GetMeeting(userInfo, model.meetingId);
//      switch (model.recipients)
//      {
          //case InviteAttendees.allAttendess:
            foreach (var attendee in meeting.MeetingAttendeeCollection)
            {
              // var result = _invatationService.SendMeetingInvatation(attendee, meeting, "instanceId");
            }
            return Ok();
          //case InviteAttendees.custom:
//            foreach (var attendee in model.customRecipients)
//            {
//              var personResult = _authenticationService.GetPersonByEmail(attendee);
//              if (!personResult.Condition) continue;
//              var meetingAttendee = new MeetingAttendee
//                                    {
//                                      Email = attendee,
//                                      Name = personResult.Person.FullName
//                                    };
//              var result = _invatationService.SendMeetingInvatation(meetingAttendee, meeting, "instanceId");
//            }
//            return Ok();
//          case InviteAttendees.newAttendees:
//            foreach (var attendee in meeting.MeetingAttendeeCollection)
//            {
//              var result = _invatationService.SendMeetingInvatation(attendee, meeting, "instanceId");
//            }
//            return Ok();
//      }      
//      return Ok();
    }

    [Authorize]
    [HttpPost("api/UploadMeetingFiles")]
    public ActionResult Post(List<IFormFile> files)
    {
      if (string.IsNullOrEmpty(HttpContext.Request.Query["id"].ToString()))
      {
        return StatusCode(404, "Please provide a referenceId");
      }
      long size = files.Sum(f => f.Length);
      string meetingId = HttpContext.Request.Query["id"].ToString();
      var userInfo = User.ToRest(); //Request.ExtractAuth(User, _authenticationService);
      // full path to file in temp location
      var filePath = Path.GetTempFileName();

      foreach (var formFile in Request.Form.Files)
      {
        if (formFile.Length <= 0) continue;
        using (var ms = new MemoryStream())
        {
           formFile.CopyTo(ms);
          var fileBytes =  ms.ToArray();
          //string file = Convert.ToBase64String(fileBytes);
          var meetingFile = new MeetingAttachment
          {
            Date = DateTime.UtcNow,
            FileData = fileBytes,
            FileName = formFile.FileName,
            Id = Guid.NewGuid(),
            MeetingAttendeeId = userInfo.Sub,
            ReferanceId =  Guid.Parse(meetingId)
          };
          var meetingAttachmentResult = _meetingAttachmentService.Add(meetingFile, userInfo);
        }
      }

      // process uploaded files
      // Don't rely on or trust the FileName property without validation.

      return Ok(new { count = files.Count, size, filePath });
      }
    
  }
}