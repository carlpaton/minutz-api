using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	public class NotificationTypeController : Controller
	{
		private readonly INotificationTypeService _notificationTypeService;

		public NotificationTypeController(
			INotificationTypeService notificationTypeService)
		{
			this._notificationTypeService = notificationTypeService;
		}

		[HttpGet("api/notificationTypes")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult Reminders()
		{
			var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
			var result = this._notificationTypeService.GetList(token);
			return Ok(result);
		}

		[HttpGet("api/notificationType")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult Reminder()
		{
			var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
			var result = this._notificationTypeService.GetNotificationType(token);
			return Ok(result);
		}
		
		[HttpPost("api/notificationType/{notificationTypeId}")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult SendMinutes(int notificationTypeId)
		{
			var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
			var result = this._notificationTypeService.SetNotificationTypeForSchema(token,notificationTypeId);
			return Ok(result);
		}
	}
}