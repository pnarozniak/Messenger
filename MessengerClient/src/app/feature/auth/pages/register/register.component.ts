import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertsService } from 'src/app/core/services/alerts.service';
import { AuthApiService } from '../../auth-api.service';
import { RegisterRequestModel } from '../../auth.model';
import { RegisterStep1InfoComponent } from './register-step1-info/register-step1-info.component';
import { RegisterStep2InfoComponent } from './register-step2-info/register-step2-info.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  @ViewChild(RegisterStep1InfoComponent) step1InfoComponent?: RegisterStep1InfoComponent;
  @ViewChild(RegisterStep2InfoComponent) step2InfoComponent?: RegisterStep2InfoComponent;

  constructor(private authApiService: AuthApiService,
    private router: Router,
    private alertsService: AlertsService) { }

  ngOnInit(): void {
  }

  get step1InfoFormGroup() {
    return this.step1InfoComponent ? this.step1InfoComponent.fg : new FormGroup({});
  }

  get step2InfoFormGroup() {
    return this.step2InfoComponent ? this.step2InfoComponent.fg : new FormGroup({});
  }

  handleRegister() {
    if (this.step2InfoFormGroup.invalid)
      return;

    this.step2InfoFormGroup.get('email')
      ?.setErrors(null);

    const request : RegisterRequestModel = {
      firstName: this.step1InfoFormGroup.get('firstName')?.value,
      lastName: this.step1InfoFormGroup.get('lastName')?.value,
      birthDate: this.step1InfoFormGroup.get('birthDate')?.value,
      email: this.step2InfoFormGroup.get('email')?.value,
      plainPassword: this.step2InfoFormGroup.get('password')?.value,
    }

    this.authApiService.register(request)
      .subscribe({
        next: () => {
          this.router.navigate(['/auth/login']);
          this.alertsService.showSuccess('You have successfully registered! Please login.');
        },
        error: ({status}) => {
          console.log("No jest error")
          if (status == 409) {
            this.step2InfoFormGroup.get('email')
              ?.setErrors({'409': true});
          }
        }
      });
  }
}
