import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { 
  Pantry, 
  PantryRecommendation 
} from '../models/pantry.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PantryService {
  private readonly apiUrl = `${environment.apiBaseUrl}/pantries`;

  constructor(private http: HttpClient) {}

  getPantriesByZip(zipCode: string): Observable<Pantry[]> {
    const params = new HttpParams().set('zip', zipCode);
    return this.http.get<Pantry[]>(this.apiUrl, { params });
  }

  getPantryById(id: number): Observable<Pantry> {
    return this.http.get<Pantry>(`${this.apiUrl}/${id}`);
  }

  createPantry(pantry: Partial<Pantry>): Observable<Pantry> {
    return this.http.post<Pantry>(this.apiUrl, pantry);
  }

  updatePantry(id: number, pantry: Partial<Pantry>): Observable<Pantry> {
    return this.http.put<Pantry>(`${this.apiUrl}/${id}`, pantry);
  }

  deletePantry(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getRecommendedPantries(zipCode: string): Observable<PantryRecommendation[]> {
    const params = new HttpParams().set('zip', zipCode);
    return this.http.get<PantryRecommendation[]>(`${environment.apiBaseUrl}/recommendations`, { params });
  }
}
