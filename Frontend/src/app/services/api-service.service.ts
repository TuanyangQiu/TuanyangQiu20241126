import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../Models/User';
import { environment } from 'src/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class ApiServiceService {

	private userEndpoint = environment.apiUrl + '/user';
	constructor(private http: HttpClient) { }

	createUser(user: User): Observable<void> {
		return this.http.post<void>(this.userEndpoint, user);
	}

}
