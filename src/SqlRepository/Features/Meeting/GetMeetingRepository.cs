using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories.Feature.Meeting;
using Minutz.Models.Message;

namespace SqlRepository.Features.Meeting
{
    public class GetMeetingRepository: IGetMeetingRepository
    {
        public MeetingMessage Get(Guid meetingId, string schema, string connectionString)
        {
            if (meetingId == Guid.Empty ||
                string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid agenda identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var instanceSql = $@"SELECT * FROM [{schema}].[Meeting] WHERE [Id] = '{meetingId}'";
                    var instanceData = dbConnection.Query<Minutz.Models.Entities.Meeting> (instanceSql).FirstOrDefault ();
                    return new MeetingMessage
                           {Code = 200, Condition = true, Message = "Success", Meeting = instanceData};
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