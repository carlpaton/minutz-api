using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories.Feature.Meeting.Agenda;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace SqlRepository.Features.Meeting.Agenda
{
    public class MinutzAgendaRepository : IMinutzAgendaRepository
    {
        public MessageBase UpdateComplete(Guid agendaId, bool isComplete, string schema, string connectionString)
        {
            if (agendaId == Guid.Empty ||
                string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid agenda identifier, schema or connection string.");
            try
            {
                var value = Convert.ToInt32(isComplete);
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var sql = $"UPDATE [{schema}].[MeetingAgenda] SET [IsComplete] = {value} WHERE Id = '{agendaId}'";
                    var data = dbConnection.Execute(sql);
                    return data == 1
                        ? new MessageBase {Code = 200, Condition = true, Message = "Success"}
                        : new MessageBase {Code = 404, Condition = false, Message = "Could not update agenda isComplete."};
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new MessageBase {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        public MessageBase UpdateOrder(Guid agendaId, int order, string schema, string connectionString)
        {
            if (agendaId == Guid.Empty ||
                string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid agenda identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var sql = $"UPDATE [{schema}].[MeetingAgenda] SET [Order] = {order} WHERE Id = '{agendaId}'";
                    var data = dbConnection.Execute(sql);
                    return data == 1
                        ? new MessageBase {Code = 200, Condition = true, Message = "Success"}
                        : new MessageBase {Code = 404, Condition = false, Message = "Could not update agenda order."};
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new MessageBase {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        public MessageBase UpdateDuration(Guid agendaId, int duration, string schema, string connectionString)
        {
            if (agendaId == Guid.Empty ||
                string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid agenda identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var sql = $"UPDATE [{schema}].[MeetingAgenda] SET [Duration] = {duration} WHERE Id = '{agendaId}'";
                    var data = dbConnection.Execute(sql);
                    return data == 1
                        ? new MessageBase {Code = 200, Condition = true, Message = "Success"}
                        : new MessageBase {Code = 404, Condition = false, Message = "Could not update agenda duration."};
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new MessageBase {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        public MessageBase UpdateTitle(Guid agendaId, string title, string schema, string connectionString)
        {
            if (agendaId == Guid.Empty ||
                string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid agenda identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var sql = $"UPDATE [{schema}].[MeetingAgenda] SET [AgendaHeading] = '{title}' WHERE Id = '{agendaId}'";
                    var data = dbConnection.Execute(sql);
                    return data == 1
                        ? new MessageBase {Code = 200, Condition = true, Message = "Success"}
                        : new MessageBase {Code = 404, Condition = false, Message = "Could not update agenda title."};
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new MessageBase {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        public MessageBase UpdateText(Guid agendaId, string text, string schema, string connectionString)
        {
            if (agendaId == Guid.Empty ||
                string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid agenda identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var sql = $"UPDATE [{schema}].[MeetingAgenda] SET [AgendaText] = '{text}' WHERE Id = '{agendaId}'";
                    var data = dbConnection.Execute(sql);
                    return data == 1
                        ? new MessageBase {Code = 200, Condition = true, Message = "Success"}
                        : new MessageBase {Code = 404, Condition = false, Message = "Could not update agenda text."};
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new MessageBase {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        public MessageBase UpdateAssignedAttendee(Guid agendaId, string attendeeEmail, string schema, string connectionString)
        {
            if (agendaId == Guid.Empty ||
                string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid agenda identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var sql = $"UPDATE [{schema}].[MeetingAgenda] SET [MeetingAttendeeId] = '{attendeeEmail}' WHERE Id = '{agendaId}'";
                    var data = dbConnection.Execute(sql);
                    return data == 1
                        ? new MessageBase {Code = 200, Condition = true, Message = "Success"}
                        : new MessageBase {Code = 404, Condition = false, Message = "Could not update agenda assigned attendee."};
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new MessageBase {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        public AgendaMessage QuickCreate(string meetingId, string agendaTitle, int order, string schema, string connectionString)
        {
            if (string.IsNullOrEmpty(meetingId) ||
                string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid agenda identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var id = Guid.NewGuid();
                    dbConnection.Open();
                    var insertSql = $@"INSERT INTO [{schema}].[MeetingAgenda]([Id],[ReferanceId],[AgendaHeading],[Order]) 
                                 VALUES('{id}','{meetingId}','{agendaTitle}', {order} )";
                    var insertData = dbConnection.Execute(insertSql);
                    if (insertData == 1)
                    {
                        var instanceSql = $@"SELECT * FROM [{schema}].[MeetingAgenda] WHERE [Id] = '{id}'";
                        var instanceData = dbConnection.Query<MeetingAgenda>(instanceSql).FirstOrDefault();
                        if (instanceData == null) return  new AgendaMessage {Code = 404, Condition = false, Message = "Could not find quick create agenda item."};
                        return new AgendaMessage {Code = 200, Condition = true, Message = "Success", Agenda = instanceData };
                    }
                    return  new AgendaMessage {Code = 404, Condition = false, Message = "Could not quick create agenda."};
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new AgendaMessage {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        public MessageBase Delete(Guid agendaId, string schema, string connectionString)
        {
            if (agendaId == Guid.Empty ||
                string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid agenda identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var sql = $@"DELETE FROM [{schema}].[MeetingAgenda] WHERE [Id] = '{agendaId}' ";
                    var data = dbConnection.Execute(sql);
                    return data == 1
                        ? new MessageBase {Code = 200, Condition = true, Message = "Success"}
                        : new MessageBase {Code = 404, Condition = false, Message = "Could not delte agenda."};
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