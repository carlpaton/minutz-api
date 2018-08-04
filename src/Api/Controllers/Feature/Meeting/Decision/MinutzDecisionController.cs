using System;
using Api.Extensions;
using Api.Models.Feature.Decision;
using Interface.Services.Feature.Meeting.Decision;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Newtonsoft.Json;

namespace Api.Controllers.Feature.Meeting.Decision
{
    public class MinutzDecisionController : Controller
    {
        private readonly IMinutzDecisionService _minutzDecisionService;
        
        public MinutzDecisionController(IMinutzDecisionService minutzDecisionService)
        {
            _minutzDecisionService = minutzDecisionService;
        }
        
        [Authorize]
        [HttpGet("api/feature/decisions", Name = "Get the decisions for a meeting")]
        public IActionResult GetMeetingDecisionsResult(string meetingId)
        {
            if (string.IsNullOrEmpty(meetingId))
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzDecisionService.GetMeetingDecisions
                (Guid.Parse(meetingId) ,User.ToRest());
            return result.Condition ? Ok(result.DecisionCollection) : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPut("api/feature/decision/quick", Name = "Quick create decision")]
        public IActionResult QuickCreateDecisionResult([FromBody] QuickDecisionRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzDecisionService.QuickDecisionCreate
                (request.MeetingId, request.DescisionText,request.Order ,User.ToRest());
            return result.Condition ? Ok(result.Decision) : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/decision/update", Name = "Update decision for a meeting")]
        public IActionResult UpdateDecisionResult([FromBody] MinutzDecision request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzDecisionService.UpdateDecision
                (request.Id, request ,User.ToRest());
            return result.Condition ? Ok(result.Decision) : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpDelete("api/feature/decision", Name = "Delete decision for a meeting")]
        public IActionResult DeleteDecisionResult(string decisionId)
        {
            if (string.IsNullOrEmpty(decisionId))
                return StatusCode(401, "Request is missing values for the request");
            var result = _minutzDecisionService.DeleteDecision
                (Guid.Parse(decisionId), User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
    }
}