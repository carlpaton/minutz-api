using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Feature.Meeting
{
    public class MeetingUpdateRequest
    {
        [Required]
        public Guid id {get;set;}
        public string name {get;set;}
        public bool isLocked {get;set;}
        public string meetingOwnerId{get;set;}
        public string location {get;set;}
        public DateTime date {get;set;}
        public DateTime updatedDate {get;set;}
        public string time {get;set;}
        public int duration {get;set;}
        public bool isRecurrence {get;set;}
        public string status {get;set;}
        public int recurrenceType {get;set;}
        public bool isPrivate {get;set;}
        public bool isFormal {get;set;}
        public string timeZone {get;set;}
        public int timeZoneOffSet {get;set;}
        public IEnumerable<string> tag {get;set;}
        public string purpose {get;set;}
        public string outcome {get;set;}
    }
}
