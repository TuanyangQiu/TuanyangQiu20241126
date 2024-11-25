import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserFormComponent } from './components/user-form/user-form.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ApiInterceptor } from './interceptors/api-interceptor';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule } from '@angular/material/snack-bar';
@NgModule({
	declarations: [
		AppComponent,
		UserFormComponent
	],
	imports: [
		BrowserModule,
		AppRoutingModule,
		ReactiveFormsModule,
		HttpClientModule,
		NoopAnimationsModule,
		MatInputModule,
		MatButtonModule,
		MatFormFieldModule,
		MatProgressSpinnerModule,
		MatCardModule,
		MatIconModule,
		MatSnackBarModule
	],
	providers: [{ provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true }],
	bootstrap: [AppComponent]
})
export class AppModule { }
