import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertsService } from 'src/app/core/services/alerts.service';
import { AuthApiService } from '../../../auth-api.service';
import { AuthTokensService } from '../../../auth-tokens.service';
import { LoginRequestModel } from '../../../auth.model';
import { EmailVerificationStateService } from '../../email-verification/email-verification-state.service';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss']
})
export class LoginFormComponent implements OnInit {
  fg = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
  })

  constructor(private authApiService: AuthApiService,
    private authTokensService: AuthTokensService,
    private emailVerificationStateService: EmailVerificationStateService,
    private router: Router,
    private alertsService: AlertsService) { }

  ngOnInit(): void { }

  get email() { return this.fg.get('email'); }
  get password() { return this.fg.get('password'); }

  handleSubmit() {
    this.setErrorsOnInputs(null);

    if (this.fg.invalid)
      return;
    
    this.login();
  }

  login() {
    const request: LoginRequestModel = {
      email: this.email!.value,
      plainPassword: this.password!.value
    }

    this.authApiService.login(request).subscribe({
        next: ({accessToken, refreshToken}) => {
          this.authTokensService.saveAccessToken(accessToken);
          this.authTokensService.saveRefreshToken(refreshToken);
          this.router.navigate(['/']);
          this.alertsService.showSuccess('You have successfully logged in!');
        },
        error: ({status}) => {
          if (status == 401) {
            const err = {"401": true};
            this.fg.setErrors(err);
            this.setErrorsOnInputs(err)
          } else if (status == 403) {
            this.emailVerificationStateService.email = request.email
            this.router.navigate(['/auth/email-verification']);
          }
        }
      });
  }

  setErrorsOnInputs(errors: any) {
    this.email?.setErrors(errors);
    this.password?.setErrors(errors);
  }
}
