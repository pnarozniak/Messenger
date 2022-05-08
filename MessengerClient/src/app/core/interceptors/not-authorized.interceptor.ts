import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { BehaviorSubject, catchError, filter, Observable, switchMap, take, tap, throwError } from "rxjs";
import { AuthTokensService } from "src/app/core/services/auth-tokens.service";
import { TokensResponse } from "src/app/feature/auth/models/tokens-response.model";
import { AlertsService } from "../services/alerts.service";
import { ApiService } from "../services/api.service";

@Injectable()
export class NotAuthorizedInterceptor implements HttpInterceptor{
    private isRefreshing = false;
    private tokensSubject = new BehaviorSubject<TokensResponse | null>(null);

    constructor(
        private authTokensService: AuthTokensService, 
        private alertsService: AlertsService,
        private router: Router,
        private apiService: ApiService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const clonedRequest = req;
        return next.handle(req).pipe(catchError(error => {
            if (error instanceof HttpErrorResponse && error.status === 401) {
                if (!req.url.includes('api/auth/'))
                    return this.handle401(clonedRequest, next);
            }
            return throwError(()=>error)
        }));
    }

    private handle401(req: HttpRequest<any>, next: HttpHandler) {
        const { refreshToken } = this.authTokensService.getTokens();

        if (this.isRefreshing || !refreshToken) {
            return this.tokensSubject.pipe(
                filter(tokens => tokens != null),
                take(1),
                switchMap((tokens) => next.handle(
                    req.clone({ headers: req.headers.set("Authorization", `Bearer ${tokens!.accessToken}`) })
                )));
        }

        return this.refresh().pipe(
            switchMap((tokens) => {
                this.isRefreshing = false;
                this.tokensSubject.next(tokens);
                return next.handle(
                    req.clone({ headers: req.headers.set("Authorization", `Bearer ${tokens.accessToken}`) })
                );
            }),
            catchError((error) => {
                this.isRefreshing = false;     
                this.sessionExpired();
                return throwError(() => error);
            })
        );
    }

    private refresh() : Observable<TokensResponse> {
        this.isRefreshing = true;
        const {accessToken, refreshToken} = this.authTokensService.getTokens();

        if (!accessToken || !refreshToken) {
            return throwError(() => 'No tokens to refresh');
        }
        
        return this.apiService.post<any, TokensResponse>('auth/refresh-token', {accessToken, refreshToken})
            .pipe(
                tap((tokens) => {
                    this.authTokensService.saveTokens(tokens.accessToken, tokens.refreshToken);
                }),
                catchError((err) => {
                    return throwError(() => err);
                })
            );       
    }

    private sessionExpired() {
        this.authTokensService.removeTokens();
        this.alertsService.showError("Your session has expired. Please sign in again.");
        this.router.navigate(['/auth/login']);
    }
}