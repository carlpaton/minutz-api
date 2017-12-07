using System;
using System.Collections.Generic;

namespace Api.Extensions
{
  public static class MeetingExtensions
  {
    public static Minutz.Models.Entities.Meeting ToEntity(this Minutz.Models.ViewModels.MeetingViewModel viewModel)
    {
      var result = new Minutz.Models.Entities.Meeting
      {
        Id = Guid.Parse(viewModel.Id),
        Name = viewModel.Name,
        Date = viewModel.Date,
        Duration = viewModel.Duration,
        IsFormal = viewModel.IsFormal,
        IsLocked = viewModel.IsLocked,
        IsPrivate = viewModel.IsPrivate,
        IsReacurance = viewModel.IsReacurance,
        MeetingOwnerId = viewModel.MeetingOwnerId,
        Outcome = viewModel.Outcome,
        Purpose = viewModel.Purpose,
        ReacuranceType = viewModel.ReacuranceType,
        Tag = viewModel.Tag,
        Time = viewModel.Time,
        TimeZone = viewModel.TimeZone,
        UpdatedDate = DateTime.UtcNow
      };
      return result;
    }

    public static Minutz.Models.ViewModels.MeetingViewModel ToViewModel(this Minutz.Models.Entities.Meeting entity)
    {
      var result = new Minutz.Models.ViewModels.MeetingViewModel
      {
        Id = entity.Id.ToString(),
        Name = entity.Name,
        Date = entity.Date,
        Duration = entity.Duration,
        IsFormal = entity.IsFormal,
        IsLocked = entity.IsLocked,
        IsPrivate = entity.IsPrivate,
        IsReacurance = entity.IsReacurance,
        MeetingOwnerId = entity.MeetingOwnerId,
        Outcome = entity.Outcome,
        Purpose = entity.Purpose,
        ReacuranceType = entity.ReacuranceType,
        Tag = entity.Tag,
        Time = entity.Time,
        TimeZone = entity.TimeZone,
        UpdatedDate = DateTime.UtcNow
      };
      _defaultValues(result);
      return result;
    }

    public static Minutz.Models.ViewModels.MeetingViewModel ToViewModel(this Minutz.Models.Entities.Meeting entity,
                                                                        List<Minutz.Models.Entities.MeetingAgenda> agendaItems,
                                                                        List<Minutz.Models.Entities.MeetingAttendee> attendees = null,
                                                                        List<Minutz.Models.Entities.MeetingNote> notes = null,
                                                                        List<Minutz.Models.Entities.MeetingAttachment> attachments = null)
    {
      var result = new Minutz.Models.ViewModels.MeetingViewModel
      {
        Id = entity.Id.ToString(),
        Name = entity.Name,
        Date = entity.Date,
        Duration = entity.Duration,
        IsFormal = entity.IsFormal,
        IsLocked = entity.IsLocked,
        IsPrivate = entity.IsPrivate,
        IsReacurance = entity.IsReacurance,
        MeetingOwnerId = entity.MeetingOwnerId,
        Outcome = entity.Outcome,
        Purpose = entity.Purpose,
        ReacuranceType = entity.ReacuranceType,
        Tag = entity.Tag,
        Time = entity.Time,
        TimeZone = entity.TimeZone,
        UpdatedDate = DateTime.UtcNow
      };
      _defaultValues(result, agendaItems, attendees, notes, attachments);
      return result;
    }

    internal static void _defaultValues(Minutz.Models.ViewModels.MeetingViewModel meetingViewModel,
                                        List<Minutz.Models.Entities.MeetingAgenda> agendaItems = null,
                                        List<Minutz.Models.Entities.MeetingAttendee> attendees = null,
                                        List<Minutz.Models.Entities.MeetingNote> notes = null,
                                        List<Minutz.Models.Entities.MeetingAttachment> attachments = null)
    {
      if (Guid.Parse(meetingViewModel.Id) == Guid.Empty)
      {
        meetingViewModel.Id = Guid.NewGuid().ToString();
      }
      if (meetingViewModel.MeetingAgendaCollection == null)
        meetingViewModel.MeetingAgendaCollection = new List<Minutz.Models.Entities.MeetingAgenda>();

      if (meetingViewModel.MeetingAttendeeCollection == null)
        meetingViewModel.MeetingAttendeeCollection = new List<Minutz.Models.Entities.MeetingAttendee>();

      if (meetingViewModel.AvailableAttendeeCollection == null)
        meetingViewModel.AvailableAttendeeCollection = new List<Minutz.Models.Entities.MeetingAttendee>();

      if (meetingViewModel.MeetingNoteCollection == null)
        meetingViewModel.MeetingNoteCollection = new List<Minutz.Models.Entities.MeetingNote>();

      if (meetingViewModel.MeetingAttachmentCollection == null)
        meetingViewModel.MeetingAttachmentCollection = new List<Minutz.Models.Entities.MeetingAttachment>();
    }
  }
}
