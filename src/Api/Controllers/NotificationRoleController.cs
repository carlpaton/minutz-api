using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	public class NotificationRoleController : Controller
	{
		private readonly INotificationRoleService _notificationRoleService;

		public NotificationRoleController(INotificationRoleService notificationRoleService)
		{
			this._notificationRoleService = notificationRoleService;
		}

		[HttpGet("api/notificationRoles")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult Reminders()
		{
			var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
			var result = this._notificationRoleService.GetList(token);
			return Ok(result);
		}

		[HttpGet("api/notificationRole")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult Reminder()
		{
			var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
			var result = this._notificationRoleService.GetNotificationRole(token);
			return Ok(result);
		}
		
		[HttpPost("api/notificationRole/{notificationRoleId}")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult SendMinutes(int notificationRoleId)
		{
			var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
			var result = this._notificationRoleService.SetNotificationRoleForSchema(token,notificationRoleId);
			return Ok(result);
		}
	}
}