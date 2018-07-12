using Api.Extensions;
using Interface.Services;
using Interface.Services.Feature.Invite;
using Interface.Services.Feature.Meeting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;

namespace Api.Controllers.Feature.Invite
{
    public class InviteController : Controller
    {
        private readonly IApplicationSetting _applicationSetting;
        private readonly IGetMeetingService _getMeetingService;
        private readonly IInvitationService _invitationService;

        public InviteController(IApplicationSetting applicationSetting ,IGetMeetingService getMeetingService, IInvitationService invitationService)
        {
            _applicationSetting = applicationSetting;
            _getMeetingService = getMeetingService;
            _invitationService = invitationService;
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
            if (validate.Condition)
                return new BadRequestObjectResult(validate.Message);

            var meeting = _getMeetingService.GetMeeting(userInfo.InstanceId, attendee.ReferenceId);
            
            attendee.PersonIdentity = attendee.Email;
       
            attendee.Role = "Invited";
            attendee.Status = "Invited";
            
            //var result = _invationService.SendMeetingInvatation(invitee, meeting,userInfo.InfoResponse.InstanceId);
//            if (result)
//            {
//                var savedUser = _meetingService.InviteUser(userInfo.InfoResponse, invitee,meeting.Id,invitee.Email);
//                if (savedUser)
//                {
//                    return new ObjectResult(invitee);
//                }
//                return BadRequest("There was a issue saving the invited user.");
//            }
            return BadRequest("There was a issue inviting the user.");
        }
    }
}