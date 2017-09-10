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
    selector: 'sd-checkbox',
    templateUrl: 'checkbox.component.html',
    styleUrls: ['checkbox.component.css']
})
export class CheckboxComponent implements OnInit, AfterViewInit {
    @Input() Id: string;
    @Input() Checked: boolean;
    @Output() CheckChange = new EventEmitter<boolean>();
    ngOnInit() {
        if (!this.Id) {
            this.Id = this.createId();
        }
    }
    ngOnChanges(changes: { [propKey: string]: SimpleChange }) {
        if(this.Checked){
            $('#' + this.Id).iCheck('check');
        }else{
            $('#' + this.Id).iCheck('uncheck');
        }
    }
    public emitfunction (): void {
        let checked: boolean = $('#' + this.Id).is(":checked");
        this.CheckChange.emit(checked);
    }
    public ngAfterViewInit() {
        $('#' + this.Id).iCheck({ checkboxClass: 'icheckbox_minimal-blue' }).on('ifChanged', () => this.emitfunction());
        if(this.Checked){
            $('#' + this.Id).iCheck('check');
        }
    }
    constructor() {
    }
    private createId(): any {
        return this.createidsection() + '-' + this.createidsection() + '-' + this.createidsection() + '-' + this.createidsection() + '-'
            + this.createidsection() + '-' + this.createidsection() + '-' + this.createidsection();
    }
    private createidsection() {
        return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    }
}