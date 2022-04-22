import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { ApiService } from "./services/api.service";
import { ToastrModule } from "ngx-toastr";
import { AlertsService } from "./services/alerts.service";
import { JwtModule } from "@auth0/angular-jwt";
import { AuthTokensService } from "../feature/auth/auth-tokens.service";

@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        ToastrModule.forRoot(),
        JwtModule.forRoot({
            config: {
                tokenGetter: () => new AuthTokensService().getAccessToken(),
                allowedDomains: ["localhost:7009"],
                disallowedRoutes: ["https://localhost:7009/api/auth/"],
            },
        })
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