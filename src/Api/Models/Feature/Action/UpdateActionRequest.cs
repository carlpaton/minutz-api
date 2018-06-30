using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Feature.Action
{
    public class UpdateActionRequest
    {
        [Required]
        public Guid Id { get; set; }
        public dynamic Value { get; set; }
    }
}