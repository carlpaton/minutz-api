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
        [HttpPost("api/feature/agenda/text", Name = "Update agenda text")]
        public IActionResult AddMeetingAttachmentResult([FromBody]MeetingItemAttachment request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _attachmentService.Add
                (request.Id, request.FileName, User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
    }
}