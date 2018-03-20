﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Extensions
{
  public static class MeetingExtensions
  {
    public static Minutz.Models.ViewModels.MeetingViewModel ToMeetingViewModel(this Models.MeetingItemViewModel model)
    {
      var meetingId = model.Id == null ? Guid.NewGuid() : Guid.Parse(model.Id);
      if (string.IsNullOrEmpty(model.Tag))
      {
        model.Tag = string.Empty;
      }
      if (string.IsNullOrEmpty(model.Purpose))
      {
        model.Purpose = string.Empty;
      }
      if (string.IsNullOrEmpty(model.Outcome))
      {
        model.Outcome = string.Empty;
      }
      if (string.IsNullOrEmpty(model.ReacuranceType))
      {
        model.ReacuranceType = string.Empty;
      }
      if (model.Date == DateTime.MinValue || model.Date == null)
      {
        model.Date = DateTime.UtcNow;
      }
      if (model.UpdatedDate == DateTime.MinValue || model.UpdatedDate == null)
      {
        model.UpdatedDate = DateTime.UtcNow;
      }
      if (string.IsNullOrEmpty(model.TimeZone))
      {
        model.TimeZone = string.Empty;
      }
      var result = new Minutz.Models.ViewModels.MeetingViewModel
      {
        Date = model.Date,
        Duration = model.Duration,
        Id = meetingId.ToString(),
        IsFormal = model.IsFormal,
        IsLocked = model.IsLocked,
        IsPrivate = model.IsPrivate,
        IsReacurance = model.IsReacurance,
        MeetingOwnerId = model.MeetingOwnerId,
        Name = model.Name,
        Outcome = model.Outcome,
        Purpose = model.Purpose,
        Status = model.Status,
        ReacuranceType = int.Parse( model.ReacuranceType),
        //Tag =  model.Tag,
        Time = model.Time,
        TimeZone = model.TimeZone,
        UpdatedDate = model.UpdatedDate,
        AvailableAttendeeCollection = new List<Minutz.Models.Entities.MeetingAttendee>(),
        MeetingActionCollection = new List<Minutz.Models.Entities.MinutzAction>(),
        MeetingAgendaCollection = new List<Minutz.Models.Entities.MeetingAgenda>(),
        MeetingAttachmentCollection = new List<Minutz.Models.Entities.MeetingAttachment>(),
        MeetingAttendeeCollection = new List<Minutz.Models.Entities.MeetingAttendee>(),
        MeetingNoteCollection = new List<Minutz.Models.Entities.MeetingNote>()
      };
      return result;
    }

    public static Minutz.Models.Entities.Meeting ToEntity(this Minutz.Models.ViewModels.MeetingViewModel viewModel)
    {
      var meetingId = viewModel.Id == null ? Guid.NewGuid() : Guid.Parse(viewModel.Id);
      if (viewModel.Tag == null)
      {
        viewModel.Tag = new List<string>();
      }
      if (viewModel.Date == DateTime.MinValue)
      {
        viewModel.Date = DateTime.UtcNow;
      }
      if (viewModel.UpdatedDate == DateTime.MinValue)
      {
        viewModel.UpdatedDate = DateTime.UtcNow;
      }
      if (string.IsNullOrEmpty(viewModel.Purpose))
      {
        viewModel.Purpose = string.Empty;
      }
      if (string.IsNullOrEmpty(viewModel.Outcome))
      {
        viewModel.Outcome = string.Empty;
      }
//      if (string.IsNullOrEmpty(viewModel.ReacuranceType))
//      {
//        viewModel.ReacuranceType = string.Empty;
//      }
      if (string.IsNullOrEmpty(viewModel.TimeZone))
      {
        viewModel.TimeZone = string.Empty;
      }
      if (viewModel.AvailableAttendeeCollection == null)
      {
        viewModel.AvailableAttendeeCollection = new List<Minutz.Models.Entities.MeetingAttendee>();
      }
      if (viewModel.MeetingAgendaCollection == null)
      {
        viewModel.MeetingAgendaCollection = new List<Minutz.Models.Entities.MeetingAgenda>();
      }
      if (viewModel.MeetingAttendeeCollection == null)
      {
        viewModel.MeetingAttendeeCollection = new List<Minutz.Models.Entities.MeetingAttendee>();
      }
      if (viewModel.MeetingAttachmentCollection == null)
      {
        viewModel.MeetingAttachmentCollection = new List<Minutz.Models.Entities.MeetingAttachment>();
      }
      if (viewModel.MeetingActionCollection == null)
      {
        viewModel.MeetingActionCollection = new List<Minutz.Models.Entities.MinutzAction>();
      }
      if (viewModel.MeetingNoteCollection == null)
      {
        viewModel.MeetingNoteCollection = new List<Minutz.Models.Entities.MeetingNote>();
      }
      var result = new Minutz.Models.Entities.Meeting
      {
        Id = meetingId,
        Name = viewModel.Name,
        Date = viewModel.Date,
        Duration = viewModel.Duration,
        IsFormal = viewModel.IsFormal,
        IsLocked = viewModel.IsLocked,
        IsPrivate = viewModel.IsPrivate,
        Status = viewModel.Status,
        IsReacurance = viewModel.IsReacurance,
        MeetingOwnerId = viewModel.MeetingOwnerId,
        Outcome = viewModel.Outcome,
        Purpose = viewModel.Purpose,
        ReacuranceType = viewModel.ReacuranceType.ToString(),
        Tag = String.Join(",", viewModel.Tag.ToArray()),
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
        Status = entity.Status,
        Outcome = entity.Outcome,
        Purpose = entity.Purpose,
        ReacuranceType = int.Parse( entity.ReacuranceType),
        Tag = entity.Tag.Split(',').ToList(),
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
        Status = entity.Status,
        ReacuranceType = int.Parse( entity.ReacuranceType),
        Tag = entity.Tag.Split(',').ToList(),
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
