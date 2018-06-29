using Api.Extensions;
using Interface.Services.Feature.Meeting.Header;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class MeetingTitleController : Controller
    {
        private readonly IMinutzTitleService _minutzTitleService;

        public MeetingTitleController(IMinutzTitleService minutzTitleService)
        {
            _minutzTitleService = minutzTitleService;
        }

        [Authorize]
        [HttpPost("api/feature/header/title", Name = "Update Meeting title")]
        public IActionResult UpdateMeetingTitleResult(string id, string title)
        {
            var userInfo = User.ToRest();
            var result = _minutzTitleService.Update(id, title, userInfo);
            if (result.Condition)
            {
                return Ok();
            }
            return StatusCode(result.Code, result.Message);
        }
    }
}