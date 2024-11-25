import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { ErrorStateMatcher, ShowOnDirtyErrorStateMatcher } from '@angular/material/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApiServiceService } from 'src/app/services/api-service.service';
import { noWhitespaceValidator } from 'src/app/validators/no-whitespace-validatior';
@Component({
	selector: 'app-user-form',
	templateUrl: './user-form.component.html',
	styleUrls: ['./user-form.component.scss'],
	changeDetection: ChangeDetectionStrategy.OnPush,

})
export class UserFormComponent {
	userForm: FormGroup;
	isSubmitting = false;
	submissionSuccess = false;
	submissionError = '';

	@ViewChild(FormGroupDirective) formGroupDirective!: FormGroupDirective;
	constructor(private fb: FormBuilder,
		private userService: ApiServiceService,
		private cdr: ChangeDetectorRef,
		private snackBar: MatSnackBar) {
		this.userForm = this.fb.group({
			firstName: ['', [Validators.required, Validators.maxLength(50), noWhitespaceValidator]],
			lastName: ['', [Validators.required, Validators.maxLength(50), noWhitespaceValidator]],
		});
	}
	onSubmit() {
		if (this.userForm.invalid) {
			return;
		}

		this.isSubmitting = true;
		this.submissionError = '';
		this.userService.createUser(this.userForm.value).subscribe({
			next: () => {
				this.submissionSuccess = true;
				this.isSubmitting = false;

				this.userForm.reset();
				this.formGroupDirective.resetForm();

				this.cdr.markForCheck();

				this.snackBar.open('User saved successfully!', 'Close', {
					duration: 3000,
				});
			},
			error: (err) => {
				this.submissionError = err.error?.message || 'An error occurred while saving the user.';
				this.isSubmitting = false;
				this.cdr.markForCheck();

				this.snackBar.open(this.submissionError, 'Close', {
					duration: 3000,
				});
			},
		});
	}

	get firstNameControl(): AbstractControl {
		return this.userForm.get('firstName')!;
	}

	get lastNameControl(): AbstractControl {
		return this.userForm.get('lastName')!;
	}
}
