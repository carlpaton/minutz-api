using System;
using System.Collections.Generic;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;

namespace Core
{
    public class MeetingDecisionService : IMeetingDecisionService
    {
        private readonly ILogService _logService;
        private readonly IApplicationSetting _applicationSetting;
        private readonly IDecisionRepository _decisionRepository;

        public MeetingDecisionService
            (IDecisionRepository decisionRepository, ILogService logService, IApplicationSetting applicationSetting)
        {
            _decisionRepository = decisionRepository;
            _logService = logService;
            _applicationSetting = applicationSetting;
        }

        public (bool condition, string message, IEnumerable<MinutzDecision> data) GetMinutzDecisions
            (string referenceId, AuthRestModel user)
        {
            if (string.IsNullOrEmpty(referenceId))
            {
                throw new ArgumentNullException(nameof(referenceId), "Please provide a valid reference id.");
            }

            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId,
                _applicationSetting.GetInstancePassword(user.InstanceId));

            var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
            var decisions =
                _decisionRepository.GetMeetingDecisions(Guid.Parse(referenceId), user.InstanceId,
                    instanceConnectionString);
            return (true, "", decisions);
        }

        public (bool condition, string message, MinutzDecision value) CreateMinutzDecision
            (string referenceId, MinutzDecision decision, AuthRestModel user)
        {
            if (decision.Id == Guid.Parse("e38b69b3-8f2a-4979-9323-1819db4331f8"))
            {
                decision.Id = Guid.NewGuid();
            }

            if (string.IsNullOrEmpty(referenceId))
            {
                throw new ArgumentNullException(nameof(referenceId), "Please provide a valid reference id.");
            }

            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId,
                _applicationSetting.GetInstancePassword(user.InstanceId));

            var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
            var result = _decisionRepository.Add(decision, user.InstanceId, instanceConnectionString);
            return (result, result ? "Success" : "Failed", decision);
        }

        public (bool condition, string message, MinutzDecision value) UpdateMinutzDecision
            (string referenceId, MinutzDecision decision, AuthRestModel user)
        {
            if (string.IsNullOrEmpty(referenceId))
            {
                throw new ArgumentNullException(nameof(referenceId), "Please provide a valid reference id.");
            }

            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId,
                _applicationSetting.GetInstancePassword(user.InstanceId));

            var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
            var result = _decisionRepository.Update(decision, user.InstanceId, instanceConnectionString);
            return (result, result ? "Success" : "Failed", decision);
        }

        public (bool condition, string message) DeleteMinutzDecision
            (string referenceId, string decisionId, AuthRestModel user)
        {
            if (string.IsNullOrEmpty(referenceId))
            {
                throw new ArgumentNullException(nameof(referenceId), "Please provide a valid reference id.");
            }

            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId,
                _applicationSetting.GetInstancePassword(user.InstanceId));

            var masterConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, _applicationSetting.Username, _applicationSetting.Password);
            var result = _decisionRepository.Delete(Guid.Parse(decisionId), user.InstanceId, instanceConnectionString);
            return (result, result ? "Success" : "Failed");
        }
    }
}