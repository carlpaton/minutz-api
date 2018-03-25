using System;
using System.Linq;
using System.Threading.Tasks;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;

//using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
  
  public class MinutesController : Controller
  {
    private readonly IMeetingService _meetingService;
    private readonly IAuthenticationService _authenticationService;

    public MinutesController(
      IMeetingService meetingService, IAuthenticationService authenticationService)
    {
      _meetingService = meetingService;
      _authenticationService = authenticationService;
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
       var userInfo = ExtractAuth();
       var result = _meetingService.SendMinutes(userInfo.infoResponse, Guid.Parse(referenceId));
       if (result.Key)
         return Ok(result.Value);
       return BadRequest(result.Value);
     }
    
    [Authorize]
    [HttpGet("api/meetingminutes/{meetingId}/preview")]
    public IActionResult GetPreview(string meetingId)
    {
      var userInfo = ExtractAuth();
      var fileResult = _meetingService.GetMinutesPreview(userInfo.infoResponse, Guid.Parse(meetingId));
      if (!fileResult.Key) return StatusCode(500);
      var response = File(fileResult.Value, "application/pdf");
      return response;
    }
    
    
    private (bool condition, string message, AuthRestModel infoResponse) ExtractAuth()
    {
      (bool condition, string message, AuthRestModel infoResponse) userInfo =
        _authenticationService.Login(
          Request.Headers.First(i => i.Key == "access_token").Value,
          Request.Headers.First(i => i.Key == "Authorization").Value,
          User.Claims.ToList().First(i => i.Type == "exp").Value, "");
      return userInfo;
    }
  }
}