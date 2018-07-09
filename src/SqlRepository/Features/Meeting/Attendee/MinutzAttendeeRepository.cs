using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories.Feature.Meeting;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace SqlRepository.Features.Meeting.Attendee
{
    public class MinutzAttendeeRepository: IMinutzAttendeeRepository
    {
        /// <summary>
        /// Get the attendees for a meeting
        /// </summary>
        /// <param name="meetingId">Current meeting Id</param>
        /// <param name="schema">Current instance id</param>
        /// <param name="connectionString">Instance connection string</param>
        /// <returns>Collection of meeting attendees</returns>
        /// <exception cref="ArgumentException"></exception>
        public AttendeeMessage GetAttendees(Guid meetingId, string schema, string connectionString)
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
                    var instanceSql = $@"SELECT * FROM [{schema}].[MeetingAttendee] WHERE [referanceId] = '{meetingId}'";
                    var instanceData = dbConnection.Query<MeetingAttendee> (instanceSql).ToList();
                    
                    var peopleSql = $@"SELECT * FROM [{schema}].[Person]";
                    var peopleData = dbConnection.Query<Person>(peopleSql).ToList();

                    var attendees = new List<MeetingAttendee>();
                    foreach (var attendee in instanceData)
                    {
                        var att = new MeetingAttendee
                                  {
                                      PersonIdentity = attendee.PersonIdentity,
                                      Id = attendee.Id,
                                      Role = attendee.Role,
                                      Status = attendee.Status,
                                      Email = attendee.Email
                                  };
                        var person = peopleData.FirstOrDefault(i => i.Identityid == attendee.PersonIdentity);
                        if (person != null)
                        {
                            att.Name = string.IsNullOrEmpty(person.FullName)
                                ? $"{person.FirstName} {person.LastName}"
                                : person.FullName;
                            att.Picture = person.ProfilePicture;
                        }

                        attendees.Add(att);
                    }
                    
                    
                    return new AttendeeMessage
                           {
                               Code = 200,
                               Condition = true,
                               Message = "Success",
                               Attendees = attendees
                           };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new AttendeeMessage {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        /// <summary>
        /// Add an attendee into the database
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="attendee">Full attendee entity</param>
        /// <param name="schema">Current instance id</param>
        /// <param name="connectionString">Full connection string for instance</param>
        /// <returns>Attendee Message with the attendee</returns>
        /// <exception cref="ArgumentException"></exception>
        public AttendeeMessage AddAttendee(Guid meetingId, MeetingAttendee attendee, string schema, string connectionString)
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
                    var insertSql = $@"INSERT INTO [{schema}].[MeetingAttendee]
                                        ([Id],[ReferanceId],[PersonIdentity],[Email],[Role],[Status]) 
                                        VALUES
                                        ('{attendee.Id}','{meetingId}','{attendee.PersonIdentity}','{attendee.Email}','{attendee.Role}','{attendee.Status}')
                                        ";

                    var data = dbConnection.Execute(insertSql);
                    return data == 1
                        ? new AttendeeMessage {Code = 200, Condition = true, Message = "Success", Attendee = attendee}
                        : new AttendeeMessage
                          {
                              Code = 404,
                              Condition = false,
                              Message = "Could not insert attendee."
                          };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new AttendeeMessage {Code = 500, Condition = false, Message = e.Message};
            }
        }

        public AttendeeMessage UpdateAttendee(Guid meetingId, MeetingAttendee attendee, string schema, string connectionString)
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
                    var insertSql = $@"UPDATE [{schema}].[MeetingAttendee]
                                        SET [Role] = '{attendee.Role}' ,[Status] = '{attendee.Status}' 
                                        WHERE [referanceId] = '{attendee.ReferenceId}' AND [PersonIdentity] = '{attendee.PersonIdentity}'
                                        ";

                    var data = dbConnection.Execute(insertSql);
                    return data == 1
                        ? new AttendeeMessage {Code = 200, Condition = true, Message = "Success", Attendee = attendee}
                        : new AttendeeMessage
                          {
                              Code = 404,
                              Condition = false,
                              Message = "Could not insert attendee."
                          };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new AttendeeMessage {Code = 500, Condition = false, Message = e.Message};
            }
        }

        public MessageBase DeleteAttendee(Guid meetingId,string attendeeEmail ,string schema, string connectionString)
        {
            if (meetingId == Guid.Empty ||
                string.IsNullOrEmpty(attendeeEmail) ||
                string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid agenda identifier, attendee email, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var instanceSql = $@"DELETE FROM [{schema}].[MeetingAttendee] 
                                         WHERE [personidentity] = '{attendeeEmail}' 
                                         AND [referanceId]= '{meetingId}' ";
                    var data = dbConnection.Execute(instanceSql);
                    return data == 1
                        ? new MessageBase {Code = 200, Condition = true, Message = "Success"}
                        : new MessageBase
                          {
                              Code = 404,
                              Condition = false,
                              Message = "Could not remove attendee."
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