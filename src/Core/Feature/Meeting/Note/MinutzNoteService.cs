using System;
using Interface.Repositories.Feature.Meeting.Note;
using Interface.Services;
using Interface.Services.Feature.Meeting.Note;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting.Note
{
    /// <inheritdoc />
    public class MinutzNoteService : IMinutzNoteService
    {
        private readonly IMinutzNoteRepository _noteRepository;
        private readonly IApplicationSetting _applicationSetting;

        public MinutzNoteService(IApplicationSetting applicationSetting,
                                     IMinutzNoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
            _applicationSetting = applicationSetting;
        }
        
        public NoteMessage GetMeetingNotes(Guid meetingId, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _noteRepository.GetNoteCollection(meetingId, user.InstanceId, instanceConnectionString);
        }

        public NoteMessage QuickNoteCreate(Guid meetingId, string noteText, int order, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _noteRepository.QuickCreateNote(meetingId, noteText, order, user.InstanceId, instanceConnectionString);
        }

        public NoteMessage UpdateNote(Guid meetingId, MeetingNote note, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _noteRepository.UpdateNote(meetingId, note, user.InstanceId, instanceConnectionString);
        }
        
        public MessageBase DeleteNote(Guid noteId, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _noteRepository.DeleteNote(noteId, user.InstanceId, instanceConnectionString);
        }
    }
}