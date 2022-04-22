import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatStepperModule } from '@angular/material/stepper';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { LoginFormComponent } from './pages/login/login-form/login-form.component';
import { RouterModule } from '@angular/router';
import { FloatingBoxComponent } from './components/floating-box/floating-box.component';
import { RegisterStep1InfoComponent } from './pages/register/register-step1-info/register-step1-info.component';
import { RegisterStep2InfoComponent } from './pages/register/register-step2-info/register-step2-info.component';
import { EmailVerificationComponent } from './pages/email-verification/email-verification.component';
import { AuthApiService } from './auth-api.service';
import { EmailVerificationFormComponent } from './pages/email-verification/email-verification-form/email-verification-form.component';
import { AuthTokensService } from './auth-tokens.service';
import { EmailVerificationStateService } from './pages/email-verification/email-verification-state.service';
import { AuthService } from './auth.service';
import { AuthGuardService } from './auth-guard.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatStepperModule,
        MatDatepickerModule,
        MatNativeDateModule,
        RouterModule,
    ],
    declarations: [
        LoginComponent,
        RegisterComponent,
        LoginFormComponent,
        FloatingBoxComponent,
        RegisterStep1InfoComponent,
        RegisterStep2InfoComponent,
        EmailVerificationComponent,
        EmailVerificationFormComponent,
    ],
    providers: [
        AuthApiService,
        AuthTokensService,
        AuthService,
        AuthGuardService,
        JwtHelperService,
        EmailVerificationStateService
    ]
})
export class AuthModule { }