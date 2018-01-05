using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	public class SubscriptionController : Controller
	{
		private readonly ISubscriptionService _subscriptionService;

		public SubscriptionController(ISubscriptionService subscriptionService)
		{
			this._subscriptionService = subscriptionService;
		}

		[HttpGet("api/subscriptions")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult Subscriptions()
		{
			var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
			var result = this._subscriptionService.GetList(token);
			return Ok(result);
		}

		[HttpGet("api/subscription")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult Subscription()
		{
			var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
			var result = this._subscriptionService.GetSubscription(token);
			return Ok(result);
		}

		[HttpPost("api/subscription/{subscriptionId}")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult SetSubscription(int subscriptionId)
		{
			var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
			var result = this._subscriptionService.SetSubscriptionForSchema(token, subscriptionId);
			return Ok(result);
		}
	}
}