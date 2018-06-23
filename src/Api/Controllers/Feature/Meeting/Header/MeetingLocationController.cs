using Api.Extensions;
using Interface.Services.Feature.Meeting.Header;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class MeetingLocationController : Controller
    {
        private readonly IMeetingLocationService _meetingLocationService;

        public MeetingLocationController(IMeetingLocationService meetingLocationService)
        {
            _meetingLocationService = meetingLocationService;
        }

        [Authorize]
        [HttpPost("api/feature/header/location", Name = "Update Meeting location")]
        public IActionResult UpdateMeetingLocationResult(string id, string location)
        {
            var userInfo = User.ToRest();
            var result = _meetingLocationService.Update(id, location, userInfo);
            if (result.Condition)
            {
                return Ok();
            }
            return StatusCode(result.Code, result.Message);
        }
    }
}