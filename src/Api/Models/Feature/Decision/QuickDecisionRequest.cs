using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Feature.Decision
{
    public class QuickDecisionRequest
    {
        [Required]
        public Guid Id { get; set; }
        public dynamic Value { get; set; }
    }
}