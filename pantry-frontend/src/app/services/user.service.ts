import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { 
  User, 
  DietaryPreference, 
  UserPreferencesRequest 
} from '../models/pantry.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly apiUrl = `${environment.apiBaseUrl}/users`;

  constructor(private http: HttpClient) {}

  getUserProfile(): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/profile`);
  }

  updateUserPreferences(preferences: DietaryPreference[]): Observable<DietaryPreference[]> {
    const request: UserPreferencesRequest = { preferences };
    return this.http.put<DietaryPreference[]>(`${this.apiUrl}/preferences`, request);
  }

  getUserPreferences(): Observable<DietaryPreference[]> {
    return this.http.get<DietaryPreference[]>(`${this.apiUrl}/preferences`);
  }
}
