using System.Collections.Generic;
using Minutz.Models.Entities;
using System;
using Minutz.Models.ViewModels;

namespace Interface.Repositories
{
  public interface IMeetingRepository
  {
    Meeting Get
      (Guid id, string schema, string connectionString);
    
    List<Meeting> List
      (string schema, string connectionString);
    
    bool Add
      (Meeting meeting, string schema, string connectionString);
    
    bool Update
      (Meeting meeting, string schema, string connectionString);
    
    bool Delete
      (Guid id, string schema, string connectionString);
    
    List<Meeting> List
      (string schema, string connectionString, List<string> meetingIds);

    (bool condition, string message) CreateUpdateMeetingMinutes
      (string schema, string connectionString, string meetingId, byte[] fileData);

    (bool condition, string message, MinutesViewModel minutes) GetMeetingMinutes
      (string schema, string connectionString, string meetingId);
  }
}
