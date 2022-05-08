import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatFormField } from '@angular/material/form-field';

@Component({
  selector: 'app-register-step1-info',
  templateUrl: './register-step1-info.component.html',
  styleUrls: ['./register-step1-info.component.scss']
})
export class RegisterStep1InfoComponent implements OnInit {
  @ViewChildren(MatFormField) formFields!: QueryList<MatFormField>;

  fg: FormGroup = new FormGroup({
    firstName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    lastName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    birthDate: new FormControl('', [Validators.required]),
  })

  constructor() { 
  }

  ngOnInit(): void {
    setTimeout(() => this.formFields.forEach(ff => ff.updateOutlineGap()), 100);
  }

  get firstName() { return this.fg.get('firstName'); }
  get lastName() { return this.fg.get('lastName'); }
  get birthDate() { return this.fg.get('birthDate'); }
}
