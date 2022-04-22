import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AlertsService } from 'src/app/core/services/alerts.service';
import { AuthService } from './auth.service';

@Injectable()
export class AuthGuardService implements CanActivate {

  constructor(private authService: AuthService, 
    private router: Router,
    private alertsService: AlertsService) {}

  canActivate(): boolean {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/auth/login']);
      this.alertsService.showError('You must be logged in to view this page.');
      return false;
    }

    return true;
  }
}