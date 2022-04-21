import { Injectable } from "@angular/core";
import { ToastrService } from "ngx-toastr";

@Injectable()
export class AlertsService {    
    constructor(private toastr: ToastrService) { }
    
    showSuccess(message: string, title?: string) {
        this.toastr.success(message, title);
    }

    showError(message: string, title?: string) {
        this.toastr.error(message, title);
    }
}
    