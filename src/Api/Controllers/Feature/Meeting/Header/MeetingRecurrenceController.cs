using Api.Extensions;
using Interface.Services.Feature.Meeting.Header;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class MeetingRecurrenceController : Controller
    {
        private readonly IMinutzRecurrenceService _minutzRecurrenceService;

        public MeetingRecurrenceController(IMinutzRecurrenceService minutzRecurrenceService)
        {
            _minutzRecurrenceService = minutzRecurrenceService;
        }

        [Authorize]
        [HttpPost("api/feature/header/recurrence", Name = "Update Meeting recurrence type")]
        public IActionResult UpdateMeetingLocationResult(string id, int recurrence)
        {
            var userInfo = User.ToRest();
            var result = _minutzRecurrenceService.Update(id, recurrence, userInfo);
            if (result.Condition)
            {
                return Ok();
            }
            return StatusCode(result.Code, result.Message);
        }
    }
}