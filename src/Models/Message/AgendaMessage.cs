using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Minutz.Models.Message
{
    public class AgendaMessage: MessageBase
    {
        public List<MeetingAgenda> AgendaCollection { get; set; }
        public MeetingAgenda Agenda { get; set; }
    }
}