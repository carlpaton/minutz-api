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
declare let $: any;
@Component({
    moduleId: module.id,
    selector: 'sd-datepicker',
    templateUrl: 'datepicker.component.html',
    styleUrls: ['datepicker.component.css']
})
export class DatePickerComponent implements OnInit, AfterViewInit {
    @Input() Id: string;
    @Input() Date: string;
    @Output() SelectedDateChange = new EventEmitter<string>();
    public ngOnChanges(changes: { [propKey: string]: SimpleChange }) {
        $('#' + this.Id).datepicker('update',this.Date);
    }
    public ngOnInit() {
        if (!this.Id) {
            this.Id = this.createId();
        }
        if (!this.Date) {
            this.Date = this.today();
        }
    }
    public ngAfterViewInit() {
        $('#' + this.Id).datepicker().on('change', (e:any) => {
            this.SelectedDateChange.emit($(e.target).val());
        });
        $('#' + this.Id).datepicker('update',this.Date);
    }
    private createId(): any {
        return `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}` +
               `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}`;
    }
    private createidsection() {
        return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    }
    private today() {
        return new Date(2011, 2, 5).toDateString();
    }
}
