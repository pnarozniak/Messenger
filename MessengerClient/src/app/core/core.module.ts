import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { ToastrModule } from "ngx-toastr";
import { JwtHelperService, JwtModule } from "@auth0/angular-jwt";
import { NotAuthorizedInterceptor } from "./interceptors/not-authorized.interceptor";
import { AuthTokensService } from "./services/auth-tokens.service";

@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        ToastrModule.forRoot(),
        JwtModule.forRoot({
            config: {
                tokenGetter: () => new AuthTokensService().getTokens().accessToken,
                allowedDomains: ["localhost:7009"],
                disallowedRoutes: ["https://localhost:7009/api/auth/"],
            },
        })
    ],
    providers: [
        JwtHelperService,
        { provide: HTTP_INTERCEPTORS, useClass: NotAuthorizedInterceptor, multi: true }
    ],
    exports: [
        HttpClientModule
    ]
})
export class CoreModule { }