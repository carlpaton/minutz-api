using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	public class ReminderController : Controller
	{
		private readonly IReminderService _reminderService;

		public ReminderController(IReminderService reminderService)
		{
			this._reminderService = reminderService;
		}

		[HttpGet("api/reminders")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult Reminders()
		{
			var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
			var result = this._reminderService.GetList(token);
			return Ok(result);
		}

		[HttpGet("api/reminder")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult Reminder()
		{
			var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
			var result = this._reminderService.GetReminder(token);
			return Ok(result);
		}
		
		[HttpPost("api/reminder/{reminderId}")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult SendMinutes(int reminderId)
		{
			var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
			var result = this._reminderService.SetReminderForSchema(token,reminderId);
			return Ok(result);
		}
	}
}