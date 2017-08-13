import { MeetingAttachment } from './meetingAttachment';
import { MeetingReacurance } from './meetingReacurance';
import { MeetingAttendee } from './meetingattendee';
import { MeetingAgenda } from './meetingAgenda';
import { MeetingAction } from './meetingAction';
import { MeetingNote } from './meetingNote';

export class MeetingModel {

    public IsLocked: boolean;
    public Id: string;
    public MeetingOwnerId: string;
    public Name: string;
    public Location: string;
    public Date: Date;
    public UpdatedDate: Date;
    public Time: string;
    public Duration: number;
    public IsReacurance: boolean;
    public ReacuranceType: string;
    public IsPrivate: boolean;
    public IsFormal: boolean;
    public TimeZone: string;
    public TimeZoneOffSet: number;
    public Tag: Array<string>;
    public MeetingAttendeeCollection: Array<MeetingAttendee>;
    public MeetingAgendaCollection: Array<MeetingAgenda>;
    public MeetingActionCollection: Array<MeetingAction>;
    public MeetingAttachmentCollection: Array<MeetingAttachment>;
    public MeetingNoteCollection: Array<MeetingNote>;
    public Purpose: string;
    public Outcome: string;
    public Reacurance: MeetingReacurance;
    constructor() {
    }
}