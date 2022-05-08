import { Injectable } from "@angular/core";
import { Observable, tap } from "rxjs";
import { ApiService } from "src/app/core/services/api.service";
import { LoginRequest } from "../models/login-request.model";
import { TokensResponse } from "../models/tokens-response.model";
import { RegisterRequest } from "../models/register-request.model";
import { EmailVerificationRequest } from "../models/email-verification-request.model";
import { AlertsService } from "src/app/core/services/alerts.service";
import { Router } from "@angular/router";
import { AuthTokensService } from "src/app/core/services/auth-tokens.service";

@Injectable()
export class AuthApiService {
    constructor(
        private apiService: ApiService,
        private alertsService: AlertsService,
        private authTokensService: AuthTokensService,
        private router: Router) {}

    login(data: LoginRequest) : Observable<TokensResponse>{
        return this.apiService.post<LoginRequest, TokensResponse>("auth/login", data)
            .pipe(
                tap(tokens => {
                    this.authTokensService.saveTokens(tokens.accessToken, tokens.refreshToken);
                    this.alertsService.showSuccess('You have successfully logged in!');
                    this.router.navigate(['/']);
                }),
            );
    }

    register(data: RegisterRequest) : Observable<any>{
        return this.apiService.post<RegisterRequest, any>("auth/register", data)
            .pipe(
                tap(() => {
                    this.alertsService.showSuccess('You have successfully registered!');
                    this.router.navigate(['/auth/login']);
                }),
            );
    }

    verifyEmail(data: EmailVerificationRequest) : Observable<any>{
        return this.apiService.post<EmailVerificationRequest, any>("auth/verify-email", data)
            .pipe(
                tap(() => {
                    this.alertsService.showSuccess('Email verified successfully. You can log in now.');
                    this.router.navigate(['/auth/login']);
                }),
            );
    }
}