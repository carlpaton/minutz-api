using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Api.Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace Api.Controllers
{
  public class MeetingAttachmentsController : Controller
  {
      private readonly IMeetingService _meetingService;
      private readonly ILogService _logService;
      private readonly IAuthenticationService _authenticationService;
      private readonly IMeetingAttachmentService _meetingAttachmentService;
      private readonly ILogger _logger;
      
    public MeetingAttachmentsController(
        IMeetingService meetingService,
        ILogService logService,
        ILoggerFactory logger,
        IAuthenticationService authenticationService,
        IMeetingAttachmentService meetingAttachmentService)
    {
        _meetingService = meetingService;
        _logService = logService;
        _authenticationService = authenticationService;
        _meetingAttachmentService = meetingAttachmentService;
        _logger = logger.CreateLogger("MeetingAttachmentsController");
    }

    /// <summary>
    /// Get all agenda items for a meeting
    /// </summary>
    /// <returns>Collection of MeetingAgenda objects</returns>
     [HttpGet("api/meeting/{referenceId}/attachments")]
     [Authorize]
     public List<MeetingAttachment> Get(string referenceId)
     {
       var userInfo = Request.ExtractAuth(User, _authenticationService);
       return new List<MeetingAttachment>();
     }

    /// <summary>
    ///  Returns one instance of a meeting agenda
    /// </summary>
    /// <returns>MeetingAgenda object</returns>
     [HttpGet("api/meeting/{referenceId}/attachment/{id}")]
     [Authorize]
     public MeetingAttachment Get(string referenceId, string id)
     {
       var userInfo = Request.ExtractAuth(User, _authenticationService);
       return new MeetingAttachment();
     }

    /// <summary>
    /// Create a meeting Agenda
    /// </summary>
    /// <returns></returns>
     [HttpPut("api/meeting/{referenceId}/attachment")]
     [Authorize]
     public MeetingAttachment Put([FromBody] MeetingAttachment attachment)
     {
       var userInfo = Request.ExtractAuth(User, _authenticationService);
       return new MeetingAttachment();
     }

    /// <summary>
    /// Update the MeetingViewModel Agenda
    /// </summary>
    /// <returns></returns>
     [HttpPost("api/meeting/{referenceId}/attachment/{id}")]
     [Authorize]
     public MeetingAttachment Post([FromBody] List<IFormFile> files, string referenceId, string id)
     {
       var userInfo = Request.ExtractAuth(User, _authenticationService);
       return new MeetingAttachment();
     }

    /// <summary>
    /// Delete the single instance of the agenda item.
    /// </summary>
    /// <returns>true if successful and false if failure.</returns>
     [HttpDelete("api/meeting/{referenceId}/attachment/{id}")]
     [Authorize]
     public bool Delete(string referenceId, string id)
     {
       var userInfo = Request.ExtractAuth(User, _authenticationService);
       return true;
     }
  }
}