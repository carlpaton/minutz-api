using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Feature.Note
{
    public class QuickNoteRequest
    {
        [Required]
        public Guid Id { get; set; }
        public dynamic Value { get; set; }
    }
}