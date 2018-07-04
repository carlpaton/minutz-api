using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Feature.ObjectivePurpose
{
    public class PurposeUpdateRequest
    {
        [Required]
        public Guid id {get;set;}
        public string Purpose { get; set; }
    }
}