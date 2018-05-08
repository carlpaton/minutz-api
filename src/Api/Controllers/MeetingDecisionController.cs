using System.Collections.Generic;
using System.Linq;
using Api.Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Minutz.Models.Message;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Controllers
{
    public class MeetingDecisionController : Controller
    {
        private readonly IMeetingDecisionService _meetingDecisionService;
        private readonly IAuthenticationService _authenticationService;

        public MeetingDecisionController(
            IMeetingDecisionService meetingDecisionService,
             IAuthenticationService authenticationService)
        {
            _meetingDecisionService = meetingDecisionService;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Get all agenda items for a meeting
        /// </summary>
        /// <returns>Collection of MeetingAgenda objects</returns>
        [Authorize]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(List<MinutzDecision>), 200)]
        [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(List<MinutzDecision>))]
        [HttpGet("api/meetingDecisions/{referenceId}", Name = "Get meeting decisions")]
        public IActionResult Get(string referenceId)
        {
            if (string.IsNullOrEmpty(referenceId))
            {
                return BadRequest("Please provide a valid referenceId [meeting id]");
            }
            var userInfo = Request.ExtractAuth(User, _authenticationService);
            var result = _meetingDecisionService.GetMinutzDecisions(referenceId, userInfo.InfoResponse);
            return Ok(result);
        }
        
        /// <summary>
        /// Create a meeting Agenda
        /// </summary>
        /// <returns></returns>
        [HttpPut("api/meetingDecision/{referenceId}/decision")]
        [Authorize]
        public IActionResult Put([FromBody] MinutzDecision decision)
        {
            var userInfo = Request.ExtractAuth(User, _authenticationService);
            var result =
                _meetingDecisionService.CreateMinutzDecision(decision.ReferanceId.ToString(), decision,userInfo.InfoResponse);
            return result.condition ? Ok(result.value) : StatusCode(500, result.message);
        }

        /// <summary>
        /// Update the MeetingViewModel Action
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/meetingDecision/{referenceId}/decision/{id}")]
        [Authorize]
        public IActionResult Post([FromBody] MinutzDecision decision)
        {
            var userInfo = Request.ExtractAuth(User, _authenticationService);
            var result =
                _meetingDecisionService.UpdateMinutzDecision(decision.ReferanceId.ToString(), decision, userInfo.InfoResponse);
            return result.condition ? Ok(result.value) : StatusCode(500, result.message);
        }

        /// <summary>
        /// Delete the single instance of the action item.
        /// </summary>
        /// <returns>true if successful and false if failure.</returns>
        [HttpDelete("api/meetingDecision/{referenceId}/decision/{id}")]
        [Authorize]
        public IActionResult Delete(string referenceId, string id)
        {
            var userInfo = Request.ExtractAuth(User, _authenticationService);
            var result = _meetingDecisionService.DeleteMinutzDecision(referenceId, id, userInfo.InfoResponse);
            return result.condition ? Ok(result.message) : StatusCode(500, result.message);
        }
    }
}