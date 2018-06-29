using Api.Extensions;
using Interface.Services.Feature.Meeting.Header;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class MeetingTimeController : Controller
    {
        private readonly IMinutzTimeService _minutzTimeService;
        
        public MeetingTimeController(IMinutzTimeService minutzTimeService)
        {
            _minutzTimeService = minutzTimeService;
        }

        [Authorize]
        [HttpPost("api/feature/header/time", Name = "Update Meeting time")]
        public IActionResult UpdateMeetingTimeResult(string id, string time)
        {
            var userInfo = User.ToRest();
            var result = _minutzTimeService.Update(id, time, userInfo);
            if (result.Condition)
            {
                return Ok();
            }

            return StatusCode(result.Code, result.Message);
        }
    }
}