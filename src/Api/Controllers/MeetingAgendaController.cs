using System.Collections.Generic;
using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Newtonsoft.Json;
using System;
using Minutz.Models.Message;

namespace Api.Controllers
{
  public class MeetingAgendaController : Controller
  {
    private readonly IMeetingService _meetingService;
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger _logger;
    internal const string _defaultAgendaId = "e38b69b3-8f2a-4979-9323-1819db4331f8";
    
    public MeetingAgendaController(
      IMeetingService meetingService, ILoggerFactory logger, IAuthenticationService authenticationService)
    {
      this._meetingService = meetingService;
      _authenticationService = authenticationService;
      this._logger = logger.CreateLogger("MeetingAgendaController");
    }

    /// <summary>
    /// Get all agenda items for a meeting
    /// </summary>
    /// <returns>Collection of MeetingAgenda objects</returns>
     [HttpGet("api/meetingAgendaItems/{referenceId}")]
     [Authorize]
     public List<MeetingAgenda> GetMeetingAgendaItems(
            string referenceId)
     {
      
       var userInfo = ExtractAuth();
       return new List<MeetingAgenda>();
     }

    /// <summary>
    ///  Returns one instance of a meeting agenda
    /// </summary>
    /// <returns>MeetingAgenda object</returns>
     [HttpGet("api/meetingAgenda/{id}")]
     [Authorize]
     public List<MeetingAgenda> GetAgendaItem(string id)
     {
       var userInfo = ExtractAuth();
       return null;
     }

     [HttpPost("api/meetingAgenda/{id}")]
     [Authorize]
     public List<MeetingAgenda> UpdateMeetingAgendaitems([FromBody] List<MeetingAgenda> agendaitems)
     {
       var userInfo = ExtractAuth();
       var id = agendaitems.First().ReferenceId;
       var result = _meetingService.UpdateMeetingAgendaItems(id,agendaitems, userInfo.InfoResponse);
       return result;
     }

    /// <summary>
    /// Create a meeting Agenda
    /// </summary>
    /// <returns></returns>
     [HttpPut("api/meetingAgenda")]
     [Authorize]
     public MeetingAgenda Put([FromBody] MeetingAgenda agenda)
     {
       _logger.LogInformation(Core.LogProvider.LoggingEvents.InsertItem, "MeetingAgenda - PUT - entry point {ID}", 1);
       var payload = JsonConvert.SerializeObject(agenda);
       _logger.LogInformation(Core.LogProvider.LoggingEvents.InsertItem," sent data {payload}", payload);
       
       var userInfo = ExtractAuth();
       if (agenda.Id == Guid.Parse(_defaultAgendaId)) agenda.Id = Guid.NewGuid();
       var result = _meetingService.CreateMeetingAgendaItem(agenda, userInfo.InfoResponse);
       return result;
     }

    /// <summary>
    /// Update the MeetingViewModel Agenda
    /// </summary>
    /// <returns></returns>
     [HttpPost("api/meetingAgenda")]
     [Authorize]
     public MeetingAgenda Post([FromBody] MeetingAgenda agenda)
     {
       var userInfo = ExtractAuth();
       return new MeetingAgenda();
     }

    /// <summary>
    /// Delete the single instance of the agenda item.
    /// </summary>
    /// <returns>true if successful and false if failure.</returns>
     [HttpDelete("api/meetingAgenda/{id}")]
     [Authorize]
     public bool Delete(string id)
     {
       var userInfo = ExtractAuth();
       return true;
     }
    
    private AuthRestModelResponse ExtractAuth()
    {
      var userInfo =
        _authenticationService.LoginFromFromToken(
          Request.Headers.First(i => i.Key == "access_token").Value,
          Request.Headers.First(i => i.Key == "Authorization").Value,
          User.Claims.ToList().First(i => i.Type == "exp").Value, "");
      return userInfo;
    }
  }
}