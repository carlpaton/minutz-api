using System;
using Api.Extensions;
using Api.Models.Feature.Action;
using Interface.Services.Feature.Meeting.Action;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Feature.Meeting.Action
{
    public class MinutzActionController : Controller
    {
        private readonly IMinutzActionService _minutzActionService;

        public MinutzActionController(IMinutzActionService minutzActionService)
        {
            _minutzActionService = minutzActionService;
        }

        [Authorize]
        [HttpGet("api/feature/actions", Name = "Get the actions for a meeting")]
        public IActionResult GetMeetingActionsResult(string meetingId)
        {
            if (string.IsNullOrEmpty(meetingId))
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzActionService.GetMeetingActions
                (Guid.Parse(meetingId) ,User.ToRest());
            return result.Condition ? Ok(result.Actions) : StatusCode(result.Code, result.Message);
        }

        [Authorize]
        [HttpPut("api/feature/action/quick", Name = "Quick create action")]
        public IActionResult QuickCreateActionResult([FromBody]QuickActionRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzActionService.QuickCreate
                (request.MeetingId, request.ActionText,request.Order ,User.ToRest());
            return result.Condition ? Ok(result.Action) : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/action/complete", Name = "Update action complete status")]
        public IActionResult UpdateActionCompleteResult([FromBody]UpdateActionRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            bool? complete = request.Value as bool?;
            if(complete == null) return StatusCode(401, "Request is missing complete value for the request");
            var result = _minutzActionService.UpdateActionComplete
                (request.Id, (bool)complete ,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/action/order", Name = "Update action order")]
        public IActionResult UpdateActionOrderResult([FromBody]UpdateActionRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            int? order = request.Value as int?;
            if(order == null) return StatusCode(401, "Request is missing order value for the request");
            var result = _minutzActionService.UpdateActionOrder
                (request.Id, (int)order ,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/action/assigned", Name = "Update action assigned attendee")]
        public IActionResult UpdateActionAssignedAttendeeResult([FromBody]UpdateActionRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            string assignedAttendee = request.Value as string;
            if(string.IsNullOrEmpty(assignedAttendee)) return StatusCode(401, "Request is missing assigned attendee value for the request");
            var result = _minutzActionService.UpdateActionAssignedAttendee
                (request.Id, assignedAttendee ,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/action/due", Name = "Update action due date")]
        public IActionResult UpdateActionDueDateResult([FromBody]UpdateActionRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            DateTime? due = request.Value as DateTime?;
            if(due == null) return StatusCode(401, "Request is missing due date value for the request");
            var result = _minutzActionService.UpdateActionDueDate
                (request.Id, (DateTime)due ,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpDelete("api/feature/action", Name = "Delete action")]
        public IActionResult DeleteActionResult(Guid id)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzActionService.Delete
                (id,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
    }
}