using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Feature.Decision
{
    public class UpdateDecisionRequest
    {
        [Required]
        public Guid Id { get; set; }
        public dynamic Value { get; set; }
    }
}