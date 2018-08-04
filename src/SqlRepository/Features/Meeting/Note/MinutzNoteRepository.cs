using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories.Feature.Meeting.Note;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace SqlRepository.Features.Meeting.Note
{
    /// <inheritdoc />
    public class MinutzNoteRepository : IMinutzNoteRepository
    {
        public NoteMessage GetNoteCollection(Guid meetingId, string schema, string connectionString)
        {
            if (meetingId == Guid.Empty ||string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var sql = $"SELECT * FROM [{schema}].[MeetingNote] WHERE [ReferanceId] = '{meetingId}'";
                    var instanceData = dbConnection.Query<MeetingNote> (sql).ToList();
                    return new NoteMessage
                           {
                               Code = 200,
                               Condition = true,
                               Message = "Success",
                               NoteCollection = instanceData
                           };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new NoteMessage {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        public NoteMessage QuickCreateNote(Guid meetingId, string noteText, int order, string schema, string connectionString)
        {
            if (meetingId == Guid.Empty ||string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var id = Guid.NewGuid();
                    var insertSql =
                        $@"INSERT INTO [{schema}].[MeetingNote]
                          (Id, ReferanceId, NoteText, CreatedDate, [Order]) 
                          VALUES('{id}','{meetingId}','{noteText}','{DateTime.UtcNow}',{order} )";
                    var insertData = dbConnection.Execute(insertSql);
                    if (insertData == 1)
                    {
                        var instanceSql = $@"SELECT * FROM [{schema}].[MeetingNote] WHERE [Id] = '{id}'";
                        var instanceData = dbConnection.Query<MeetingNote>(instanceSql).FirstOrDefault();
                        if (instanceData == null)
                            return new NoteMessage
                                   {
                                       Code = 404,
                                       Condition = false,
                                       Message = "Could not find quick create decision item."
                                   };
                        return new NoteMessage
                               {
                                   Code = 200,
                                   Condition = true,
                                   Message = "Success",
                                   Note = instanceData
                               };
                    }
                    return new NoteMessage
                           {
                               Code = 404,
                               Condition = false,
                               Message = "Could not quick create decision."
                           };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new NoteMessage {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        public NoteMessage UpdateNote(Guid meetingId, MeetingNote note, string schema, string connectionString)
        {
            if (meetingId == Guid.Empty ||string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var sql = string.Empty;
                    if (string.IsNullOrEmpty(note.MeetingAttendeeId))
                    {
                        sql = $@"UPDATE [{schema}].[MeetingNote] SET
                                   [CreatedDate] = '{note.CreatedDate}',
                                   [NoteText] = '{note.NoteText}',
                                   [Order] = {note.Order}
                                 WHERE Id = '{meetingId}'";
                    }
                    else
                    {
                        sql = $@"UPDATE [{schema}].[MeetingNote] SET
                                   [CreatedDate] = '{note.CreatedDate}',
                                   [NoteText] = '{note.NoteText}',
                                   [MeetingAttendeeId] = '{note.MeetingAttendeeId}',
                                   [Order] = {note.Order}
                                 WHERE Id = '{meetingId}'";
                    }

                    
                    var data = dbConnection.Execute(sql);
                    return data == 1 
                        ? new NoteMessage{ Code = 200, Condition =  true, Message = "Success"} 
                        : new NoteMessage{ Code = 404, Condition =  false, Message = "Could not update meeting."};
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new NoteMessage {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        public MessageBase DeleteNote(Guid noteId, string schema, string connectionString)
        {
            if (noteId == Guid.Empty  ||string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var sql = $"DELETE FROM [{schema}].[MeetingNote] WHERE Id = '{noteId}'";
                    var data = dbConnection.Execute(sql);
                    return data == 1 
                        ? new MessageBase{ Code = 200, Condition =  true, Message = "Success"} 
                        : new MessageBase{ Code = 404, Condition =  false, Message = "Could not update meeting."};
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