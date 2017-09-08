import {
    Component,
    OnInit,
    Input,
    EventEmitter,
    Output,
    OnChanges,
    AfterViewInit
} from '@angular/core';
import * as moment from 'moment';
declare let $: any;

@Component({
    moduleId: module.id,
    selector: 'sd-timezone',
    templateUrl: 'timezone.component.html',
    styleUrls: ['timezone.component.css']
})
export class TimeZoneComponent implements AfterViewInit {
    Id: string = '';
    @Input() Date: string;
    @Output() Change = new EventEmitter<any>();
    public ngAfterViewInit() {
        let current = moment.parseZone(new Date().getDate()).zone();
        console.log('-----');
        console.log(current);
        $('#' + this.Id).betterTimezone({
            showIANA: false,
            showOptgroups: false // cannot be true when showIANA is false 
        }).on('change', (e: any) => {
            this.Change.emit($(e.target).val());
        });
    }
    public SelectedDate(time: any) {
        this.Change.emit(time);
    }
    constructor() {
        this.Id = 'time-zone-' + this.createId();
    }
    private createId(): any {
        return `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}` +
            `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}`;
    }
    private createidsection() {
        return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    }
}
