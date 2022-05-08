import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function equalTo(toFormControlName: string): ValidatorFn {
    return (control: AbstractControl):  ValidationErrors | null => {
        if (!control.parent)
            return null;

        return control.value === control.parent.get(toFormControlName)?.value 
        ? null
        : { equalto: true } 
    };
}