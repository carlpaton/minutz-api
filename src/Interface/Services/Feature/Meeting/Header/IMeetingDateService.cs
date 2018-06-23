using System;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Header
{
    public interface IMeetingDateService
    {
        MessageBase Update(string meetingId, DateTime date, AuthRestModel user);
    }
}