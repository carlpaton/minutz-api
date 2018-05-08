using System;
using System.IO;
using Api.Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.SwaggerGen;

//using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
  
  public class MinutesController : Controller
  {
    private readonly IMeetingService _meetingService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IFileProvider _fileProvider;

    public MinutesController(
      IMeetingService meetingService,
      IAuthenticationService authenticationService,
      IFileProvider fileProvider)
    {
      _meetingService = meetingService;
      _authenticationService = authenticationService;
      _fileProvider = fileProvider;
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
       var userInfo = Request.ExtractAuth(User, _authenticationService);
       var result = _meetingService.SendMinutes(userInfo.InfoResponse, Guid.Parse(referenceId));
       if (result.Key)
         return Ok(result.Value);
       return BadRequest(result.Value);
     }
    
    [Authorize]
    [HttpGet("api/meetingminutes/{meetingId}/preview")]
    public IActionResult GetPreview(string meetingId)
    {
      var userInfo = Request.ExtractAuth(User, _authenticationService);
      var path = Directory.GetCurrentDirectory();
      var fileResult = _meetingService.GetMinutesPreview(userInfo.InfoResponse, Guid.Parse(meetingId),path);
      if (!fileResult.Key) return StatusCode(500);
      // var directoryContents = _fileProvider.GetDirectoryContents("");
//      var path = Directory.GetCurrentDirectory();
//      if (directoryContents.Any(i => i.Name == "test.pdf"))
//      {
//        // delete then recreate
//      }
//      else
//      {
//        
//        using (var fs = new FileStream($"{path}/test.pdf", FileMode.Create, FileAccess.Write))
//        {
//          fs.Write(fileResult.Value, 0, fileResult.Value.Length);
//        }
//      }

      //var response = $"{path}/test.pdf"; //File(fileResult.Value, "application/pdf");
      //return Ok(response);
      return Ok();
    }
  }
}