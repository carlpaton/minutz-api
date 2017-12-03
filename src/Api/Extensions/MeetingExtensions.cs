using System;
using System.Collections.Generic;

namespace Api.Extensions
{
  public static class MeetingExtensions
  {
    public static Models.Entities.Meeting ToEntity(this Models.ViewModels.MeetingViewModel viewModel)
    {
      var result = new Models.Entities.Meeting
      {
        Id = viewModel.Id,
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

    public static Models.ViewModels.MeetingViewModel ToViewModel(this Models.Entities.Meeting entity)
    {
      var result = new Models.ViewModels.MeetingViewModel
      {
        Id = entity.Id,
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

    public static Models.ViewModels.MeetingViewModel ToViewModel(this Models.Entities.Meeting entity,
                                                                      List<Models.Entities.MeetingAgenda> agendaItems,
                                                                      List<Models.Entities.MeetingAttendee> attendees = null,
                                                                      List<Models.Entities.MeetingNote> notes = null,
                                                                      List<Models.Entities.MeetingAttachment> attachments = null)
    {
      var result = new Models.ViewModels.MeetingViewModel
      {
        Id = entity.Id,
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
      _defaultValues(result,agendaItems,attendees,notes,attachments);
      return result;
    }

    internal static void _defaultValues(Models.ViewModels.MeetingViewModel meetingViewModel,
                                        List<Models.Entities.MeetingAgenda> agendaItems = null,
                                        List<Models.Entities.MeetingAttendee> attendees = null,
                                        List<Models.Entities.MeetingNote> notes = null,
                                        List<Models.Entities.MeetingAttachment> attachments = null)
    {
      if (meetingViewModel.Id == Guid.Empty)
      {
        meetingViewModel.Id = Guid.NewGuid();
      }
      if (meetingViewModel.Agenda == null)
        meetingViewModel.Agenda = new List<Models.Entities.MeetingAgenda>();

      if (meetingViewModel.Attendees == null)
        meetingViewModel.Attendees = new List<Models.Entities.MeetingAttendee>();

      if (meetingViewModel.AvailibleAttendees == null)
        meetingViewModel.AvailibleAttendees = new List<Models.Entities.MeetingAttendee>();

      if (meetingViewModel.Notes == null)
        meetingViewModel.Notes = new List<Models.Entities.MeetingNote>();

      if (meetingViewModel.Attachments == null)
        meetingViewModel.Attachments = new List<Models.Entities.MeetingAttachment>();
    }
  }
}
