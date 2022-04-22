import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from './feature/auth/auth-guard.service';
import { EmailVerificationComponent } from './feature/auth/pages/email-verification/email-verification.component';
import { LoginComponent } from './feature/auth/pages/login/login.component';
import { RegisterComponent } from './feature/auth/pages/register/register.component';
import { ChatComponent } from './feature/chat/pages/chat/chat.component';

const routes: Routes = [
  { path: 'auth/login', component: LoginComponent },
  { path: 'auth/register', component: RegisterComponent },
  { path: 'auth/email-verification', component: EmailVerificationComponent },
  { path: 'chat', component: ChatComponent, canActivate: [AuthGuardService] },
  { path: '', redirectTo: '/chat', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
