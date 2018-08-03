using System;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Note
{
    public interface IMinutzNoteService
    {
        NoteMessage GetMeetingNotes(Guid meetingId, AuthRestModel user);

        NoteMessage QuickNoteCreate(Guid meetingId, string noteText, int order, AuthRestModel user);

        NoteMessage UpdateNote(Guid meetingId, MeetingNote note, AuthRestModel user);

        MessageBase DeleteNote(Guid noteId, AuthRestModel user);
    }
}