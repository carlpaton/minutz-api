using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories.Feature.Meeting.Action;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace SqlRepository.Features.Meeting.Action {
    /// <inheritdoc />
    public class MinutzActionRepository : IMinutzActionRepository {

        public ActionMessage GetMeetingActions (Guid meetingId, string schema, string connectionString) {
            if (meetingId == Guid.Empty ||
                string.IsNullOrEmpty (schema) ||
                string.IsNullOrEmpty (connectionString))
                throw new ArgumentException ("Please provide a valid agenda identifier, schema or connection string.");
            try {
                using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
                    dbConnection.Open ();
                    var instanceSql = $@"SELECT * FROM [{schema}].[MinutzAction] WHERE [ReferanceId] = '{meetingId}'";
                    var instanceData = dbConnection.Query<MinutzAction> (instanceSql);
                    foreach (var action in instanceData)
                    {
                        action.ActionTitle = action.ActionText.Length < 5 ? 
                            $"{action.ActionText.Substring(0, action.ActionText.Length)}..." 
                            : $"{action.ActionText.Substring(0, 5)}...";
                    }
                    return new ActionMessage { Code = 200, Condition = true, Message = "Success", Actions = instanceData };
                }
            } catch (Exception e) {
                Console.WriteLine (e);
                return new ActionMessage { Code = 500, Condition = false, Message = e.Message };
            }
        }

        public MessageBase UpdateActionComplete (Guid actionId, bool isComplete, string schema, string connectionString) {
            if (actionId == Guid.Empty ||
                string.IsNullOrEmpty (schema) ||
                string.IsNullOrEmpty (connectionString))
                throw new ArgumentException ("Please provide a valid agenda identifier, schema or connection string.");
            try {
                var value = Convert.ToInt32 (isComplete);
                using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
                    dbConnection.Open ();
                    var sql = $"UPDATE [{schema}].[MinutzAction] SET [IsComplete] = {value} WHERE Id = '{actionId}'";
                    var data = dbConnection.Execute (sql);
                    return data == 1 ?
                        new MessageBase { Code = 200, Condition = true, Message = "Success" } :
                        new MessageBase { Code = 404, Condition = false, Message = "Could not update action isComplete." };
                }
            } catch (Exception e) {
                Console.WriteLine (e);
                return new MessageBase { Code = 500, Condition = false, Message = e.Message };
            }
        }

        public MessageBase UpdateActionText (Guid actionId, string text, string schema, string connectionString) {
            if (actionId == Guid.Empty ||
                string.IsNullOrEmpty (schema) ||
                string.IsNullOrEmpty (connectionString))
                throw new ArgumentException ("Please provide a valid agenda identifier, schema or connection string.");
            try {
                using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
                    dbConnection.Open ();
                    var sql = $"UPDATE [{schema}].[MinutzAction] SET [ActionText] = '{text}' WHERE Id = '{actionId}'";
                    var data = dbConnection.Execute (sql);
                    return data == 1 ?
                        new MessageBase { Code = 200, Condition = true, Message = "Success" } :
                        new MessageBase { Code = 404, Condition = false, Message = "Could not update action text." };
                }
            } catch (Exception e) {
                Console.WriteLine (e);
                return new MessageBase { Code = 500, Condition = false, Message = e.Message };
            }
        }
        
        public MessageBase UpdateActionTitle (Guid actionId, string text, string schema, string connectionString) {
            if (actionId == Guid.Empty ||
                string.IsNullOrEmpty (schema) ||
                string.IsNullOrEmpty (connectionString))
                throw new ArgumentException ("Please provide a valid agenda identifier, schema or connection string.");
            try {
                using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
                    dbConnection.Open ();
                    var sql = $"UPDATE [{schema}].[MinutzAction] SET [ActionText] = '{text}' WHERE Id = '{actionId}'";
                    var data = dbConnection.Execute (sql);
                    return data == 1 ?
                        new MessageBase { Code = 200, Condition = true, Message = "Success" } :
                        new MessageBase { Code = 404, Condition = false, Message = "Could not update action text." };
                }
            } catch (Exception e) {
                Console.WriteLine (e);
                return new MessageBase { Code = 500, Condition = false, Message = e.Message };
            }
        }

        public MessageBase UpdateActionAssignedAttendee (Guid actionId, string personId, string schema, string connectionString) {
            if (actionId == Guid.Empty ||
                string.IsNullOrEmpty (schema) ||
                string.IsNullOrEmpty (connectionString))
                throw new ArgumentException ("Please provide a valid agenda identifier, schema or connection string.");
            try {
                using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
                    dbConnection.Open ();
                    var sql = $"UPDATE [{schema}].[MinutzAction] SET [PersonId] = '{personId}' WHERE Id = '{actionId}'";
                    var data = dbConnection.Execute (sql);
                    return data == 1 ?
                        new MessageBase { Code = 200, Condition = true, Message = "Success" } :
                        new MessageBase { Code = 404, Condition = false, Message = "Could not update action assigned attendee." };
                }
            } catch (Exception e) {
                Console.WriteLine (e);
                return new MessageBase { Code = 500, Condition = false, Message = e.Message };
            }
        }

        public MessageBase UpdateActionDueDate (Guid actionId, DateTime dueDate, string schema, string connectionString) {
            if (actionId == Guid.Empty ||
                string.IsNullOrEmpty (schema) ||
                string.IsNullOrEmpty (connectionString))
                throw new ArgumentException ("Please provide a valid agenda identifier, schema or connection string.");
            try {
                using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
                    dbConnection.Open ();
                    var sql = $"UPDATE [{schema}].[MinutzAction] SET [DueDate] = '{dueDate}' WHERE Id = '{actionId}'";
                    var data = dbConnection.Execute (sql);
                    return data == 1 ?
                        new MessageBase { Code = 200, Condition = true, Message = "Success" } :
                        new MessageBase { Code = 404, Condition = false, Message = "Could not update action due date." };
                }
            } catch (Exception e) {
                Console.WriteLine (e);
                return new MessageBase { Code = 500, Condition = false, Message = e.Message };
            }
        }
        
        public MessageBase UpdateActionRaisedDate (Guid actionId, DateTime raisedDate, string schema, string connectionString) {
            if (actionId == Guid.Empty ||
                string.IsNullOrEmpty (schema) ||
                string.IsNullOrEmpty (connectionString))
                throw new ArgumentException ("Please provide a valid agenda identifier, schema or connection string.");
            try {
                using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
                    dbConnection.Open ();
                    var sql = $"UPDATE [{schema}].[MinutzAction] SET [CreatedDate] = '{raisedDate}' WHERE Id = '{actionId}'";
                    var data = dbConnection.Execute (sql);
                    return data == 1 ?
                        new MessageBase { Code = 200, Condition = true, Message = "Success" } :
                        new MessageBase { Code = 404, Condition = false, Message = "Could not update action due date." };
                }
            } catch (Exception e) {
                Console.WriteLine (e);
                return new MessageBase { Code = 500, Condition = false, Message = e.Message };
            }
        }

        public MessageBase UpdateActionOrder (Guid actionId, int order, string schema, string connectionString) {
            if (actionId == Guid.Empty ||
                string.IsNullOrEmpty (schema) ||
                string.IsNullOrEmpty (connectionString))
                throw new ArgumentException ("Please provide a valid agenda identifier, schema or connection string.");
            try {
                using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
                    dbConnection.Open ();
                    var sql = $"UPDATE [{schema}].[MinutzAction] SET [Order] = {order} WHERE Id = '{actionId}'";
                    var data = dbConnection.Execute (sql);
                    return data == 1 ?
                        new MessageBase { Code = 200, Condition = true, Message = "Success" } :
                        new MessageBase { Code = 404, Condition = false, Message = "Could not update action order." };
                }
            } catch (Exception e) {
                Console.WriteLine (e);
                return new MessageBase { Code = 500, Condition = false, Message = e.Message };
            }
        }

        public ActionMessage QuickCreate (Guid meetingId, string actionText, int order, string schema, string connectionString) {
            if (meetingId == Guid.Empty ||
                string.IsNullOrEmpty (schema) ||
                string.IsNullOrEmpty (connectionString))
                throw new ArgumentException ("Please provide a valid agenda identifier, schema or connection string.");
            try {
                using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
                    var id = Guid.NewGuid ();
                    dbConnection.Open ();
                    var insertSql = $@"INSERT INTO [{schema}].[MinutzAction]
                                 ([Id],[ReferanceId],[ActionText],[Order], [CreatedDate], [IsComplete]) 
                                 VALUES('{id}','{meetingId}','{actionText}', {order},'{DateTime.UtcNow}', 0 )";
                    var insertData = dbConnection.Execute (insertSql);
                    if (insertData == 1) {
                        var instanceSql = $@"SELECT * FROM [{schema}].[MinutzAction] WHERE [Id] = '{id}'";
                        var instanceData = dbConnection.Query<MinutzAction> (instanceSql).FirstOrDefault ();
                        if (instanceData == null) return new ActionMessage { Code = 404, Condition = false, Message = "Could not find quick create action item." };
                        instanceData.ActionTitle = $"{instanceData.ActionText.Substring(0, 5)}...";
                        return new ActionMessage { Code = 200, Condition = true, Message = "Success", Action = instanceData };
                    }
                    return new ActionMessage { Code = 404, Condition = false, Message = "Could not quick create agenda." };
                }
            } catch (Exception e) {
                Console.WriteLine (e);
                return new ActionMessage { Code = 500, Condition = false, Message = e.Message };
            }
        }

        public MessageBase Delete (Guid actionId, string schema, string connectionString) {
            if (actionId == Guid.Empty ||
                string.IsNullOrEmpty (schema) ||
                string.IsNullOrEmpty (connectionString))
                throw new ArgumentException ("Please provide a valid agenda identifier, schema or connection string.");
            try {
                using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
                    dbConnection.Open ();
                    var sql = $@"DELETE FROM [{schema}].[MinutzAction] WHERE [Id] = '{actionId}' ";
                    var data = dbConnection.Execute (sql);
                    return data == 1 ?
                        new MessageBase { Code = 200, Condition = true, Message = "Success" } :
                        new MessageBase { Code = 404, Condition = false, Message = "Could not delete action." };
                }
            } catch (Exception e) {
                Console.WriteLine (e);
                return new MessageBase { Code = 500, Condition = false, Message = e.Message };
            }
        }
    }
}