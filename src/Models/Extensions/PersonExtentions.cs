using System.Collections.Generic;
using Minutz.Models.Entities;

namespace Minutz.Models.Extensions
{
    public static class PersonExtentions
    {
        public static List<(string instanceId, string meetingId)> RelatedItems(this Person person)
        {
            return person.Related.SplitToList (StringDeviders.InstanceStringDevider, StringDeviders.MeetingStringDevider);
        }
    }
}

