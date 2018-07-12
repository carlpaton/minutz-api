using System;
using Interface.Repositories.Feature.Meeting;
using Interface.Services;
using Interface.Services.Feature.Meeting;
using Minutz.Models.Message;

namespace Core.Feature.Meeting
{
    public class GetMeetingService: IGetMeetingService
    {
        private readonly IApplicationSetting _applicationSetting;
        private readonly IGetMeetingRepository _getMeetingRepository;

        public GetMeetingService(IApplicationSetting applicationSetting, IGetMeetingRepository getMeetingRepository)
        {
            _applicationSetting = applicationSetting;
            _getMeetingRepository = getMeetingRepository;
        }

        public MeetingMessage GetMeeting(string instanceId, Guid meetingId)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, instanceId, _applicationSetting.GetInstancePassword(instanceId));
            return _getMeetingRepository.Get(meetingId, instanceId, instanceConnectionString);
        }
    }
}