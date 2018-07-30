using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories.Feature.Meeting.Attachment;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace SqlRepository.Features.Meeting.Attachment
{
    public class MinutzMeetingAttachmentRepository : IMinutzMeetingAttachmentRepository
    {
        public AttachmentMessage Get(Guid meetingId, string schema, string connectionString)
        {
            if (meetingId == Guid.Empty ||
                string.IsNullOrEmpty (schema) ||
                string.IsNullOrEmpty (connectionString))
                throw new ArgumentException ("Please provide a valid agenda identifier, schema or connection string.");
            try {
                using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
                    dbConnection.Open ();
                    var instanceSql = $@"SELECT * FROM [{schema}].[MeetingAttachment] WHERE [ReferanceId] = '{meetingId}'";
                    var instanceData = dbConnection.Query<MeetingAttachment> (instanceSql).ToList();
                        return new AttachmentMessage { Code = 200, Condition = true, Message = "Success", AttachmentCollection = instanceData };
                }
            } catch (Exception e) {
                Console.WriteLine (e);
                return new AttachmentMessage { Code = 500, Condition = false, Message = e.Message };
            } 
        }

        public AttachmentMessage Add(Guid meetingId, string fileName,int order, string schema, string connectionString)
        {
            if (meetingId == Guid.Empty ||
                string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid agenda identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var date = DateTime.UtcNow;
                    var id = Guid.NewGuid();
                    dbConnection.Open();
                    var sql = $@"INSERT [{schema}].[MeetingAttachment] 
                                (Id, ReferanceId, FileName, Date, [Order])
                                VALUES 
                                ('{id}', '{meetingId}', '{fileName}', '{date}', {order} )";
                    var data = dbConnection.Execute(sql);
                    return data == 1
                        ? new AttachmentMessage {
                            Code = 200, Condition = true, Message = "Success", Attachment = 
                            new MeetingAttachment
                            {
                                Date = date,
                                FileData = null,
                                FileName = fileName,
                                Id = id,
                                MeetingAttendeeId = "",
                                ReferanceId = meetingId,
                                Order = order
                            }}
                        : new AttachmentMessage
                        {
                            Code = 404,
                            Condition = false,
                            Message = "Could not update attachment ."
                        };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new AttachmentMessage {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        public MessageBase Update(Guid meetingId, string fileName,int order, string schema, string connectionString)
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
                    var sql = $@"UPDATE [{schema}].[MeetingAttachment] 
                                 SET [Order] = {order}, [FileName]= '{fileName}'  WHERE Id = '{meetingId}'";
                    var data = dbConnection.Execute(sql);
                    return data == 1
                        ? new MessageBase {Code = 200, Condition = true, Message = "Success"}
                        : new MessageBase
                        {
                            Code = 404,
                            Condition = false,
                            Message = "Could not insert attachment."
                        };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new MessageBase {Code = 500, Condition = false, Message = e.Message};
            }
        }
    }
}