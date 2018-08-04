using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Feature.Note
{
    public class QuickNoteRequest
    {
        [Required]
        public Guid MeetingId { get; set; }
        [Required]
        public string NoteText { get; set; }
        public int Order { get; set; }
    }
}