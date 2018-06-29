using Api.Extensions;
using Interface.Services.Feature.Meeting.Header;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class MeetingDurationController : Controller
    {
        private readonly IMinutzDurationService _minutzDurationService;
        
        public MeetingDurationController(IMinutzDurationService minutzDurationService)
        {
            _minutzDurationService = minutzDurationService;
        }

        [Authorize]
        [HttpPost("api/feature/header/duration", Name = "Update Meeting duration")]
        public IActionResult UpdateMeetingDurationResult(string id, int duration)
        {
            var userInfo = User.ToRest();
            var result = _minutzDurationService.Update(id, duration, userInfo);
            if (result.Condition)
            {
                return Ok();
            }

            return StatusCode(result.Code, result.Message);
        }
    }
}