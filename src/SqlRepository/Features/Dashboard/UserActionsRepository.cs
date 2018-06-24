using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Interface.Repositories.Feature.Dashboard;
using Minutz.Models.Message;

namespace SqlRepository.Features.Dashboard
{
    public class UserActionsRepository: IUserActionsRepository
    {
        public ActionMessage Actions(string email, string schema, string connectionString)
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
                    var sql = $@"SELECT
                                    A.Id,
                                    A.ReferanceId,
                                    PersonIdentity,
                                    Email,
                                    Status,
                                    Role,
                                    MA.Id,
                                    MA.ReferanceId,
                                    ActionText,
                                    PersonId,
                                    CreatedDate,
                                    DueDate,
                                    IsComplete
                                FROM [A_bfc5ab54_76d8_4cff_b526_b589bfe4e929].[AvailibleAttendee] A
                                INNER JOIN A_bfc5ab54_76d8_4cff_b526_b589bfe4e929.MeetingAction MA
                                ON A.Id = MA.PersonId WHERE A.Email = '{email}';
                                ";
                    var data = dbConnection.Query<Minutz.Models.Entities.MinutzAction>(sql);
                    return new ActionMessage { Code = 200, Condition = true, Message = "Success", Actions = data};
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ActionMessage {Code = 500, Condition = false, Message = e.Message};
            }
        }
    }
}