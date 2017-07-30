﻿using System.Collections.Generic;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.interfaces
{
  public interface IMeetingRepository
  {
    /// <summary>
    /// Get by Id and return meeting object, if does not exist create the instance
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="schema"></param>
    /// <param name="meeting"></param>
    /// <returns></returns>
    Meeting Get(string connectionString, string schema, Meeting meeting, string callingUserId ,bool read = false);

    IEnumerable<Meeting> Get(string connectionString, string schema, UserProfile user);

    void DeleteMeetingSchema(string connectionString, string schema, UserProfile user);

		void DeleteMeetingAgendaItem(string connectionString, string schema, string agendaItemId);

		MeetingAttachmentItem SaveFile(string connectionString, string schema, UserProfile user, string fileName, byte[] data, string meetingId);

		KeyValuePair<string, byte[]> GetFileData(string connectionString, string schema, string fileId);
	}
}