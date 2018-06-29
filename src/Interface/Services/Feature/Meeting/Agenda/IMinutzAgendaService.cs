using System;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Agenda
{
    public interface IMinutzAgendaService
    {
        MessageBase UpdateComplete(Guid agendaId, bool isComplete, AuthRestModel user);
        MessageBase UpdateOrder(Guid agendaId, int order, AuthRestModel user);
        MessageBase UpdateDuration(Guid agendaId, int duration, AuthRestModel user);
        MessageBase UpdateTitle(Guid agendaId, string title, AuthRestModel user);
        MessageBase UpdateText(Guid agendaId, string text, AuthRestModel user);
        MessageBase UpdateAssignedAttendee(Guid agendaId, string attendeeEmail, AuthRestModel user);
        AgendaMessage QuickCreate(string meetingId, string agendaTitle, int order, AuthRestModel user);
        MessageBase Delete(Guid agendaId, AuthRestModel user);
    }
}