using System;
using Api.Extensions;
using Interface.Services.Feature.Meeting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Feature.Meeting
{
    public class MinutzAvailabilityController : Controller
    {
        private readonly IMinutzAvailabilityService _minutzAvailabilityService;

        public MinutzAvailabilityController(IMinutzAvailabilityService minutzAvailabilityService)
        {
            _minutzAvailabilityService = minutzAvailabilityService;
        }

        /// <summary>
        /// Get the available attendees for a meeting
        /// </summary>
        /// <returns>Collection of MeetingAttendees</returns>
        [Authorize]
        [HttpGet("api/feature/availability/attendees", Name = "Get Available Attendees")]
        public IActionResult GetAvailableAttendeesResult()
        {
            var userInfo = User.ToRest();
            var result = _minutzAvailabilityService.GetAvailableAttendees(userInfo);
            return result.Condition ? Ok(result.Attendees) : StatusCode(result.Code, result.Message);
        }
    }
}