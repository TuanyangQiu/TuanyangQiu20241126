import { ComponentFixture, flush, TestBed, waitForAsync } from '@angular/core/testing';
import { UserFormComponent } from './user-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ApiServiceService } from 'src/app/services/api-service.service';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { of, throwError } from 'rxjs';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { fakeAsync, tick } from '@angular/core/testing';

describe('UserFormComponent', () => {
	let component: UserFormComponent;
	let fixture: ComponentFixture<UserFormComponent>;
	let apiServiceSpy: jasmine.SpyObj<ApiServiceService>;

	beforeEach(waitForAsync(() => {
		const spy = jasmine.createSpyObj('ApiServiceService', ['createUser']);

		TestBed.configureTestingModule({
			declarations: [UserFormComponent],
			imports: [
				ReactiveFormsModule,
				MatSnackBarModule,
				BrowserAnimationsModule,
				HttpClientModule,
				MatFormFieldModule,
				MatInputModule,
				MatProgressSpinnerModule,
				MatIconModule,
				MatCardModule,
			],
			providers: [
				{ provide: ApiServiceService, useValue: spy },
			],
		}).compileComponents();

		apiServiceSpy = TestBed.inject(ApiServiceService) as jasmine.SpyObj<ApiServiceService>;
	}));

	beforeEach(() => {
		fixture = TestBed.createComponent(UserFormComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create the component', () => {
		expect(component).toBeTruthy();
	});

	it('should have an invalid form when empty', () => {
		expect(component.userForm.valid).toBeFalsy();
	});

	it('should validate firstName as required', () => {
		const firstNameControl = component.firstNameControl;
		expect(firstNameControl.valid).toBeFalsy();

		firstNameControl.setValue('');
		expect(firstNameControl.hasError('required')).toBeTruthy();
	});

	it('should validate lastName as required', () => {
		const lastNameControl = component.lastNameControl;
		expect(lastNameControl.valid).toBeFalsy();

		lastNameControl.setValue('');
		expect(lastNameControl.hasError('required')).toBeTruthy();
	});
	it('should submit the form successfully', fakeAsync(() => {
		apiServiceSpy.createUser.and.returnValue(of(undefined));

		component.userForm.setValue({ firstName: 'John', lastName: 'Doe' });

		component.onSubmit();

		tick();
		flush();

		fixture.detectChanges();

		expect(component.isSubmitting).toBeFalse();
		expect(component.submissionSuccess).toBeTrue();
		expect(apiServiceSpy.createUser).toHaveBeenCalledWith({ firstName: 'John', lastName: 'Doe' });
	}));


	it('should handle API error on submit', () => {

		apiServiceSpy.createUser.and.returnValue(throwError(() => new Error('API error')));

		component.userForm.setValue({ firstName: 'John', lastName: 'Doe' });

		component.onSubmit();

		expect(component.isSubmitting).toBeFalse();
		expect(component.submissionSuccess).toBeFalse();
		expect(component.submissionError).toBe('An error occurred while saving the user.');
		expect(apiServiceSpy.createUser).toHaveBeenCalled();
	});
});
