using System;
using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Meeting.Header
{
    public interface IMinutzDateRepository
    {
        MessageBase Update(string meetingId, DateTime date,  string schema, string connectionString);
    }
}