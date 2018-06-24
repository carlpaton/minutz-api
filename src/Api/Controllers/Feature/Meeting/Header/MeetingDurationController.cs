using Api.Extensions;
using Interface.Services.Feature.Meeting.Header;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class MeetingDurationController : Controller
    {
        private readonly IMeetingDurationService _meetingDurationService;
        
        public MeetingDurationController(IMeetingDurationService meetingDurationService)
        {
            _meetingDurationService = meetingDurationService;
        }

        [Authorize]
        [HttpPost("api/feature/header/duration", Name = "Update Meeting duration")]
        public IActionResult UpdateMeetingDurationResult(string id, int duration)
        {
            var userInfo = User.ToRest();
            var result = _meetingDurationService.Update(id, duration, userInfo);
            if (result.Condition)
            {
                return Ok();
            }

            return StatusCode(result.Code, result.Message);
        }
    }
}