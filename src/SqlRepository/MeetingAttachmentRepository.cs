using System;
using System.Collections.Generic;
using Interface.Repositories;
using Models.Entities;

namespace SqlRepository
{
  public class MeetingAttachmentRepository : IMeetingAttachmentRepository
  {
    public MeetingAttachment Get(Guid id, string schema, string connectionString)
    {
      throw new NotImplementedException();
    }
    public List<MeetingAttachment> GetMeetingAttachments(Guid referenceId, string schema, string connectionString)
    {
      throw new NotImplementedException();
    }
    public IEnumerable<MeetingAttachment> List(string schema, string connectionString)
    {
      throw new NotImplementedException();
    }
    public bool Add(MeetingAttachment action, string schema, string connectionString)
    {
      throw new NotImplementedException();
    }
    public bool Update(MeetingAttachment action, string schema, string connectionString)
    {
      throw new NotImplementedException();
    }
    public bool Delete(Guid attachmentId, string schema, string connectionString)
    {
      throw new NotImplementedException();
    }
  }
}
