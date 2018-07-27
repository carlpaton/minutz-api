using System;
using Interface.Repositories.Feature.Meeting.Status;
using Interface.Services;
using Interface.Services.Feature.Meeting.Status;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting.Status
{
    public class MeetingStatusService : IMeetingStatusService
    {
        private readonly IApplicationSetting _applicationSetting;
        private readonly IMeetingStatusRepository _meetingStatusRepository;

        public MeetingStatusService(IApplicationSetting applicationSetting, IMeetingStatusRepository meetingStatusRepository)
        {
            _applicationSetting = applicationSetting;
            _meetingStatusRepository = meetingStatusRepository;
        }

        public MessageBase UpdateMeetingStatus(Guid meetingId, string status, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _meetingStatusRepository.UpdateMeetingStatus(meetingId, status, user.InstanceId,
                instanceConnectionString);
        }
    }
}