using System;
using System.Collections.Generic;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;

namespace Core
{
    public class MeetingActionService : IMeetingActionService
    {
        private readonly ILogService _logService;
        private readonly IApplicationSetting _applicationSetting;
        private readonly IMeetingActionRepository _actionRepository;
        

        public MeetingActionService
            (ILogService logService, IApplicationSetting applicationSetting, IMeetingActionRepository actionRepository)
        {
            _logService = logService;
            _applicationSetting = applicationSetting;
            _actionRepository = actionRepository;
        }

        public (bool condition, string message , IEnumerable<MinutzAction> data) GetMinutzActions
            (string referenceId, AuthRestModel user)
        {
            if (string.IsNullOrEmpty(referenceId))
            {
                throw new ArgumentNullException(nameof(referenceId), "Please provide a valid reference id.");
            }
      
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
          
            var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);

            if (referenceId != user.InstanceId)
            {
                var actions =
                    _actionRepository.GetMeetingActions(Guid.Parse(referenceId), user.InstanceId, instanceConnectionString);
                return (true, "",actions);
            }

            // check if referenceId is a meetingViewModel id
            // if id is a meetingViewModel id then check if meetingViewModel has actions for user

            // if meetingViewModel is not a meetingViewModel id [referenceId] then use it as the user Id and check for actions - these become tasks
            return (true, "", new List<MinutzAction>());
        }

        public (bool condition, string message, MinutzAction value) CreateMinutzAction
            (string referenceId, MinutzAction action ,AuthRestModel user)
        {
            if (action.Id == Guid.Parse("e38b69b3-8f2a-4979-9323-1819db4331f8"))
            {
                action.Id = Guid.NewGuid();
            }

            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId,
                _applicationSetting.GetInstancePassword(user.InstanceId));
            var result = _actionRepository.Add(action, user.InstanceId, instanceConnectionString);
            return (result,result ? "Success": "Failed",action);
        }

        public (bool condition, string message, MinutzAction value) UpdateMinutzAction
            (string referenceId, MinutzAction action,AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId,
                _applicationSetting.GetInstancePassword(user.InstanceId));
            var result = _actionRepository.Update(action, user.InstanceId, instanceConnectionString);
            return (result,result ? "Success": "Failed", action);
        }

        public (bool condition, string message) DeleteMinutzAction
            (string referenceId, string actionId, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId,
                _applicationSetting.GetInstancePassword(user.InstanceId));
            var result = _actionRepository.Delete(Guid.Parse(actionId), user.InstanceId, instanceConnectionString);
            return (true, "");
        }

    }
}