using System;
using Api.Extensions;
using Api.Models.Feature.Agenda;
using Interface.Services.Feature.Meeting.Agenda;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Feature.Meeting.Agenda
{
    public class MeetingAgendaController : Controller
    {
        private readonly IMinutzAgendaService _minutzAgendaService;

        public MeetingAgendaController(IMinutzAgendaService minutzAgendaService)
        {
            _minutzAgendaService = minutzAgendaService;
        }

        [Authorize]
        [HttpPut("api/feature/agenda/quick", Name = "Quick create agenda")]
        public IActionResult QuickCreateAgendaResult([FromBody]QuickAgendaRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzAgendaService.QuickCreate
                (request.MeetingId, request.AgendaTitle,request.Order ,User.ToRest());
            return result.Condition ? Ok(result.Agenda) : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/agenda/complete", Name = "Update agenda complete status")]
        public IActionResult UpdateCompleteResult([FromBody]AgendaUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            bool? complete = request.Value as bool?;
            if(complete == null) return StatusCode(401, "Request is missing complete value for the request");
            var result = _minutzAgendaService.UpdateComplete
                (request.Id, (bool)complete ,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/agenda/order", Name = "Update agenda order")]
        public IActionResult UpdateOrderResult([FromBody]AgendaUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            int? order = request.Value as int?;
            if(order == null) return StatusCode(401, "Request is missing order value for the request");
            var result = _minutzAgendaService.UpdateOrder
                (request.Id, (int)order ,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/agenda/duration", Name = "Update agenda duration")]
        public IActionResult UpdateDurationResult([FromBody]AgendaUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            int? duration = request.Value as int?;
            if(duration == null) return StatusCode(401, "Request is missing duration value for the request");
            var result = _minutzAgendaService.UpdateDuration
                (request.Id, (int)duration ,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/agenda/title", Name = "Update agenda title")]
        public IActionResult UpdateTitleResult([FromBody]AgendaUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            string title = request.Value as string;
            if(title == null) return StatusCode(401, "Request is missing title value for the request");
            var result = _minutzAgendaService.UpdateTitle
                (request.Id, (string)title ,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/agenda/text", Name = "Update agenda text")]
        public IActionResult UpdateTextResult([FromBody]AgendaUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            string text = request.Value as string;
            if(text == null) return StatusCode(401, "Request is missing text value for the request");
            var result = _minutzAgendaService.UpdateText
                (request.Id, (string)text ,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/agenda/attendee", Name = "Update attendee")]
        public IActionResult UpdateAssignedAttendeeResult([FromBody]AgendaUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            string attendee = request.Value as string;
            if(attendee == null) return StatusCode(401, "Request is missing attendee value for the request");
            var result = _minutzAgendaService.UpdateAssignedAttendee
                (request.Id, (string)attendee ,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpDelete("api/feature/agenda", Name = "Delete agenda")]
        public IActionResult DeleteAgendaResult(Guid id)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzAgendaService.Delete
                (id,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
    }
}