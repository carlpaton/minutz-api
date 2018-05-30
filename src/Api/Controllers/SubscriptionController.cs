using System.Linq;
using Api.Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
	public class SubscriptionController : Controller
	{
		private readonly ISubscriptionService _subscriptionService;
		private readonly INotificationTypeService _notificationTypeService;
		private readonly IMeetingService _meetingService;
		private readonly ILogService _logService;
		private readonly IAuthenticationService _authenticationService;
		private readonly IMeetingAttachmentService _meetingAttachmentService;
		private readonly ILogger _logger;

		public SubscriptionController(
			ISubscriptionService subscriptionService,
			INotificationTypeService notificationTypeService,
			INotificationRoleService notificationRoleService,
			IMeetingService meetingService,
			ILogService logService,
			ILoggerFactory logger,
			IAuthenticationService authenticationService,
			IMeetingAttachmentService meetingAttachmentService)
		{
			_subscriptionService = subscriptionService;
			_notificationTypeService = notificationTypeService;
			_meetingService = meetingService;
			_logService = logService;
			_authenticationService = authenticationService;
			_meetingAttachmentService = meetingAttachmentService;
			_logger = logger.CreateLogger("SubscriptionController");
		}

		[HttpGet("api/subscriptions")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult Subscriptions()
		{
			var userInfo = Request.ExtractAuth(User, _authenticationService);
			var result = _subscriptionService.GetList(userInfo.InfoResponse.AccessToken);
			return Ok(result);
		}

		[HttpGet("api/subscription")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult Subscription()
		{
			var userInfo = Request.ExtractAuth(User, _authenticationService);
			var result = _subscriptionService.GetSubscription(userInfo.InfoResponse.AccessToken);
			return Ok(result);
		}

		[HttpPost("api/subscription/{subscriptionId}")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult SetSubscription(int subscriptionId)
		{
			var userInfo = Request.ExtractAuth(User, _authenticationService);
			var result = _subscriptionService.SetSubscriptionForSchema(userInfo.InfoResponse.AccessToken, subscriptionId);
			return Ok(result);
		}
	}
}