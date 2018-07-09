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
    /// <summary>
    /// The Class that Manages the available attendees for a instance 
    /// </summary>
    public class MinutzAvailabilityRepository : IMinutzAvailabilityRepository
    {
        /// <summary>
        /// Get the available attendees and convert them to Attendees Full Detail
        /// </summary>
        /// <param name="schema">Instance Schema for the account</param>
        /// <param name="connectionString">Built connection string for instance</param>
        /// <returns>Collection of MeetingAttendees</returns>
        /// <exception cref="ArgumentException"></exception>
        public AttendeeMessage GetAvailableAttendees(string schema, string connectionString)
        {
            if (string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid agenda identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var instanceSql = $@"SELECT * FROM [{schema}].[AvailibleAttendee]";
                    var instanceData = dbConnection.Query<Minutz.Models.Entities.AvailibleAttendee>(instanceSql)
                        .ToList();

                    var peopleSql = $@"SELECT * FROM [{schema}].[Person]";
                    var peopleData = dbConnection.Query<Minutz.Models.Entities.Person>(peopleSql).ToList();

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
    }
}