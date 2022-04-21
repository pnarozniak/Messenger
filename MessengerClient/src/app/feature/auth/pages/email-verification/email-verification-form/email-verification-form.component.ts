import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertsService } from 'src/app/core/services/alerts.service';
import { AuthApiService } from '../../../auth-api.service';
import { EmailVerificationRequestModel } from '../../../auth.model';
import { EmailVerificationStateService } from '../email-verification-state.service';

@Component({
  selector: 'app-email-verification-form',
  templateUrl: './email-verification-form.component.html',
  styleUrls: ['./email-verification-form.component.scss']
})
export class EmailVerificationFormComponent implements OnInit {
  @ViewChild('form') form!: ElementRef<HTMLInputElement>;
  fg: FormGroup = new FormGroup({
    field_1: new FormControl('', Validators.required),
    field_2: new FormControl('', Validators.required),
    field_3: new FormControl('', Validators.required),
    field_4: new FormControl('', Validators.required),
    field_5: new FormControl('', Validators.required),
    field_6: new FormControl('', Validators.required),
  });

  constructor(private authApiService: AuthApiService,
    private emailVerificationStateService: EmailVerificationStateService,
    private router: Router,
    private alertsService: AlertsService) { }

  ngOnInit(): void { 
    if (!this.emailVerificationStateService.email)
      this.router.navigate(['/auth/login']);
  }

  get verificationToken() : string { 
    let code = '';
    for (const key in this.fg.controls) {
      if (this.fg.controls.hasOwnProperty(key)) {
        const element = this.fg.controls[key];
        if (element.value)
          code += element.value
      }
    }
    return code;
  }
  
  handleSubmit() {
    if (this.fg.invalid)
      return;

    this.verifyEmail();
  }

  verifyEmail() {
    const request: EmailVerificationRequestModel = {
      email: this.emailVerificationStateService.email!,
      token: this.verificationToken
    };

    this.authApiService.verifyEmail(request)
    .subscribe(
      {
        next: (_) => {
          this.router.navigate(['/auth/login']);
          this.alertsService.showSuccess('Email verified successfully. You can log in now.');
        },
        error: ({status}) => {
          if (status == 404) {
            const err = {"404": true};
            this.fg.setErrors(err);
          }
        }
      }
    );
  }

  handleKeyDown(event: KeyboardEvent) {
    event.preventDefault();

    const re = /^[0-9]{1}$/;
    if (!re.test(event.key))
      return;
    
    const formControlName = (event.target as HTMLInputElement).getAttribute('formControlName');
    this.fg.get(formControlName!)?.setValue(event.key);
    this.fg.updateValueAndValidity();

    const formControlNumber: number = parseInt(formControlName!.split("_")[1]);
    const nextFormControlName = `field_${formControlNumber + 1}`;
    const element = this.form.nativeElement.querySelector(`[formControlName="${nextFormControlName}"]`) 
    if (element) {
      (element as HTMLInputElement).focus();
    }
  }
}
