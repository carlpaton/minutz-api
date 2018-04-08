using Minutz.Models.Entities;

namespace Interface.Services
{
    public interface IMeetingAttachmentService
    {
        (bool condition, string message) Add
            (MeetingAttachment attachment, AuthRestModel user);
        
        (bool condition, string message) Update
            (MeetingAttachment attachment, AuthRestModel user);
    }
}