import {
    FormsModule
} from '@angular/forms';
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
import {
    MeetingModel
} from "../../shared/models/meetingModel";
declare let $: any;
@Component({
    moduleId: module.id,
    selector: 'sd-heading',
    templateUrl: 'heading.component.html',
    styleUrls: ['heading.component.css']
})
export class HeadingComponent implements OnInit, AfterViewInit {
    @Input() Id: string;
    @Output() Change = new EventEmitter<string>();
    @Input() Meeting : MeetingModel;
    MeetingHeading: string;
    public ngOnChanges(changes: { [propKey: string]: SimpleChange }) {

    }
    public ngOnInit() {
        if (!this.Id) {
            this.Id = this.createId();
        }
    }
    public ngAfterViewInit() {
        this.MeetingHeading = this.Meeting.Name;
    }
    public textChange($event: any):void {
        this.Meeting.Name = $event;
        this.Change.emit($event);
    }
    private createId(): any {
        return `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}` +
            `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}`;
    }
    private createidsection() {
        return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    }
}
