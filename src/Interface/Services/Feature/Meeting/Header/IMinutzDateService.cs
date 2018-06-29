using System;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Services.Feature.Meeting.Header
{
    public interface IMinutzDateService
    {
        MessageBase Update(string meetingId, DateTime date, AuthRestModel user);
    }
}