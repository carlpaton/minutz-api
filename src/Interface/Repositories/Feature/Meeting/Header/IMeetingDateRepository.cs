using System;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Header
{
    public interface IMeetingDateRepository
    {
        MessageBase Update(string meetingId, DateTime date,  string schema, string connectionString);
    }
}