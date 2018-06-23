using Interface.Repositories.Feature.Meeting.Header;
using Interface.Services;
using Interface.Services.Feature.Meeting.Header;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting.Header
{
    public class MeetingTitleService : IMeetingTitleService
    {
        private readonly IMeetingTitleRepository _meetingTitleRepository;
        private readonly IApplicationSetting _applicationSetting;

        public MeetingTitleService(IMeetingTitleRepository meetingTitleRepository, IApplicationSetting applicationSetting)
        {
            _meetingTitleRepository = meetingTitleRepository;
            _applicationSetting = applicationSetting;
        }

        public MessageBase Update(string meetingId, string title, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            return _meetingTitleRepository.Update(meetingId, title, user.InstanceId, instanceConnectionString);
        }
    }
}