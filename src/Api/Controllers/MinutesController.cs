using System;
using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

//using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
  
  public class MinutesController : Controller
  {
    private readonly IMeetingService _meetingService;

    public MinutesController(IMeetingService meetingService)
    {
      _meetingService = meetingService;
    }

     [HttpPost("api/meetingminutes/{referenceId}")]
     [Authorize]
     [Produces("application/json")]
     [ProducesResponseType(typeof(string), 400)]
     [ProducesResponseType(typeof(string), 200)]
     [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(string))]
     public IActionResult SendMinutes(string referenceId)
     {
       if (string.IsNullOrEmpty(referenceId))
         return BadRequest("Please provide a valid id");
       var token = Request.Headers.FirstOrDefault(i => i.Key == "Authorization").Value;
       var result = _meetingService.SendMinutes(token, Guid.Parse(referenceId));
       if (result.Key)
         return Ok(result.Value);
       return BadRequest(result.Value);
     }
  }
}