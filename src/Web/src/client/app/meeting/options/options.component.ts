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
    @Input() Meeting: MeetingModel;
    @Input() Id: string;
    @Output() Change = new EventEmitter<MeetingModel>();
    public ngOnChanges(changes: { [propKey: string]: SimpleChange }) {

    }
    public ngOnInit() {
        if (!this.Id) {
            this.Id = this.createId();
        }
    }
    public ngAfterViewInit() {
        console.log(this.Meeting);
    }
    public locationChange($event: any) {
        this.Meeting.Location = $event;
        this.Change.emit(this.Meeting);
    }
    public dateChange($event: any) {
        this.Meeting.Date = $event;
        this.Change.emit(this.Meeting);
        console.log('Date Change');
    }
    public timeChange($event:any) {
        this.Meeting.TimeZone = $event;
        this.Change.emit(this.Meeting);
        console.log($event);
    }
    public meetingDurationChange($event:any) {
        this.Meeting.Duration = $event.Duration;
    }
    private createId(): any {
        return `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}` +
            `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}`;
    }
    private createidsection() {
        return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    }
}
