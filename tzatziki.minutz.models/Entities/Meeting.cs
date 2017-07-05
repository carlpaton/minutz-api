using System;
using System.Collections.Generic;

namespace tzatziki.minutz.models.Entities
{
  public class Meeting
  {
    /// <summary>
    /// IsLocked this is tell the UI if the meeting is readonly for running the meeting
    /// </summary>
    public bool IsLocked { get; set; }
    public Guid Id { get; set; }
    public string MeetingOwnerId { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string Time { get; set; }
    public int Duration { get; set; }
    public bool IsReacurance { get; set; }
    public string ReacuranceType { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsFormal { get; set; }
    public string TimeZone { get; set; }
		public int TimeZoneOffSet { get; set; }
		public string[] Tag { get; set; }
    public List<MeetingAttendee> MeetingAttendeeCollection { get; set; }
    public string Purpose { get; set; }
    public string Outcome { get; set; }
    public List<MeetingAgendaItem> MeetingAgendaCollection { get; set; }
    public List<ActionItem> MeetingActionCollection { get; set; }
    public List<MeetingNoteItem> MeetingNoteCollection { get; set; }
    public List<MeetingAttachmentItem> MeetingAttachmentCollection { get; set; }
		public MeetingReacurance Reacurance { get; set; }
	}
}