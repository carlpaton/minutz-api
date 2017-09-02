import {
    Component,
    OnInit,
    Input,
    EventEmitter,
    Output
} from '@angular/core';
declare let $: any;
@Component({
    moduleId: module.id,
    selector: 'sd-attachment-panel',
    templateUrl: 'attachment-panel.component.html',
    styleUrls: ['attachment-panel.component.css']
})
export class AttachmentPanelComponent implements OnInit {
    Name:string;
    @Input() Id: string;
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
