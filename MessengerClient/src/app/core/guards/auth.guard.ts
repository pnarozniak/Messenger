import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AlertsService } from 'src/app/core/services/alerts.service';
import { AuthTokensService } from '../services/auth-tokens.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authTokensService: AuthTokensService,
    private alertsService: AlertsService,
    private router: Router) {}

  canActivate(): boolean {
    const {accessToken, refreshToken} = this.authTokensService.getTokens();
    if (accessToken && refreshToken)
      return true;

    this.alertsService.showError('You must sing in to view this page.');
    this.authTokensService.removeTokens();
    this.router.navigate(['/auth/login']);
    return false;
  }
}