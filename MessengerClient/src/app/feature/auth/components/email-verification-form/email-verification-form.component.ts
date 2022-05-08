import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthStateService } from '../../services/auth-state.service';
import { EmailVerificationRequest } from '../../models/email-verification-request.model';
import { AuthApiService } from '../../services/auth-api.service';

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

  constructor(
    private authStateService: AuthStateService,
    private router: Router,
    private authApiService: AuthApiService) { }

  ngOnInit(): void { 
    if (!this.authStateService.emailToVerify)
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

  handleSubmit() {
    if (!this.fg.invalid)
      this.verifyEmail();
  }

  verifyEmail() {
    const reqBody: EmailVerificationRequest = {
      email: this.authStateService.emailToVerify!,
      token: this.verificationToken
    };

    this.authApiService.verifyEmail(reqBody)
      .subscribe({
        error: ({status}) => {
          if (status == 404) {
            const err = {"404": true};
            this.fg.setErrors(err);
          }
        }
    });
  }
}
