using System;
using Api.Extensions;
using Interface.Services.Feature.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Feature.Dashboard.Meeting
{
    public class UserMeetingController : Controller
    {
        private readonly IUserMeetingsService _userMeetingsService;

        public UserMeetingController(IUserMeetingsService userMeetingsService)
        {
            _userMeetingsService = userMeetingsService;
        }

        [Authorize]
        [HttpGet("api/feature/dashboard/usermeetings", Name = "User Meeting List")]
        public IActionResult UserMeetingsResult()
        {
            var userInfo = User.ToRest();
            var result = _userMeetingsService.Meetings(userInfo);
            if (result.Condition)
            {
                return Ok(result.Meetings);
            }
            return StatusCode(result.Code, result.Message);
        }

        [Authorize]
        [HttpGet("api/feature/dashboard/usermeeting", Name = "User Meeting Item")]
        public IActionResult UserMeetingResult(Guid meetingId)
        {
            var userInfo = User.ToRest();
            var result = _userMeetingsService.Meeting(userInfo, meetingId);
            if (result.Condition)
            {
                return Ok(result.Meeting);
            }
            return StatusCode(result.Code, result.Message);
        }
    }
}