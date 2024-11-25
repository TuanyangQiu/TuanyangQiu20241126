
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function OnlyAtoZLetterValidator(): ValidatorFn {
	return (control: AbstractControl): ValidationErrors | null => {
		const value = control.value;
		if (value === null || value === undefined) {
			return null;
		}
		const hasSpecialCharacters = /[^a-zA-Z\s]/.test(value);
		return hasSpecialCharacters ? { 'noSpecialCharacters': true } : null;
	};
}
