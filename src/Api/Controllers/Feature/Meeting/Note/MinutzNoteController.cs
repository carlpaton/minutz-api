using System;
using Api.Extensions;
using Api.Models.Feature.Note;
using Interface.Services.Feature.Meeting.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minutz.Models.Entities;
using Newtonsoft.Json;

namespace Api.Controllers.Feature.Meeting.Note
{
    public class MinutzNoteController : Controller
    {
        private readonly IMinutzNoteService _noteService;

        public MinutzNoteController(IMinutzNoteService noteService)
        {
            _noteService = noteService;
        }
        
        [Authorize]
        [HttpGet("api/feature/notes", Name = "Get the notes for a meeting")]
        public IActionResult GetMeetingNotesResult(string meetingId)
        {
            if (string.IsNullOrEmpty(meetingId))
                return StatusCode(401, "Request is missing values for the request");
            var result = _noteService.GetMeetingNotes
                (Guid.Parse(meetingId) ,User.ToRest());
            return result.Condition ? Ok(result.NoteCollection) : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPut("api/feature/note/quick", Name = "Quick create note")]
        public IActionResult QuickCreateNoteResult([FromBody] QuickNoteRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var result = _noteService.QuickNoteCreate
                (request.Id, request.Value.DescisionText,request.Value.Order ,User.ToRest());
            return result.Condition ? Ok(result.Note) : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpPost("api/feature/note/update", Name = "Update note for a meeting")]
        public IActionResult UpdateNoteResult([FromBody] QuickNoteRequest request)
        {
            if (!ModelState.IsValid)
                return StatusCode(401, "Request is missing values for the request");
            var model = JsonConvert.DeserializeObject<MeetingNote>(request.Value);
            var result = _noteService.UpdateNote
                (request.Id, model ,User.ToRest());
            return result.Condition ? Ok(result.Note) : StatusCode(result.Code, result.Message);
        }
        
        [Authorize]
        [HttpDelete("api/feature/note", Name = "Delete note for a meeting")]
        public IActionResult DeleteDecisionResult(string noteId)
        {
            if (string.IsNullOrEmpty(noteId))
                return StatusCode(401, "Request is missing values for the request");
            var result = _noteService.DeleteNote
                (Guid.Parse(noteId), User.ToRest());
            return result.Condition ? (IActionResult) Ok() : StatusCode(result.Code, result.Message);
        }
    }
}