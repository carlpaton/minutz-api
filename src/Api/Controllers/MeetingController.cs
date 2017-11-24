using System.Collections.Generic;
using System.Linq;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace Api.Controllers
{
  public class MeetingController : Controller
  {
    private readonly IMeetingService _meetingService;
    public MeetingController (IMeetingService meetingService)
    {
      _meetingService = meetingService;
    }

    /// <summary>
    /// Get all the meetings for a user
    /// </summary>
    /// <returns>Collection of Meeting objects</returns>
    [HttpGet ("api/meeting")]
    [Authorize]
    public IEnumerable<Meeting> Get ()
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return _meetingService.GetMeetings (token);
    }

    /// <summary>
    /// Get the meeting object based on identifier.
    /// </summary>
    /// <param name="id">Meeting identifier [Guid]</param>
    /// <returns>The meeting object.</returns>
    [HttpGet ("api/meeting/{id}")]
    [Authorize]
    public Meeting Get (string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return _meetingService.GetMeeting (token, id);
    }

    /// <summary>
    /// Create a meeting
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The created meeting object.</returns>
    [HttpPut ("api/meeting")]
    [Authorize]
    public Meeting Put (Meeting meeting)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return _meetingService.GetMeeting (token, meeting.Id.ToString());
    }

    /// <summary>
    /// update a meeting
    /// </summary>
    /// <param name="id">meeting identifier.</param>
    /// <returns>The saved meeting object.</returns>
    [HttpPost ("api/meeting/{id}")]
    [Authorize]
    public Meeting Post (Meeting meeting)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return _meetingService.GetMeeting (token, meeting.Id.ToString());
    }

    /// <summary>
    /// Delete a meeting
    /// </summary>
    /// <param name="id">meeting identifier.</param>
    /// <returns>True if Successful or False if unsuccessful.</returns>
    [HttpDelete ("api/meeting/{id}")]
    [Authorize]
    public bool Delete (string id)
    {
      var token = Request.Headers.FirstOrDefault (i => i.Key == "Authorization").Value;
      return true; //_meetingService.GetMeeting (token, id);
    }
  }
}