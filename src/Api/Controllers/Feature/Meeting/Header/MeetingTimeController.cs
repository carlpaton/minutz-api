using Api.Extensions;
using Interface.Services.Feature.Meeting.Header;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class MeetingTimeController : Controller
    {
        private readonly IMeetingTimeService _meetingTimeService;
        
        public MeetingTimeController(IMeetingTimeService meetingTimeService)
        {
            _meetingTimeService = meetingTimeService;
        }

        [Authorize]
        [HttpPost("api/feature/header/time", Name = "Update Meeting time")]
        public IActionResult UpdateMeetingTimeResult(string id, string time)
        {
            var userInfo = User.ToRest();
            var result = _meetingTimeService.Update(id, time, userInfo);
            if (result.Condition)
            {
                return Ok();
            }

            return StatusCode(result.Code, result.Message);
        }
    }
}