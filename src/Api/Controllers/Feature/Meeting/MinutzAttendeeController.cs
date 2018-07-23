using System;
using Api.Extensions;
using Interface.Services.Feature.Meeting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;

namespace Api.Controllers.Feature.Meeting
{
    public class MinutzAttendeeController : Controller
    {
        private readonly IMinutzAttendeeService _minutzAttendeeService;

        public MinutzAttendeeController(IMinutzAttendeeService minutzAttendeeService)
        {
            _minutzAttendeeService = minutzAttendeeService;
        }

        [Authorize]
        [HttpGet("api/feature/attendee/attendees", Name = "Get Attendees for meeting")]
        public IActionResult GetMeetingAttendeesResult(string meetingId)
        {
            var userInfo = User.ToRest();
            var result = _minutzAttendeeService.GetAttendees(Guid.Parse(meetingId),userInfo);
            return result.Condition ? Ok(result.Attendees) : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/attendee/update", Name = "Update Attendee for meeting")]
        public IActionResult GetAvailableAttendeesResult([FromBody] MeetingAttendee attendee)
        {
            var userInfo = User.ToRest();
            var result = _minutzAttendeeService.UpdateAttendee(attendee.ReferenceId, attendee,userInfo);
            return result.Condition ? Ok(result.Attendees) : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/attendee/add", Name = "Add Attendee to Meeting")]
        public IActionResult AddMeetingAttendeeResult([FromBody] MeetingAttendee attendee) 
        {
            var userInfo = User.ToRest();
            var result = _minutzAttendeeService.AddAttendee(attendee.ReferenceId,attendee,userInfo);
            return result.Condition ? Ok(result.Attendees) : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpDelete("api/feature/attendee/remove", Name = "Remove Attendee from Meeting")]
        public IActionResult GetAvailableAttendeesResult(string meetingId, string attendeeEmail)
        {
            var userInfo = User.ToRest();
            var result = _minutzAttendeeService.DeleteAttendee(Guid.Parse(meetingId), attendeeEmail,userInfo);
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
    }
}