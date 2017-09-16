import {
    Component,
    OnInit,
    Input,
    EventEmitter,
    Output,
    OnChanges,
    AfterViewInit
} from '@angular/core';
declare let $: any;
@Component({
    moduleId: module.id,
    selector: 'sd-duration',
    templateUrl: 'durationPicker.component.html',
    styleUrls: ['durationPicker.component.css']
})
export class DurationPickerComponent implements AfterViewInit, OnInit {
    Name: string;
    SecondsName: string;
    @Input() On: string;
    @Input() Duration: number;
    @Output() Seconds = new EventEmitter<any>();
    @Output() DurationEvent = new EventEmitter<any>();
    public ngAfterViewInit() {
        if(!this.Duration) {
            this.Duration = 100;
        }        
        $('#' + this.Name).timeDurationPicker({
            defaultValue: () => {
                return this.Duration;
            },
            years: false,
            months: false,
            days: false,
            seconds: false,
            onSelect: (element: any, seconds: any, duration: any) => {
                $('#' + this.SecondsName).val(seconds);
                $('#' + this.Name).val(duration);
                this.Seconds.emit(seconds);
                let result = { Duration : seconds, Label : duration };
                this.DurationEvent.emit(result);
            }
        });
    }
    public ngOnInit() {
        this.Name = 'duration-' + this.createId();
        this.SecondsName = 'seconds-' + this.createId();
    }
    private createId(): any {
        return `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}` +
            `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}`;
    }
    private createidsection() {
        return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    }
}
