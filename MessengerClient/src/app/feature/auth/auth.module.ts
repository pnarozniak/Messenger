import { NgModule } from '@angular/core';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { FloatingBoxComponent } from './components/floating-box/floating-box.component';
import { EmailVerificationComponent } from './pages/email-verification/email-verification.component';
import { AuthApiService } from './services/auth-api.service';
import { SharedModule } from 'src/app/shared/shared.module';
import { AuthStateService } from './services/auth-state.service';
import { EmailVerificationFormComponent } from './components/email-verification-form/email-verification-form.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { RegisterStep1InfoComponent } from './components/register-step1-info/register-step1-info.component';
import { RegisterStep2InfoComponent } from './components/register-step2-info/register-step2-info.component';

@NgModule({
    imports: [
        SharedModule
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
        AuthStateService
    ]
})
export class AuthModule { }