using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Feature.Action
{
    public class QuickActionRequest
    {
        [Required]
        public Guid MeetingId { get; set; }
        [Required]
        public string ActionText { get; set; }
        public int Order { get; set; }
    }
}