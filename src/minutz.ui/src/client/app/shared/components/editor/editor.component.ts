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
    selector: 'sd-editor',
    templateUrl: 'editor.component.html',
    styleUrls: ['editor.component.css']
})
export class EditorComponent implements OnInit, AfterViewInit {
    Name:string;
    @Input() Id: string;
    @Input() Date: string;
    @Output() SelectedDateChange = new EventEmitter<string>();
    public ngOnChanges(changes: { [propKey: string]: SimpleChange }) {

    }
    public ngOnInit() {
        if (!this.Id) {
            this.Name = this.createId();
        }else {
            this.Name = this.Id;
        }
    }
    public ngAfterViewInit() {
        $('#' + this.Name).wysihtml5();
    }
    private createId(): any {
        return `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}` +
               `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}`;
    }
    private createidsection() {
        return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    }
}
