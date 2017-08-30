import {
    forEach
} from '@angular/router/src/utils/collection';
import { 
    Component,
    ElementRef,
    OnInit,
    Input,
    EventEmitter,
    Output,
    OnChanges,
    AfterViewInit
} from '@angular/core';
import { FormControl } from '@angular/forms';
declare let $: any;
@Component({
    moduleId: module.id,
    selector: 'select2',
    templateUrl: `select2.component.html`,
})
export class Select2Component implements AfterViewInit {
    @Input() Name: string = '';
    @Input() SelectedItems: Array<string> = [];
    @Input() Data: Array<string> = [];
    @Input() IsMultiple: boolean = false;
    @Input() Control: FormControl;
    @Output() SelectedItemsChange = new EventEmitter<any[]>();
    public constructor(private el: ElementRef) {
        console.log('select2 construct');
    }
    public ngAfterViewInit() {
        $('#' + this.Name).select2();
        $('#' + this.Name).on(
            'change', (e:any) => {
                this.SelectedItems = $(e.target).val();
                this.SelectedItemsChange.emit($(e.target).val());
            }
        );
        if(this.SelectedItems.length > 0) {
            $('#' + this.Name).val(this.SelectedItems).trigger('change');
        }
    }
}
