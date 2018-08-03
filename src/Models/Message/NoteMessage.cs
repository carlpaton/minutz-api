using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Minutz.Models.Message
{
    public class NoteMessage: MessageBase
    {
        public List<MeetingNote> NoteCollection { get; set; }
        public MeetingNote Note { get; set; }
    }
}