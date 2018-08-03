using System;
using Interface.Repositories.Feature.Meeting.Decision;
using Interface.Services;
using Interface.Services.Feature.Meeting.Decision;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting.Decision
{
    /// <inheritdoc />
    public class MinutzDecisionService : IMinutzDecisionService
    {
        private readonly IMinutzDecisionRepository _decisionRepository;
        private readonly IApplicationSetting _applicationSetting;

        public MinutzDecisionService(IApplicationSetting applicationSetting,
                                     IMinutzDecisionRepository decisionRepository)
        {
            _decisionRepository = decisionRepository;
            _applicationSetting = applicationSetting;
        }
        
        public DecisionMessage GetMeetingDecisions(Guid meetingId, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _decisionRepository.GetDecisionCollection(meetingId, user.InstanceId, instanceConnectionString);
        }

        public DecisionMessage QuickDecisionCreate(Guid meetingId, string decisionText, int order, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _decisionRepository.QuickCreateDecision(meetingId, decisionText, order, user.InstanceId, instanceConnectionString);
        }

        public DecisionMessage UpdateDecision(Guid meetingId, MinutzDecision decision, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _decisionRepository.UpdateDecision(meetingId, decision, user.InstanceId, instanceConnectionString);
        }
        
        public MessageBase DeleteDecision(Guid decisionId, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _decisionRepository.DeleteDecision(decisionId, user.InstanceId, instanceConnectionString);
        }
    }
}