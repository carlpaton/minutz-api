using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Interface.Repositories.Feature.Dashboard;
using Minutz.Models.Message;

namespace SqlRepository.Features.Dashboard
{
    public class UserMeetingsRepository : IUserMeetingsRepository
    {
        public MeetingMessage Meetings(string email, string schema, string connectionString)
        {
            if (string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var sql = $"SELECT * FROM [{schema}].[Meeting] WHERE MeetingOwnerId = '{email}'";
                    var data = dbConnection.Query<Minutz.Models.Entities.Meeting>(sql);
                    return new MeetingMessage { Code = 200, Condition = true, Message = "Success", Meetings = data};
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new MeetingMessage {Code = 500, Condition = false, Message = e.Message};
            }
        }
    }
}