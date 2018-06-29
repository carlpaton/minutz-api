using System.ComponentModel.DataAnnotations;

namespace Api.Models.Feature.Agenda
{
    public class QuickAgendaRequest
    {
        [Required]
        public string MeetingId { get; set; }
        [Required]
        public string AgendaTitle { get; set; }
        [Required]
        public int Order { get; set; }
    }
}
