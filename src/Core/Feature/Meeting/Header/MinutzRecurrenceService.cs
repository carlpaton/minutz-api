using System;
using Interface.Repositories.Feature.Meeting.Header;
using Interface.Services;
using Interface.Services.Feature.Meeting.Header;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting.Header
{
    public class MinutzRecurrenceService: IMinutzRecurrenceService
    {
        private readonly IApplicationSetting _applicationSetting;
        private readonly IMeetingRecurrenceRepository _meetingRecurrenceRepository;

        public MinutzRecurrenceService(IApplicationSetting applicationSetting, IMeetingRecurrenceRepository meetingRecurrenceRepository)
        {
            _applicationSetting = applicationSetting;
            _meetingRecurrenceRepository = meetingRecurrenceRepository;
        }
        
        public MessageBase Update(string meetingId, int recurrence, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _meetingRecurrenceRepository.Update(meetingId, recurrence, user.InstanceId, instanceConnectionString);
        }
    }
}