using System;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Note
{
    public interface IMinutzNoteRepository
    {
        NoteMessage GetNoteCollection(Guid meetingId, string schema, string connectionString);

        NoteMessage QuickCreateNote(Guid meetingId, string noteText, int order, string schema, string connectionString);

        NoteMessage UpdateNote(Guid meetingId, MeetingNote note, string schema, string connectionString);

        MessageBase DeleteNote(Guid noteId, string schema, string connectionString);
    }
}