using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Minutz.Models.Message
{
    public class MeetingMessage : MessageBase
    {
        public Meeting Meeting { get; set; }
        public IEnumerable<Meeting> Meetings { get; set; }
    }
}