using System;
using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Action
{
    public interface IMinutzActionRepository
    {
        ActionMessage GetMeetingActions(Guid meetingId, string schema, string connectionString);
        MessageBase UpdateActionComplete(Guid actionId, bool isComplete, string schema, string connectionString);
        MessageBase UpdateActionText(Guid actionId, string text, string schema, string connectionString);
        MessageBase UpdateActionAssignedAttendee(Guid actionId, string email, string schema, string connectionString);
        MessageBase UpdateActionDueDate(Guid actionId, DateTime dueDate, string schema, string connectionString);
        MessageBase UpdateActionOrder(Guid actionId, int order, string schema, string connectionString);
        ActionMessage QuickCreate(Guid meetingId, string actionText, int order, string schema, string connectionString);
        MessageBase Delete(Guid actionId, string schema, string connectionString);
    }
}