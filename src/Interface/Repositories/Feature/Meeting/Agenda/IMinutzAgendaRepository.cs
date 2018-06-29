using System;
using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Agenda
{
    public interface IMinutzAgendaRepository
    {
        MessageBase UpdateComplete(Guid agendaId, bool isComplete, string schema, string connectionString);
        MessageBase UpdateOrder(Guid agendaId, int order, string schema, string connectionString);
        MessageBase UpdateDuration(Guid agendaId, int duration, string schema, string connectionString);
        MessageBase UpdateTitle(Guid agendaId, string title, string schema, string connectionString);
        MessageBase UpdateText(Guid agendaId, string text, string schema, string connectionString);
        MessageBase UpdateAssignedAttendee(Guid agendaId, string attendeeEmail, string schema, string connectionString);

        MessageBase QuickCreate(string meetingId, string agendaTitle, int order, string schema, string connectionString);
        MessageBase Delete(Guid agendaId, string schema, string connectionString);
    }
}