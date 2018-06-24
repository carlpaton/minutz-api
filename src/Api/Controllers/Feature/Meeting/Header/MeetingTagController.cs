using Api.Extensions;
using Interface.Services.Feature.Meeting.Header;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class MeetingTagController : Controller
    {
        private readonly IMeetingTagService _meetingTagService;
        
        public MeetingTagController(IMeetingTagService meetingTagService)
        {
            _meetingTagService = meetingTagService;
        }

        [Authorize]
        [HttpPost("api/feature/header/tag", Name = "Update Meeting tag")]
        public IActionResult UpdateMeetingTagResult(string id, string tag)
        {
            var userInfo = User.ToRest();
            var result = _meetingTagService.Update(id, tag, userInfo);
            if (result.Condition)
            {
                return Ok();
            }

            return StatusCode(result.Code, result.Message);
        }
    }
}