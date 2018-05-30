using System.Linq;
using Api.Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    public class ReminderController : Controller
    {
        private readonly IReminderService _reminderService;
        private readonly INotificationTypeService _notificationTypeService;
        private readonly IMeetingService _meetingService;
        private readonly ILogService _logService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMeetingAttachmentService _meetingAttachmentService;
        private readonly ILogger _logger;

        public ReminderController(
            IReminderService reminderService,
            INotificationTypeService notificationTypeService,
            INotificationRoleService notificationRoleService,
            IMeetingService meetingService,
            ILogService logService,
            ILoggerFactory logger,
            IAuthenticationService authenticationService,
            IMeetingAttachmentService meetingAttachmentService)
        {
            _reminderService = reminderService;
            _notificationTypeService = notificationTypeService;
            _meetingService = meetingService;
            _logService = logService;
            _authenticationService = authenticationService;
            _meetingAttachmentService = meetingAttachmentService;
            _logger = logger.CreateLogger("ReminderController");
        }

        [HttpGet("api/reminders")]
        [Authorize]
        [Produces("application/json")]
        public IActionResult Reminders()
        {
            var userInfo = Request.ExtractAuth(User, _authenticationService);
            var result = _reminderService.GetList(userInfo.InfoResponse.AccessToken);
            return Ok(result);
        }

        [HttpGet("api/reminder")]
        [Authorize]
        [Produces("application/json")]
        public IActionResult Reminder()
        {
            var userInfo = Request.ExtractAuth(User, _authenticationService);
            var result = _reminderService.GetReminder(userInfo.InfoResponse.AccessToken);
            return Ok(result);
        }

        [HttpPost("api/reminder/{reminderId}")]
        [Authorize]
        [Produces("application/json")]
        public IActionResult SetReminder(int reminderId)
        {
            var userInfo = Request.ExtractAuth(User, _authenticationService);
            var result = _reminderService.SetReminderForSchema(userInfo.InfoResponse.AccessToken, reminderId);
            return Ok(result);
        }
    }
}