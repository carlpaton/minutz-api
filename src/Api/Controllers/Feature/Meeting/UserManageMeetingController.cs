using Api.Models.Feature.Meeting;
using Microsoft.AspNetCore.Authorization;
using Interface.Services.Feature.Dashboard;
using Interface.Repositories.Feature.Meeting;
using Microsoft.AspNetCore.Mvc;
using Api.Extensions;
using System;

namespace Api.Controllers.Feature.Meeting
{
    public class UserManageMeetingController: Controller
    {
        private readonly IUserMeetingsService _userMeetingsService;
        private readonly IUserManageMeetingService _userManageMeetingService;

        public UserManageMeetingController(
            IUserMeetingsService userMeetingsService,
            IUserManageMeetingService userManageMeetingService)
        {
             _userMeetingsService = userMeetingsService;
        }

        [Authorize]
        [HttpGet("api/feature/managemeeting/user", Name = "User Meeting Item")]
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

        [Authorize]
        [HttpPost("api/feature/managemeeting/user", Name = "User Meeting Item")]
        public IActionResult UserMeetingResult([FromBody] MeetingModel meeting)
        {
            var userInfo = User.ToRest();
            var meetingEntity = new Minutz.Models.Entities.Meeting {
                Id = meeting.id,
                
            };
            var result = _userManageMeetingService.UpdateMeeting(meetingEntity, userInfo);
            if (result.Condition)
            {
                return Ok(result.Meeting);
            }
            return StatusCode(result.Code, result.Message);
        }
    }
}