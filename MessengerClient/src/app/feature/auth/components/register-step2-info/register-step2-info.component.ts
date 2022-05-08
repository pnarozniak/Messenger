import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatFormField } from '@angular/material/form-field';
import { equalTo } from 'src/app/shared/validators/equalTo.validator';

@Component({
  selector: 'app-register-step2-info',
  templateUrl: './register-step2-info.component.html',
  styleUrls: ['./register-step2-info.component.scss']
})
export class RegisterStep2InfoComponent implements OnInit {
  @ViewChildren(MatFormField) formFields!: QueryList<MatFormField>;

  fg: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
    confirmPassword: new FormControl('', [Validators.required, equalTo('password')]),
  })

  constructor() { 
    setTimeout(() => this.formFields.forEach(ff => ff.updateOutlineGap()), 100);
  }

  ngOnInit(): void {
    this.fg.get('password')?.valueChanges.subscribe(_ => {
      this.fg.get('confirmPassword')?.updateValueAndValidity();
    });
  }

  get email() { return this.fg.get('email'); }
  get password() { return this.fg.get('password'); }
  get confirmPassword() { return this.fg.get('confirmPassword'); }
}
