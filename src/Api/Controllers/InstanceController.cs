using System.Linq;
using Api.Models;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;

namespace Api.Controllers
{
	public class InstanceController : Controller
	{
		private readonly IInstanceService _instanceService;
		public InstanceController(IInstanceService instanceService)
		{
			this._instanceService = instanceService;
		}

		[HttpPost("api/instance")]
		[Authorize]
		[Produces("application/json")]
		public IActionResult UpdateInstance(InstanceSimple instance)
		{
			var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
			var dirty = new Instance();
			dirty.AllowInformal = instance.AllowInformal;
			dirty.Id = instance.Id;
			dirty.Colour = instance.Colour;
			dirty.Style = instance.Style;
			var result = this._instanceService.SetInstanceDetailsForSchema(token,dirty);
			return Ok(result);
		}
	}
}