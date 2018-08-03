using System;
using Api.Extensions;
using Api.Models.Feature.Agenda;
using Interface.Services.Feature.Meeting.Agenda;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Newtonsoft.Json;

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
        [HttpGet("api/feature/agenda/meeting", Name = "Get meeting agenda collection")]
        public IActionResult GetMeetingAgendaCollectionResult(string meetingId)
        {
            if (string.IsNullOrEmpty(meetingId))
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzAgendaService.GetMeetingAgendaCollection(Guid.Parse(meetingId) ,User.ToRest());
            return result.Condition ? Ok(result.Agenda) : StatusCode(result.Code, result.Message);
        } 
            
         [Authorize]
        [HttpGet("api/feature/agenda/collection", Name = "Get the collection of agenda items")]
        public IActionResult GetCollectionAgendaResult(string refId)
        {
            if (string.IsNullOrEmpty(refId))
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzAgendaService.GetMeetingAgendaCollection
                (Guid.Parse(refId),User.ToRest());
            return result.Condition ? Ok(result.AgendaCollection) : StatusCode(result.Code, result.Message);
        }

        
        [Authorize]
        [HttpPut("api/feature/agenda/quick", Name = "Quick create agenda")]
        public IActionResult QuickCreateAgendaResult([FromBody]MeetingAgenda request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzAgendaService.QuickCreate
                (request.ReferenceId, request.AgendaHeading,request.Order ,User.ToRest());
            return result.Condition ? Ok(result.Agenda) : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/agenda/complete", Name = "Update agenda complete status")]
        public IActionResult UpdateCompleteResult([FromBody]MeetingAgenda request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzAgendaService.UpdateComplete
                (request.Id, request.IsComplete ,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/agenda/order", Name = "Update agenda order")]
        public IActionResult UpdateOrderResult([FromBody]MeetingAgenda request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzAgendaService.UpdateOrder
                (request.Id, request.Order ,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/agenda/duration", Name = "Update agenda duration")]
        public IActionResult UpdateDurationResult([FromBody]MeetingAgenda request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzAgendaService.UpdateDuration
                (request.Id, Convert.ToInt32(request.Duration) ,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/agenda/title", Name = "Update agenda title")]
        public IActionResult UpdateTitleResult([FromBody]MeetingAgenda request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzAgendaService.UpdateTitle
                (request.Id, request.AgendaHeading ,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/agenda/text", Name = "Update agenda text")]
        public IActionResult UpdateTextResult([FromBody]MeetingAgenda request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzAgendaService.UpdateText
                (request.Id, request.AgendaText,User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/agenda/attendee", Name = "Update attendee")]
        public IActionResult UpdateAssignedAttendeeResult([FromBody]MeetingAgenda request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzAgendaService.UpdateAssignedAttendee
                (request.Id, request.MeetingAttendeeId ,User.ToRest());
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