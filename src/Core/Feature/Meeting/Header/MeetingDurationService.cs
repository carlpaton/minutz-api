using Interface.Repositories.Feature.Meeting.Header;
using Interface.Services;
using Interface.Services.Feature.Meeting.Header;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting.Header
{
    public class MeetingDurationService: IMeetingDurationService
    {
        private readonly IMeetingDurationRepository _meetingDurationRepository;
        private readonly IApplicationSetting _applicationSetting;

        public MeetingDurationService(IMeetingDurationRepository meetingTimeRepository, IApplicationSetting applicationSetting)
        {
            _meetingDurationRepository = meetingTimeRepository;
            _applicationSetting = applicationSetting;
        }

        public MessageBase Update(string meetingId, int duration, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _meetingDurationRepository.Update(meetingId, duration, user.InstanceId, instanceConnectionString);
        }
    }
}