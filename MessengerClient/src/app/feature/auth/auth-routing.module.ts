import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { EmailVerificationComponent } from "./pages/email-verification/email-verification.component";
import { LoginComponent } from "./pages/login/login.component";
import { RegisterComponent } from "./pages/register/register.component";

const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'email-verification', component: EmailVerificationComponent },
];;

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }