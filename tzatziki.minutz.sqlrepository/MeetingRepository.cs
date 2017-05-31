using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.sqlrepository
{
  public class MeetingRepository
  {
    public Meeting UpdateMeeting(string connectionString, string schema, Meeting meeting)
    {
      return ToMeeting(connectionString, schema, meeting);
    }

    internal List<Meeting> ToList(string schema, string connectionString)
    {
      var result = new List<Meeting>();
      using (SqlConnection con = new SqlConnection(connectionString))
      {
        using (SqlCommand command = new SqlCommand(SelectMeetingStatement(schema), con))
        {
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              result.Add(ToMeeting(reader));
            }
          }
        }
      }
      return result;
    }

    internal Meeting ToMeeting(string connectionString, string schema, Meeting meeting)
    {
      using (SqlConnection con = new SqlConnection(connectionString))
      {
        using (SqlCommand command = new SqlCommand(UpdateMeetingStatement(schema, meeting), con))
        {
          try
          {
            command.ExecuteNonQuery();
          }
          catch (Exception ex)
          {
            throw new Exception($"Issue updating the meeting record. {ex.Message}", ex.InnerException);
          }
        }
        return ToList(schema, connectionString).FirstOrDefault(i => i.Id == meeting.Id);
      }
    }

    internal string SelectMeetingStatement(string schema)
    {
      return $"SELECT * FROM [{schema}].[Meeting] ";
    }

    internal string UpdateMeetingStatement(string schema, Meeting meeting)
    {
      var isFormal = meeting.IsFormal == true ? 1 : 0;
      var isReacurance = meeting.IsReacurance == true ? 1 : 0;
      var isPrivate = meeting.IsPrivate == true ? 1 : 0;
      var isLocked = meeting.IsLocked == true ? 1 : 0;
      return $@"UPDATE [{schema}].[Meeting]
        SET [Name] = {meeting.Name},
        [Location] ={meeting.Location},
        [Date] = {meeting.Date},
        [Time] = {meeting.Time},
        [Duration] = {meeting.Duration},
        [IsReacurance] = {isReacurance},
        [IsPrivate] = {isPrivate},
        [ReacuranceType] = {meeting.ReacuranceType},
        [IsLocked] = {isLocked},
        [IsFormal] = {isFormal},
        [TimeZone] = {meeting.TimeZone},
        [Tag] = {string.Join(",", meeting.Tag)},
        [Purpose] = {meeting.Purpose},
        [Outcome] = {meeting.Outcome}
        WHERE [Id] = {meeting.Id}";
    }

    internal string InsertMeetingStatement(string schema, Meeting meeting)
    {
      var isFormal = meeting.IsFormal == true ? 1 : 0;
      var isReacurance = meeting.IsReacurance == true ? 1 : 0;
      var isPrivate = meeting.IsPrivate == true ? 1 : 0;
      var isLocked = meeting.IsLocked == true ? 1 : 0;
      return $@"INSERT INTO [{schema}].[Meeting] VALUES (
        [Id] = {meeting.Id},
        [Name] = {meeting.Name},
        [Location] ={meeting.Location},
        [Date] = {meeting.Date},
        [Time] = {meeting.Time},
        [Duration] = {meeting.Duration},
        [IsReacurance] = {isReacurance},
        [IsPrivate] = {isPrivate},
        [ReacuranceType] = {meeting.ReacuranceType},
        [IsLocked] = {isLocked},
        [IsFormal] = {isFormal},
        [TimeZone] = {meeting.TimeZone},
        [Tag] = {string.Join(",", meeting.Tag)},
        [Purpose] = {meeting.Purpose},
        [Outcome] = {meeting.Outcome}
        )";
    }

    internal Meeting ToMeeting(SqlDataReader dataReader)
    {
      return new Meeting
      {
        Id = Guid.Parse(dataReader["Id"].ToString()),
        Name = dataReader["Name"].ToString(),
        Location = dataReader["Location"].ToString(),
        Date = DateTime.Parse(dataReader["FirstName"].ToString()),
        Time = dataReader["Time"].ToString(),
        Duration = Int32.Parse(dataReader["Duration"].ToString()),
        IsReacurance = bool.Parse(dataReader["IsReacurance"].ToString()),
        IsPrivate = bool.Parse(dataReader["IsPrivate"].ToString()),
        IsLocked = bool.Parse(dataReader["IsLocked"].ToString()),
        IsFormal = bool.Parse(dataReader["IsFormal"].ToString()),
        ReacuranceType = dataReader["ReacuranceType"].ToString(),
        Tag = dataReader["Tag"].ToString().Split(','),
        Purpose = dataReader["Purpose"].ToString(),
        Outcome = dataReader["Outcome"].ToString(),
        TimeZone = dataReader["TimeZone"].ToString()
      };
    }
  }
}