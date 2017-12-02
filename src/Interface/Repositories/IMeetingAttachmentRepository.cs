using System;
using System.Collections.Generic;
using Models.Entities;

namespace Interface.Repositories
{
  public interface IMeetingAttachmentRepository
  {
    MeetingAttachment Get(Guid id, string schema, string connectionString);
    List<MeetingAttachment> GetMeetingAttachments(Guid referenceId, string schema, string connectionString);
    IEnumerable<MeetingAttachment> List(string schema, string connectionString);
    bool Add(MeetingAttachment action, string schema, string connectionString);
    bool Update(MeetingAttachment action, string schema, string connectionString);
    bool DeleteMeetingAcchments(Guid referenceId, string schema, string connectionString);
    bool Delete(Guid attachmentId, string schema, string connectionString);
  }
}
