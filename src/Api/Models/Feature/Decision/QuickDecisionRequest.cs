using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Feature.Decision
{
    public class QuickDecisionRequest
    {
        [Required]
        public Guid MeetingId { get; set; }
        [Required]
        public string DescisionText { get; set; }
        public int Order { get; set; }
    }
}