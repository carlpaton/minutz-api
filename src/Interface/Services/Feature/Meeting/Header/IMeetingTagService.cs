using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Header
{
    public interface IMeetingTagService
    {
        MessageBase Update(string meetingId, string tags, AuthRestModel user);
    }
}