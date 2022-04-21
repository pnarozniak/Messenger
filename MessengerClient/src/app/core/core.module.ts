import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { ApiService } from "./services/api.service";
import { ToastrModule } from "ngx-toastr";
import { AlertsService } from "./services/alerts.service";

@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        ToastrModule.forRoot()
    ],
    providers: [
        ApiService,
        AlertsService
    ],
    exports: [
        HttpClientModule
    ]
})
export class CoreModule { }