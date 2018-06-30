using System;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Action
{
    public interface IMinutzActionService
    {
        MessageBase UpdateActionComplete(Guid actionId, bool isComplete, AuthRestModel user);
        MessageBase UpdateActionText(Guid actionId, string text, AuthRestModel user);
        MessageBase UpdateActionAssignedAttendee(Guid actionId, string email, AuthRestModel user);
        MessageBase UpdateActionDueDate(Guid actionId, DateTime dueDate, AuthRestModel user);
        MessageBase UpdateActionOrder(Guid actionId, int order, AuthRestModel user);
        ActionMessage QuickCreate(Guid meetingId, string actionText, int order, AuthRestModel user);
        MessageBase Delete(Guid actionId, AuthRestModel user);
    }
}