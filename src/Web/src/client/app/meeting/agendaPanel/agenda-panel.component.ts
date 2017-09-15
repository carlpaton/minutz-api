import {
    Component,
    OnInit,
    Input,
    EventEmitter,
    Output
} from '@angular/core';
import {
    MeetingModel
} from "../../shared/models/meetingModel";
declare let $: any;
@Component({
    moduleId: module.id,
    selector: 'sd-agenda-panel',
    templateUrl: 'agenda-panel.component.html',
    styleUrls: ['agenda-panel.component.css']
})
export class AgendaPanelComponent implements OnInit {
    Name:string;
    @Input() Id: string;
    @Input() Meeting : MeetingModel;
    @Output() Click = new EventEmitter();
    private createId(): any {
        return `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}` +
               `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}`;
    }
    private createidsection() {
        return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    }
    public ngOnInit() {
        if (!this.Id) {
            this.Name = this.createId();
        }else {
            this.Name = this.Id;
        }
    }
    public click():void {
        this.Click.emit();
    }
}
