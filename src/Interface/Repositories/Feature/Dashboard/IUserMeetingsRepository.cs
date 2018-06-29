﻿using Minutz.Models.Message;

namespace Interface.Repositories.Feature.Dashboard
{
    public interface IUserMeetingsRepository
    {
        MeetingMessage Meetings(string email, string schema, string connectionString);
    }
}