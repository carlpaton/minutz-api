using System;
using Interface.Repositories.Feature.Meeting.Action;
using Interface.Services;
using Interface.Services.Feature.Meeting.Action;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting.Action
{
    public class MinutzActionService : IMinutzActionService
    {
        private readonly IMinutzActionRepository _minutzActionRepository;
        private readonly IApplicationSetting _applicationSetting;

        public MinutzActionService(IMinutzActionRepository minutzActionRepository,
                                   IApplicationSetting applicationSetting)
        {
            _minutzActionRepository = minutzActionRepository;
            _applicationSetting = applicationSetting;
        }

        public ActionMessage GetMeetingActions(Guid meetingId, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzActionRepository.GetMeetingActions(meetingId, user.InstanceId,instanceConnectionString);
        }

        public MessageBase UpdateActionComplete(Guid actionId, bool isComplete, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzActionRepository.UpdateActionComplete(actionId, isComplete, user.InstanceId,
                instanceConnectionString);
        }

        public MessageBase UpdateActionText(Guid actionId, string text, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzActionRepository.UpdateActionText(actionId, text, user.InstanceId, instanceConnectionString);
        }

        public MessageBase UpdateActionAssignedAttendee(Guid actionId, string email, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzActionRepository.UpdateActionAssignedAttendee(actionId, email, user.InstanceId,
                instanceConnectionString);
        }

        public MessageBase UpdateActionDueDate(Guid actionId, DateTime dueDate, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzActionRepository.UpdateActionDueDate(actionId, dueDate, user.InstanceId,
                instanceConnectionString);
        }

        public MessageBase UpdateActionOrder(Guid actionId, int order, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzActionRepository.UpdateActionOrder(actionId, order, user.InstanceId,
                instanceConnectionString);
        }

        public ActionMessage QuickCreate(Guid meetingId, string actionText, int order, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzActionRepository.QuickCreate(meetingId, actionText, order, user.InstanceId,
                instanceConnectionString);
        }

        public MessageBase Delete(Guid actionId, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzActionRepository.Delete(actionId, user.InstanceId, instanceConnectionString);
        }
    }
}