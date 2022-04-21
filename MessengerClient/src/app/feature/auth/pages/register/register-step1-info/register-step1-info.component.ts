import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';

@Component({
  selector: 'app-register-step1-info',
  templateUrl: './register-step1-info.component.html',
  styleUrls: ['./register-step1-info.component.scss']
})
export class RegisterStep1InfoComponent implements OnInit {
  fg: FormGroup = new FormGroup({
    firstName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    lastName: new FormControl('', [Validators.required, Validators.minLength(2)]),
    birthDate: new FormControl('', [Validators.required]),
  })

  constructor() { 
  }

  ngOnInit(): void {
  }

  get firstName() { return this.fg.get('firstName'); }
  get lastName() { return this.fg.get('lastName'); }
  get birthDate() { return this.fg.get('birthDate'); }
}
