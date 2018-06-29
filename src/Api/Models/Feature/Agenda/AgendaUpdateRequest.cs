using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Feature.Agenda
{
    public class AgendaUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }
        public dynamic Value { get; set; }
    }
}