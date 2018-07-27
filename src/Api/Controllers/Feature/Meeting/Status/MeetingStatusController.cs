using System;
using Api.Extensions;
using Interface.Services.Feature.Meeting.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Feature.Meeting.Status
{
    public class MeetingStatusController : Controller
    {
        private readonly IMeetingStatusService _meetingStatusService;

        public MeetingStatusController(IMeetingStatusService meetingStatusService)
        {
            _meetingStatusService = meetingStatusService;
        }

        [Authorize]
        [HttpPost("api/feature/status/update", Name = "Update meeting Status Update")]
        public IActionResult UpdateMeetingStatusResult(string meetingId, string status)
        {
            if (string.IsNullOrEmpty(meetingId))
                return StatusCode(401, "Request is missing values for the request");
            var result = _meetingStatusService.UpdateMeetingStatus
                (Guid.Parse(meetingId), status, User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
    }
}
