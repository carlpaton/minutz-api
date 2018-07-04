using Api.Extensions;
using Api.Models.Feature.Meeting;
using Interface.Services.Feature.Meeting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Feature.Meeting.ObjectivePurpose
{
    public class MeetingObjectivePurposeController : Controller
    {
        private readonly IMeetingObjectivePurposeService _meetingObjectivePurposeService;
        
        public MeetingObjectivePurposeController(IMeetingObjectivePurposeService meetingObjectivePurposeService)
        {
            _meetingObjectivePurposeService = meetingObjectivePurposeService;
        }

        [Authorize]
        [HttpPost("api/feature/objective/update", Name = "Update meeting objective")]
        public IActionResult UpdateObjectiveMeetingUpdateRequest([FromBody]MeetingUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _meetingObjectivePurposeService.UpdateObjective
                (request.id, request.outcome, User.ToRest());
            return result.Condition ? (IActionResult) Ok(result.Meeting) : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/purpose/update", Name = "Update meeting purpose")]
        public IActionResult UpdatePurposeMeetingUpdateRequest([FromBody]MeetingUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _meetingObjectivePurposeService.UpdatePurpose
                (request.id, request.purpose ,User.ToRest());
            return result.Condition ? (IActionResult) Ok(result.Meeting) : StatusCode(result.Code, result.Message);
        }
    }
}