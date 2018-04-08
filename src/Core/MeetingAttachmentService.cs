using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Entities;

namespace Core
{
    public class MeetingAttachmentService: IMeetingAttachmentService
    {
        private readonly IMeetingAttachmentRepository _meetingAttachmentRepository;
        private readonly IApplicationSetting _applicationSetting;

        public MeetingAttachmentService
            (IMeetingAttachmentRepository meetingAttachmentRepository,
            IApplicationSetting applicationSetting)
        {
            _meetingAttachmentRepository = meetingAttachmentRepository;
            _applicationSetting = applicationSetting;
        }

        public (bool condition, string message) Add
            (MeetingAttachment attachment, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            
            var result = _meetingAttachmentRepository.Add(attachment, user.InstanceId, instanceConnectionString);
            if (result)
            {
                return (true, "Success");
            }

            return (false, "Something went wrong");
        }

        public (bool condition, string message) Update
            (MeetingAttachment attachment, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            
            var result = _meetingAttachmentRepository.Update(attachment, user.InstanceId, instanceConnectionString);
            if (result)
            {
                return (true, "Success");
            }

            return (false, "Something went wrong");
        }
    }
}