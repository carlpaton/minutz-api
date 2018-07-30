using System;
using Interface.Repositories.Feature.Meeting.Attachment;
using Interface.Services;
using Interface.Services.Feature.Meeting.Attachment;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Core.Feature.Meeting.Attachment
{
    public class MinutzMeetingAttachmentService: IMinutzMeetingAttachmentService
    {
        private readonly IApplicationSetting _applicationSetting;
        private readonly IMinutzMeetingAttachmentRepository _meetingAttachmentRepository;

        public MinutzMeetingAttachmentService
            (IApplicationSetting applicationSetting,  IMinutzMeetingAttachmentRepository meetingAttachmentRepository)
        {
            _applicationSetting = applicationSetting;
            _meetingAttachmentRepository = meetingAttachmentRepository;
        }
        
        public AttachmentMessage Get(Guid meetingId, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));

            return _meetingAttachmentRepository.Get(meetingId, user.InstanceId,
                instanceConnectionString);
        }
        
        public AttachmentMessage Add(Guid meetingId, string fileUrl, int order, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));

            return _meetingAttachmentRepository.Add(meetingId, fileUrl, order, user.InstanceId,
                instanceConnectionString);
        }
        
        public MessageBase Update(Guid meetingId, string fileUrl, int order, AuthRestModel user)
        {
            var instanceConnectionString = _applicationSetting.CreateConnectionString(_applicationSetting.Server,
                _applicationSetting.Catalogue, user.InstanceId, _applicationSetting.GetInstancePassword(user.InstanceId));
            
            return _meetingAttachmentRepository.Update(meetingId, fileUrl, order, user.InstanceId,
                instanceConnectionString);
        }
    }
}