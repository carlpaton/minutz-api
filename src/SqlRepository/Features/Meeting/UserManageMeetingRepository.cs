using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Interface.Repositories.Feature.Meeting;
using Minutz.Models.Message;

namespace SqlRepository.Features.Meeting
{
    public class UserManageMeetingRepository: IUserManageMeetingRepository
    {
        public MeetingMessage Update(Minutz.Models.Entities.Meeting meeting, string schema, string connectionString)
        {
            if (meeting.Id == Guid.Empty ||
                string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid agenda identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    var isReacurance = Convert.ToInt32(meeting.IsReacurance);
                    var isPrivate = Convert.ToInt32(meeting.IsPrivate);
                    var isLocked = Convert.ToInt32(meeting.IsLocked);
                    var isFormal = Convert.ToInt32(meeting.IsFormal);
                    dbConnection.Open();
                    var sql = $@"UPDATE [{schema}].[Meeting] 
                                    SET [Name] = '{meeting.Name}',
                                    SET [Location] = '{meeting.Location}',
                                    SET [Date] = '{meeting.Date}',
                                    SET [UpdatedDate] = '{DateTime.UtcNow}',
                                    SET [Time] = '{meeting.Time};,
                                    SET [Duration] = {meeting.Duration},
                                    SET [IsReacurance] = {isReacurance},
                                    SET [IsPrivate] = {isPrivate},
                                    SET [ReacuranceType] = {meeting.ReacuranceType},
                                    SET [IsLocked] = {isLocked},
                                    SET [IsFormal] = {isFormal},
                                    SET [TimeZone] = '{meeting.TimeZone}',
                                    SET [Tag] = '{meeting.Tag}',
                                    SET [Purpose] = '{meeting.Purpose}',
                                    SET [Status] = '{meeting.Status}',
                                    SET [MeetingOwnerId] = '{meeting.MeetingOwnerId}',
                                    SET [RecurrenceData] = '{meeting.ReacuranceType}',
                                    SET [Outcome] = '{meeting.Outcome}'
                                   WHERE Id = '{meeting.Id}'";
                    var data = dbConnection.Execute(sql);
                    return data == 1
                        ? new MeetingMessage {Code = 200, Condition = true, Message = "Success", Meeting = meeting}
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