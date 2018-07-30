using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories.Feature.Dashboard;
using Minutz.Models.Message;

namespace SqlRepository.Features.Dashboard {
    public class UserMeetingsRepository : IUserMeetingsRepository {
        public MeetingMessage Meetings (string email, string schema, string connectionString) {
            if (string.IsNullOrEmpty (email) ||
                string.IsNullOrEmpty (schema) ||
                string.IsNullOrEmpty (connectionString))
                throw new ArgumentException ("Please provide a valid meeting identifier, schema or connection string.");
            try {
                using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
                    dbConnection.Open ();
                    var sql = $"SELECT * FROM [{schema}].[Meeting] WHERE MeetingOwnerId = '{email}'";
                    var data = dbConnection.Query<Minutz.Models.Entities.Meeting> (sql);
                    return new MeetingMessage { Code = 200, Condition = true, Message = "Success", Meetings = data };
                }
            } catch (Exception e) {
                Console.WriteLine (e);
                return new MeetingMessage { Code = 500, Condition = false, Message = e.Message };
            }
        }

        public MeetingMessage CreateEmptyUserMeeting (string email, string schema, string connectionString) {
            if (string.IsNullOrEmpty (schema) ||
                string.IsNullOrEmpty (connectionString))
                throw new ArgumentException ("Please provide a valid meeting identifier, schema or connection string.");
            try {
                using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
                    dbConnection.Open ();
                    var Id = Guid.NewGuid ();
                    var insertSql = $@"INSERT INTO [{schema}].[Meeting]
                                    ([Id], [Name], [Location], [Date],[UpdatedDate],[Time],[Duration],[IsReacurance],[IsPrivate],[ReacuranceType],[IsLocked],[IsFormal],[MeetingOwnerId],[Status])
                                    VALUES ('{Id}', 'Minutz','', GETDATE(), GETDATE(),'0:00', 0, 0, 0, 0, 0, 0, '{email}','create');";
                    var instanceSql = $"SELECT * FROM [{schema}].[Meeting] WHERE [Id] = '{Id}'";
                    var insertData = dbConnection.Execute (insertSql);
                    if (insertData == 1) {
                        var insertAttendeeSql = $@"INSERT INTO [{schema}].[MeetingAttendee]
                                                ([Id],[ReferanceId], [PersonIdentity], [Email], [Status], [Role])
                                                VALUES(NEWID(), NEWID() , 'leeroya@gmail.com', 'leeroya@gmail.com', 'Accepted', 'Owner');";
                        var insertAttendeeData = dbConnection.Execute (insertAttendeeSql);
                        var data = dbConnection.Query<Minutz.Models.Entities.Meeting> (instanceSql).FirstOrDefault ();
                        return new MeetingMessage { Code = 200, Condition = true, Message = "Success", Meeting = data };
                    }
                    return new MeetingMessage { Code = 404, Condition = false, Message = "Could not quick create meeting." };
                }
            } catch (Exception e) {
                Console.WriteLine (e);
                return new MeetingMessage { Code = 500, Condition = false, Message = e.Message };
            }
        }

        public MeetingMessage Meeting (Guid meetingId, string schema, string connectionString) {
            if (meetingId == Guid.Empty ||
                string.IsNullOrEmpty (schema) ||
                string.IsNullOrEmpty (connectionString))
                throw new ArgumentException ("Please provide a valid meeting identifier, schema or connection string.");
            try {
                using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
                    dbConnection.Open ();
                    var instanceSql = $"SELECT * FROM [{schema}].[Meeting] WHERE [Id] = '{meetingId}'";
                    var data = dbConnection.Query<Minutz.Models.Entities.Meeting> (instanceSql).FirstOrDefault ();
                    return new MeetingMessage { Code = 200, Condition = true, Message = "Success", Meeting = data };
                }
            } catch (Exception e) {
                Console.WriteLine (e);
                return new MeetingMessage { Code = 500, Condition = false, Message = e.Message };
            }
        }
    }
}