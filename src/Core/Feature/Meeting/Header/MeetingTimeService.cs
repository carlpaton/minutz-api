using Interface.Repositories.Feature.Meeting.Header;
using Interface.Services;
using Interface.Services.Feature.Meeting.Header;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting.Header
{
    public class MeetingTimeService : IMeetingTimeService
    {
        private readonly IMeetingTimeRepository _meetingTimeRepository;
        private readonly IApplicationSetting _applicationSetting;

        public MeetingTimeService(IMeetingTimeRepository meetingTimeRepository, IApplicationSetting applicationSetting)
        {
            _meetingTimeRepository = meetingTimeRepository;
            _applicationSetting = applicationSetting;
        }

        public MessageBase Update(string meetingId, string time, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _meetingTimeRepository.Update(meetingId, time, user.InstanceId, instanceConnectionString);
        }
    }
}