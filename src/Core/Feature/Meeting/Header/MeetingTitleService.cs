using Interface.Repositories.Feature.Meeting.Header;
using Interface.Services;
using Interface.Services.Feature.Meeting.Header;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting.Header
{
    public class MeetingTitleService : IMeetingTitleService
    {
        private readonly IMinutzTitleRepository _minutzTitleRepository;
        private readonly IApplicationSetting _applicationSetting;

        public MeetingTitleService(IMinutzTitleRepository minutzTitleRepository, IApplicationSetting applicationSetting)
        {
            _minutzTitleRepository = minutzTitleRepository;
            _applicationSetting = applicationSetting;
        }

        public MessageBase Update(string meetingId, string title, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _minutzTitleRepository.Update(meetingId, title, user.InstanceId, instanceConnectionString);
        }
    }
}