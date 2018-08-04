using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories.Feature.Meeting.Decision;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace SqlRepository.Features.Meeting.Decision
{
    public class MinutzDecisionRepository : IMinutzDecisionRepository
    {
        public DecisionMessage GetDecisionCollection(Guid meetingId, string schema, string connectionString)
        {
            if (meetingId == Guid.Empty ||string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var sql = $"SELECT * FROM [{schema}].[MinutzDecision] WHERE [ReferanceId] = '{meetingId}'";
                    var instanceData = dbConnection.Query<MinutzDecision> (sql).ToList();
                    return new DecisionMessage
                           {
                               Code = 200,
                               Condition = true,
                               Message = "Success",
                               DecisionCollection = instanceData
                           };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DecisionMessage {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        public DecisionMessage QuickCreateDecision(Guid meetingId, string decisionText, int order, string schema, string connectionString)
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
                        $@"INSERT INTO [{schema}].[MinutzDecision]
                           ([Id],[ReferanceId],[DescisionText],[CreatedDate],[IsOverturned],[Order]) 
                           VALUES('{id}','{meetingId}','{decisionText}','{DateTime.UtcNow}', 0 ,{order} )";
                    
                    var insertData = dbConnection.Execute(insertSql);
                    
                    if (insertData == 1)
                    {
                        var instanceSql = $@"SELECT * FROM [{schema}].[MinutzDecision] WHERE [Id] = '{id}'";
                        var instanceData = dbConnection.Query<MinutzDecision>(instanceSql).FirstOrDefault();
                        if (instanceData == null)
                            return new DecisionMessage
                                   {
                                       Code = 404,
                                       Condition = false,
                                       Message = "Could not find quick create decision item."
                                   };
                        return new DecisionMessage
                               {
                                   Code = 200,
                                   Condition = true,
                                   Message = "Success",
                                   Decision = instanceData
                               };
                    }
                    return new DecisionMessage
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
                return new DecisionMessage {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        public DecisionMessage UpdateDecision(Guid meetingId, MinutzDecision decision, string schema, string connectionString)
        {
            if (meetingId == Guid.Empty ||string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var overturned = Convert.ToByte(decision.IsOverturned);
                    var sql = string.Empty;
                    if (string.IsNullOrEmpty(decision.PersonId))
                    {
                        sql = $@"UPDATE [{schema}].[MinutzDecision] SET
                                   [DescisionText] = '{decision.DescisionText}',
                                   [Descisioncomment] = '{decision.Descisioncomment}',
                                   [AgendaId] = '{decision.AgendaId}',
                                   [CreatedDate] = '{decision.CreatedDate}',
                                   [IsOverturned] = {overturned},
                                   [Order] = {decision.Order} 
                                 WHERE Id = '{meetingId}'";
                    }
                    else
                    {
                        sql = $@"UPDATE [{schema}].[MinutzDecision] SET
                                   [DescisionText] = '{decision.DescisionText}',
                                   [Descisioncomment] = '{decision.Descisioncomment}',
                                   [AgendaId] = '{decision.AgendaId}',
                                   [PersonId] = '{decision.PersonId}',
                                   [CreatedDate] = '{decision.CreatedDate}',
                                   [IsOverturned] = {overturned},
                                   [Order] = {decision.Order} 
                                 WHERE Id = '{meetingId}'";
                    }
                    var data = dbConnection.Execute(sql);
                    return data == 1 
                        ? new DecisionMessage{ Code = 200, Condition =  true, Message = "Success"} 
                        : new DecisionMessage{ Code = 404, Condition =  false, Message = "Could not update meeting."};
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DecisionMessage {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        public MessageBase DeleteDecision(Guid decisionId, string schema, string connectionString)
        {
            if (decisionId == Guid.Empty  ||string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var sql = $"DELETE FROM [{schema}].[MinutzDecision] WHERE Id = '{decisionId}'";
                    var data = dbConnection.Execute(sql);
                    return data == 1 
                        ? new DecisionMessage{ Code = 200, Condition =  true, Message = "Success"} 
                        : new DecisionMessage{ Code = 404, Condition =  false, Message = "Could not update meeting."};
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DecisionMessage {Code = 500, Condition = false, Message = e.Message};
            }
        }
    }
}