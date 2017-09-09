import { FormsModule } from '@angular/forms';
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
//import { FileUploader } from "ng2-file-upload";
declare let $: any;
const URL = 'https://evening-anchorage-3159.herokuapp.com/api/';
@Component({
    moduleId: module.id,
    selector: 'sd-fileupload',
    templateUrl: 'fileupload.component.html',
    styleUrls: ['fileupload.component.css']
})
export class FileuploadComponent implements OnInit, AfterViewInit {
    // public uploader: FileUploader = new FileUploader({ url: URL });
    // public hasBaseDropZoneOver: boolean = false;
    // public hasAnotherDropZoneOver: boolean = false;
    @Input() Id: string;
    @Input() UploadUrl: string;
    @Input() Date: string;
    @Output() SelectedDateChange = new EventEmitter<string>();
    public ngOnChanges(changes: { [propKey: string]: SimpleChange }) {

    }
    public ngOnInit() {
        if (!this.Id) {
            this.Id = this.createId();
        }
        this.UploadUrl = '/foo';
        $("#upload").dropzone({ url: "/file/post" });
    }
    public ngAfterViewInit() {
        
        
        //$('.drop').dropzone({ url: "/file/post" });
    }
    public fileOverBase(e: any): void {
       // this.hasBaseDropZoneOver = e;
    }

    public fileOverAnother(e: any): void {
        //this.hasAnotherDropZoneOver = e;
    }
    private createId(): any {
        return `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}` +
            `${this.createidsection()}-${this.createidsection()}-${this.createidsection()}-${this.createidsection()}`;
    }
    private createidsection() {
        return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
    }
}
