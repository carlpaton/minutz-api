using System;
using System.Collections.Generic;

namespace Minutz.Models.ViewModels
{
  public class MeetingViewModel
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string Time { get; set; }
    public int Duration { get; set; }
    public bool IsReacurance { get; set; }
    public bool IsPrivate { get; set; }
    public int ReacuranceType { get; set; }
    public bool IsLocked { get; set; }
    public bool IsFormal { get; set; }
    public string Status { get; set; }
    public string Location {get;set;}
    public string TimeZone { get; set; }
    public int TimeZoneOffSet{ get; set;}
    public List<string> Tag { get; set; }
    public string Purpose { get; set; }
    public string MeetingOwnerId { get; set; }
    public string Outcome { get; set; }
    

    public List<Entities.MeetingAgenda> MeetingAgendaCollection { get; set; }
    public List<Entities.MeetingAttachment> MeetingAttachmentCollection { get; set; }
    public List<Entities.MeetingAttendee> MeetingAttendeeCollection { get; set; }
    public List<Entities.MeetingAttendee> AvailableAttendeeCollection { get; set; }
    public List<Entities.MeetingNote> MeetingNoteCollection { get; set; }
    public List<Entities.MinutzAction> MeetingActionCollection { get; set; }
    public List<Entities.MinutzDecision> MeetingDecisionCollection { get; set; }

    public string ResultMessage { get; set; }
  }
}
