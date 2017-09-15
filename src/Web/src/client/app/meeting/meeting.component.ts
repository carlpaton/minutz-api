import {
    Component,
    OnInit
} from '@angular/core';
import {
    ActivatedRoute
} from '@angular/router';
import {
    MeetingModel
} from "../shared/models/meetingModel";
import {
    MeetingAttendee
} from '../shared/models/meetingAttendee';
declare let $: any;
@Component({
    moduleId: module.id,
    selector: 'sd-meeting',
    templateUrl: 'meeting.component.html',
    styleUrls: ['meeting.component.css']
})
export class MeetingComponent implements OnInit {
    Name = 'Meeting';
    Id: string = '';
    Check: boolean;
    TestDate: string;
    MeetingObject: MeetingModel;
    public ngOnInit() {
        this.route.params.subscribe(params => {
            this.Id = params['id'];
        });
        //todo: check if id then get meeting else create new one
        this.MeetingObject = new MeetingModel();
        this.MeetingObject.Date = new Date();
        this.MeetingObject.Name = 'Give your meeting a name';
        let att = new MeetingAttendee();
        att.Id = 'id';
        att.PersonIdentity = 'identity';
        att.ReferanceId = 'refid';
        att.Role = 'Attendee';
        att.Name = 'Mauro';
        att.Email = 'lee@leeroya.com';
        this.MeetingObject.MeetingAttendeeCollection = [];
        this.MeetingObject.MeetingAttendeeCollection.push(att);

        let tz = new Date().getTimezoneOffset();
        console.log(tz);
    }
    public constructor(
        private route: ActivatedRoute
    ) {
        this.Check = true;
        this.TestDate = '08/02/2016';
    }
    public saveModel(): void {
        console.log('save');
    }
    public TestClick($evnet: any) {
        this.TestDate = '08/02/2015';
        //if(this.Check){ this.Check = false;}else{this.Check = true;} 
    }
    public CheckChangedEvent($event: any) {
        console.log($event);
    }
    public SerachByOptionChange($event: any): void {
        console.log($event);
    };
    public testEvent($event: any): void {
        console.log($event);
    }
    public save(): void {
        console.log('save');
    }
    public create() {
        console.log('create');
    }
    public discover() {
        console.log('discovery');
    }
    public enableSearch() {
        console.log('discovery');
    }
    public HeadingChange($event: any) {
        this.MeetingObject.Name = $event;
    }
    search(): void {
        console.log('discovery');
    }
}
