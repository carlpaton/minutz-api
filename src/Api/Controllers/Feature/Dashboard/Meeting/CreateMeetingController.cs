using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api.Extensions;
using Interface.Services.Feature.Dashboard;

namespace Api.Controllers.Feature.Dashboard.Meeting
{
    public class CreateMeetingController : Controller
    {
        private readonly IUserMeetingsService _userMeetingService;
        public CreateMeetingController(IUserMeetingsService userMeetingService)
        {
            _userMeetingService = userMeetingService;
        }

        [Authorize]
        [HttpPut("api/feature/dashboard/createusermeeting", Name = "Create a User Meeting")]
        public IActionResult UserMeetingsResult()
        {
            var userInfo = User.ToRest();
            var result = _userMeetingService.CreateEmptyUserMeeting(userInfo);
            if (result.Condition)
            {
                return Ok(result.Meeting);
            }
            return StatusCode(result.Code, result.Message);
        }
    }
}