import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatFormField } from '@angular/material/form-field';
import { Router } from '@angular/router';
import { LoginRequest } from '../../models/login-request.model';
import { AuthApiService } from '../../services/auth-api.service';
import { AuthStateService } from '../../services/auth-state.service';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss']
})
export class LoginFormComponent implements OnInit {
  @ViewChildren(MatFormField) formFields!: QueryList<MatFormField>;
  fg = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
  })

  constructor(
    private authApiService: AuthApiService,
    private authStateService: AuthStateService,
    private router: Router) { }

  ngOnInit(): void { 
    setTimeout(() => this.formFields.forEach(ff => ff.updateOutlineGap()), 100);
  }

  get email() { return this.fg.get('email'); }
  get password() { return this.fg.get('password'); }

  handleSubmit() {
    this.setErrorsOnInputs(null);

    if (!this.fg.invalid)
      this.login();    
  }

  login() : void {
    const reqBody: LoginRequest = {
      email: this.email!.value,
      plainPassword: this.password!.value
    }

    this.authApiService.login(reqBody)
      .subscribe({
        error: ({status}) => {
          if (status == 401) {
            const err = {"401": true};
            this.fg.setErrors(err);
            this.setErrorsOnInputs(err)
          } else if (status == 403) {
            this.authStateService.emailToVerify = reqBody.email
            this.router.navigate(['/auth/email-verification']);
          }
        }
      });
  }

  private setErrorsOnInputs(errors: any) : void {
    this.email?.setErrors(errors);
    this.password?.setErrors(errors);
  }
}
