﻿using System.Collections.Generic;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.interfaces
{
  public interface IMeetingService
  {
    Meeting Get(string schema, Meeting meeting, string callingUserId ,bool read = false);
    IEnumerable<Meeting> Get(string schema, UserProfile user);
		void DeleteAgenda(string schema, string agendaItemId);
		MeetingAttachmentItem SaveFile(string schema, UserProfile user, string fileName, byte[] data, string meetingId);
	}
}
