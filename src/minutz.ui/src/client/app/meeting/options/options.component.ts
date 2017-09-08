import { FormsModule } from '@angular/forms';
import {
    Component,
    OnInit,
    Input,
    EventEmitter,
    Output,
    AfterViewInit,
    OnChanges,
    SimpleChange
} from '@angular/core';
import { MeetingModel } from "../../shared/models/meetingModel";
declare let $: any;
@Component({
    moduleId: module.id,
    selector: 'sd-options',
    templateUrl: 'options.component.html',
    styleUrls: ['options.component.css']
})
export class OptionsComponent implements OnInit, AfterViewInit {
    MeetingKey: string = 'meeting';
    IsLoggedIn: boolean;
    Meeting: MeetingModel;
    @Input() Id: string;
    @Output() Change = new EventEmitter<MeetingModel>();
    public ngOnChanges(changes: { [propKey: string]: SimpleChange }) {

    }
    public ngOnInit() {
        if (!this.Id) {
            this.Id = this.createId();
        }
        this.Meeting = new MeetingModel();
    }
    public ngAfterViewInit() {
    }
    public locationChange($event: any) {
        //var session = JSON.parse(localStorage.getItem(this.MeetingKey));
        //session.Location = $event;
        //localStorage.setItem(this.MeetingKey, JSON.stringify(session));
        this.Meeting.Location = $event;
        this.Change.emit(this.Meeting);
    }
    public timeChange($event:any) {
        this.Meeting.TimeZone = $event;
        this.Change.emit(this.Meeting);
        console.log($event);
    }
    public save() {
        console.log(JSON.parse(localStorage.getItem(this.MeetingKey)));
      }
    private createId(): any {
        return `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}` +
            `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}`;
    }
    private createidsection() {
        return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    }
}
