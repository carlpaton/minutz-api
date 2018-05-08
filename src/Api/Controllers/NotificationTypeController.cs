using System.Linq;
using Api.Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
	public class NotificationTypeController : Controller
	{
		private readonly INotificationTypeService _notificationTypeService;
		private readonly IMeetingService _meetingService;
		private readonly ILogService _logService;
		private readonly IAuthenticationService _authenticationService;
		private readonly IMeetingAttachmentService _meetingAttachmentService;
		private readonly ILogger _logger;

		public NotificationTypeController(
			INotificationTypeService notificationTypeService,
			INotificationRoleService notificationRoleService,
			IMeetingService meetingService,
			ILogService logService,
			ILoggerFactory logger,
			IAuthenticationService authenticationService,
			IMeetingAttachmentService meetingAttachmentService)
		{
			_notificationTypeService = notificationTypeService;
			_meetingService = meetingService;
			_logService = logService;
			_authenticationService = authenticationService;
			_meetingAttachmentService = meetingAttachmentService;
			_logger = logger.CreateLogger("NotificationTypeController");
		}

		[HttpGet("api/notificationTypes")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult SotificationTypes()
		{
			var userInfo = Request.ExtractAuth(User, _authenticationService);
			var result = this._notificationTypeService.GetList(userInfo.InfoResponse.AccessToken);
			return Ok(result);
		}

		[HttpGet("api/notificationType")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult NotificationType()
		{
			var userInfo = Request.ExtractAuth(User, _authenticationService);
			var result = this._notificationTypeService.GetNotificationType(userInfo.InfoResponse.AccessToken);
			return Ok(result);
		}
		
		[HttpPost("api/notificationType/{notificationTypeId}")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult SetNotificationType(int notificationTypeId)
		{
			var userInfo = Request.ExtractAuth(User, _authenticationService);
			var result = this._notificationTypeService.SetNotificationTypeForSchema(userInfo.InfoResponse.AccessToken,notificationTypeId);
			return Ok(result);
		}
	}
}