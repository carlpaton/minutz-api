using System;
using Interface.Repositories.Feature.Meeting;
using Interface.Services;
using Interface.Services.Feature.Meeting;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting
{
    public class MeetingObjectivePurposeService: IMeetingObjectivePurposeService
    {
        private readonly IApplicationSetting _applicationSetting;
        private readonly IMeetingObjectivePurposeRepository _meetingObjectivePurposeRepository;

        public MeetingObjectivePurposeService(IApplicationSetting applicationSetting,
                                              IMeetingObjectivePurposeRepository meetingObjectivePurposeRepository)
        {
            _applicationSetting = applicationSetting;
            _meetingObjectivePurposeRepository = meetingObjectivePurposeRepository;
        }

        public MeetingMessage UpdateObjective(Guid meetingId, string objective, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString (_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword (user.InstanceId));
            return _meetingObjectivePurposeRepository.UpdateObjective(meetingId, objective, user.InstanceId,
                instanceConnectionString);
        }

        public MeetingMessage UpdatePurpose(Guid meetingId, string purpose, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString (_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword (user.InstanceId));
            return _meetingObjectivePurposeRepository.UpdatePurpose(meetingId, purpose, user.InstanceId,
                instanceConnectionString);
        }
    }
}