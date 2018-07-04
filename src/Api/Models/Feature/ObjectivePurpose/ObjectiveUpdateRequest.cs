using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Feature.ObjectivePurpose
{
    public class ObjectiveUpdateRequest
    {
        [Required]
        public Guid id {get;set;}
        public string Objective { get; set; }
    }
}