using System;
using Api.Extensions;
using Api.Models;
using Interface.Services.Feature.Meeting.Attachment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Feature.Meeting.Attachment
{
    public class MeetingAttachmentController : Controller
    {
        private readonly IMinutzMeetingAttachmentService _attachmentService;

        public MeetingAttachmentController(IMinutzMeetingAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [Authorize]
        [HttpGet("api/feature/attachments", Name = "Get the  meeting attachments")]
        public IActionResult GetMeetingAttachmentResult(string meetingId)
        {
            if (string.IsNullOrEmpty(meetingId))
                return StatusCode(401, "Request is missing values for the request");
            var result = _attachmentService.Get
                (Guid.Parse(meetingId), User.ToRest());
            return result.Condition ? Ok(result.AttachmentCollection) : StatusCode(result.Code, result.Message);
        }
        
        
        [Authorize]
        [HttpPost("api/feature/attachment/add", Name = "Add a meeting attachment")]
        public IActionResult AddMeetingAttachmentResult([FromBody] MeetingItemAttachment request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _attachmentService.Add
                (Guid.Parse( request.ReferanceId), request.FileName, request.Order , User.ToRest());
            return result.Condition ?  Ok(result.Attachment) : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/attachment/update", Name = "Update a meeting attachment")]
        public IActionResult UpdateMeetingAttachmentResult([FromBody] MeetingItemAttachment request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _attachmentService.Update
                (Guid.Parse( request.Id), request.FileName, request.Order , User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
    }
}