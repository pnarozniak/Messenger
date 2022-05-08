import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { RegisterStep1InfoComponent } from '../../components/register-step1-info/register-step1-info.component';
import { RegisterStep2InfoComponent } from '../../components/register-step2-info/register-step2-info.component';
import { RegisterRequest } from '../../models/register-request.model';
import { AuthApiService } from '../../services/auth-api.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  @ViewChild(RegisterStep1InfoComponent) step1InfoComponent?: RegisterStep1InfoComponent;
  @ViewChild(RegisterStep2InfoComponent) step2InfoComponent?: RegisterStep2InfoComponent;

  constructor(private authApiService: AuthApiService) { }

  ngOnInit(): void { }

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

    const request : RegisterRequest = {
      firstName: this.step1InfoFormGroup.get('firstName')?.value,
      lastName: this.step1InfoFormGroup.get('lastName')?.value,
      birthDate: this.step1InfoFormGroup.get('birthDate')?.value,
      email: this.step2InfoFormGroup.get('email')?.value,
      plainPassword: this.step2InfoFormGroup.get('password')?.value,
    }

    this.authApiService.register(request)
      .subscribe({
        error: ({status}) => {
          if (status == 409)
            this.step2InfoFormGroup.get('email')?.setErrors({'409': true});        }
      })
  }
}
