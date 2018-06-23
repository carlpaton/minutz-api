using Interface.Repositories.Feature.Meeting.Header;
using Interface.Services;
using Interface.Services.Feature.Meeting.Header;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting.Header
{
    public class MeetingLocationService: IMeetingLocationService
    {
        private readonly IMeetingLocationRepository _meetingLocationRepository;
        private readonly IApplicationSetting _applicationSetting;

        public MeetingLocationService(IMeetingLocationRepository meetingLocationRepository, IApplicationSetting applicationSetting)
        {
            _meetingLocationRepository = meetingLocationRepository;
            _applicationSetting = applicationSetting;
        }
        
        public MessageBase Update(string meetingId, string location, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _meetingLocationRepository.Update(meetingId, location, user.InstanceId, instanceConnectionString);
        }
    }
}