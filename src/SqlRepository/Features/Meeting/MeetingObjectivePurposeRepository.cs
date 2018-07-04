using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Interface.Repositories.Feature.Meeting;
using Minutz.Models.Message;
using System.Linq;

namespace SqlRepository.Features.Meeting
{
    public class MeetingObjectivePurposeRepository :IMeetingObjectivePurposeRepository
    {
        public MeetingMessage UpdateObjective(Guid meetingId, string objective, string schema, string connectionString)
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
                    var sql = $@"UPDATE [{schema}].[Meeting] 
                                    SET [Outcome] = '{objective}'
                                   WHERE Id = '{meetingId}'";
                    var data = dbConnection.Execute(sql);
                    var instanceSql = $@"SELECT * FROM [{schema}].[Meeting] WHERE [Id] = '{meetingId}'";
                    var instanceData = dbConnection.Query<Minutz.Models.Entities.Meeting> (instanceSql).FirstOrDefault ();
                    return data == 1
                        ? new MeetingMessage {Code = 200, Condition = true, Message = "Success", Meeting = instanceData}
                        : new MeetingMessage
                          {
                              Code = 404,
                              Condition = false,
                              Message = "Could not update meeting values."
                          };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new MeetingMessage {Code = 500, Condition = false, Message = e.Message};
            }
        }

        public MeetingMessage UpdatePurpose(Guid meetingId, string purpose, string schema, string connectionString)
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
                    var sql = $@"UPDATE [{schema}].[Meeting] 
                                    SET [Purpose] = '{purpose}'
                                   WHERE Id = '{meetingId}'";
                    var data = dbConnection.Execute(sql);
                    var instanceSql = $@"SELECT * FROM [{schema}].[Meeting] WHERE [Id] = '{meetingId}'";
                    var instanceData = dbConnection.Query<Minutz.Models.Entities.Meeting> (instanceSql).FirstOrDefault ();
                    return data == 1
                        ? new MeetingMessage {Code = 200, Condition = true, Message = "Success", Meeting = instanceData}
                        : new MeetingMessage
                          {
                              Code = 404,
                              Condition = false,
                              Message = "Could not update meeting values."
                          };
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