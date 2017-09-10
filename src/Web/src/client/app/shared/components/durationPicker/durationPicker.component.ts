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
    @Input() Off: string;
    @Output() Seconds = new EventEmitter<any>();
    @Output() Duration = new EventEmitter<any>();
    public ngAfterViewInit() {
        $('#' + this.Name).timeDurationPicker({
            defaultValue: () => {
                $('#' + this.SecondsName).val();
            },
            years: false,
            months: false,
            days: false,
            seconds: false,
            onSelect: (element: any, seconds: any, duration: any) => {
                $('#' + this.SecondsName).val(seconds);
                $('#' + this.Name).val(duration);
                this.Seconds.emit(seconds);
                this.Duration.emit(duration);
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
