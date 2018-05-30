using System.Linq;
using Api.Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
	public class NotificationRoleController : Controller
	{
		private readonly INotificationRoleService _notificationRoleService;
		private readonly IMeetingService _meetingService;
		private readonly ILogService _logService;
		private readonly IAuthenticationService _authenticationService;
		private readonly IMeetingAttachmentService _meetingAttachmentService;
		private readonly ILogger _logger;
		
		public NotificationRoleController(
			INotificationRoleService notificationRoleService,
			IMeetingService meetingService,
			ILogService logService,
			ILoggerFactory logger,
			IAuthenticationService authenticationService,
			IMeetingAttachmentService meetingAttachmentService)
		{
			_notificationRoleService = notificationRoleService;
			_meetingService = meetingService;
			_logService = logService;
			_authenticationService = authenticationService;
			_meetingAttachmentService = meetingAttachmentService;
			_logger = logger.CreateLogger("NotificationRoleController");
		}

		[HttpGet("api/notificationRoles")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult NotificationRoles()
		{
			var userInfo = Request.ExtractAuth(User, _authenticationService);
			var result = _notificationRoleService.GetList(userInfo.InfoResponse.AccessToken);
			return Ok(result);
		}

		[HttpGet("api/notificationRole")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult NotificationRole()
		{
			var userInfo = Request.ExtractAuth(User, _authenticationService);
			var result = _notificationRoleService.GetNotificationRole(userInfo.InfoResponse.AccessToken);
			return Ok(result);
		}
		
		[HttpPost("api/notificationRole/{notificationRoleId}")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult SetNotificationRole(int notificationRoleId)
		{
			var userInfo = Request.ExtractAuth(User, _authenticationService);
			var result = _notificationRoleService.SetNotificationRoleForSchema(userInfo.InfoResponse.AccessToken,notificationRoleId);
			return Ok(result);
		}
	}
}