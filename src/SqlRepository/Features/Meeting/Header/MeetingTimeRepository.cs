﻿using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Interface.Repositories.Feature.Meeting.Header;
using Minutz.Models.Message;

namespace SqlRepository.Features.Meeting.Header
{
    public class MinutzTimeRepository : IMinutzTimeRepository
    {
        public MessageBase Update(string meetingId, string time, string schema, string connectionString)
        {
            if ( string.IsNullOrEmpty(meetingId) ||string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var sql = $"UPDATE [{schema}].[Meeting] SET Time ='{time}' WHERE Id = '{meetingId}'";
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