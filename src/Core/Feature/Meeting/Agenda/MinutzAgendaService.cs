using System;
using System.Collections.Generic;
using System.Linq;
using Interface.Repositories.Feature.Meeting.Agenda;
using Interface.Services;
using Interface.Services.Feature.Meeting.Agenda;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting.Agenda
{
    public class MinutzAgendaService: IMinutzAgendaService
    {
        private readonly IApplicationSetting _applicationSetting;
        private readonly IMinutzAgendaRepository _minutzAgendaRepository;

        public MinutzAgendaService(IApplicationSetting applicationSetting, IMinutzAgendaRepository minutzAgendaRepository)
        {
            _applicationSetting = applicationSetting;
            _minutzAgendaRepository = minutzAgendaRepository;
        }

        public AgendaMessage GetMeetingAgendaCollection(Guid meetingId, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            var data = _minutzAgendaRepository.GetMeetingAgendaCollection(meetingId, user.InstanceId, instanceConnectionString);
            if (data.Condition)
            {
                if (data.AgendaCollection.Any())
                {
                    var updated = new List<MeetingAgenda>();
                    foreach (var agenda in data.AgendaCollection)
                    {
                        if (agenda.Order == 0)
                        {
                            agenda.Order = updated.Count;
                        }
                        updated.Add(agenda);
                    }
                    data.AgendaCollection = updated;
                    data.AgendaCollection = data.AgendaCollection.OrderBy(i => i.Order).ToList();
                }
            }
            return new AgendaMessage{ Condition = data.Condition, Message = data.Message, AgendaCollection = data.AgendaCollection};
        }

        public MessageBase UpdateComplete(Guid agendaId, bool isComplete, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzAgendaRepository.UpdateComplete(agendaId, isComplete, user.InstanceId, instanceConnectionString);
        }

        public MessageBase UpdateOrder(Guid agendaId, int order, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzAgendaRepository.UpdateOrder(agendaId, order, user.InstanceId, instanceConnectionString);
        }

        public MessageBase UpdateDuration(Guid agendaId, int duration, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzAgendaRepository.UpdateDuration(agendaId, duration, user.InstanceId, instanceConnectionString);
        }

        public MessageBase UpdateTitle(Guid agendaId, string title, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzAgendaRepository.UpdateTitle(agendaId, title, user.InstanceId, instanceConnectionString);
        }

        public MessageBase UpdateText(Guid agendaId, string text, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzAgendaRepository.UpdateText(agendaId, text, user.InstanceId, instanceConnectionString);
        }

        public MessageBase UpdateAssignedAttendee(Guid agendaId, string attendeeEmail, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzAgendaRepository.UpdateAssignedAttendee(agendaId, attendeeEmail, user.InstanceId, instanceConnectionString);
        }

        public AgendaMessage QuickCreate(string meetingId, string agendaTitle, int order, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzAgendaRepository.QuickCreate(meetingId, agendaTitle, order, user.InstanceId, instanceConnectionString);
        }

        public MessageBase Delete(Guid agendaId, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzAgendaRepository.Delete(agendaId, user.InstanceId, instanceConnectionString);
        }
    }
}