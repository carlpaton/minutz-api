using Api.Extensions;
using Interface.Services.Feature.Meeting.Header;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class MeetingTagController : Controller
    {
        private readonly IMinutzTagService _minutzTagService;
        
        public MeetingTagController(IMinutzTagService minutzTagService)
        {
            _minutzTagService = minutzTagService;
        }

        [Authorize]
        [HttpPost("api/feature/header/tag", Name = "Update Meeting tag")]
        public IActionResult UpdateMeetingTagResult(string id, string tag)
        {
            var userInfo = User.ToRest();
            var result = _minutzTagService.Update(id, tag, userInfo);
            if (result.Condition)
            {
                return Ok();
            }

            return StatusCode(result.Code, result.Message);
        }
    }
}