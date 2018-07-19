using Api.Extensions;
using Interface.Services;
using Interface.Services.Feature.Invite;
using Interface.Services.Feature.Meeting;
using Interface.Services.Feature.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Api.Controllers.Feature.Invite
{
    public class InviteController : Controller
    {
        private readonly IApplicationSetting _applicationSetting;
        private readonly IGetMeetingService _getMeetingService;
        private readonly IInvitationService _invitationService;
        private readonly INotificationService _notificationService;

        public InviteController(IApplicationSetting applicationSetting,
                                IGetMeetingService getMeetingService,
                                IInvitationService invitationService,
                                INotificationService notificationService)
        {
            _applicationSetting = applicationSetting;
            _getMeetingService = getMeetingService;
            _invitationService = invitationService;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Invite Attendee.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Invite
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly-created TodoItem</returns>
        /// <response code="201">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>
        [Authorize]
        [HttpPost("api/feature/attendee/invite", Name = "Invite Attendee for meeting")]
        public IActionResult InviteAttendeeResult([FromBody] MeetingAttendee attendee)
        {
            var userInfo = User.ToRest();
            var validate = attendee.Validate();
            if (!validate.Condition)
                return new BadRequestObjectResult(validate.Message);

            attendee.PersonIdentity = attendee.Email;
            attendee.Role = "Invited";
            attendee.Status = "Invited";

            var meeting = _getMeetingService.GetMeeting(userInfo.InstanceId, attendee.ReferenceId);
            if (!meeting.Condition)
            {
                return BadRequest("There was a issue getting the meeting information.");
            }

            var inviteRecords = _invitationService.InviteUser(userInfo, attendee, meeting.Meeting);
            if (!inviteRecords.Condition)
            {
                return StatusCode(inviteRecords.Code, inviteRecords.Message);
            }

            var notificationResult = _notificationService.SendMeetingInvitation(attendee, meeting.Meeting, userInfo.InstanceId);
            if (notificationResult.Condition)
            {
                return Ok();
            }

            return StatusCode(notificationResult.Code, notificationResult.Message);
        }
    }
}