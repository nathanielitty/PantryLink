import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface FoodDonation {
  pantryId: number;
  items: DonationItem[];
  contactInfo: {
    name: string;
    email: string;
    phone: string;
  };
  preferredPickupTime: string;
  notes: string;
}

export interface DonationItem {
  name: string;
  quantity: number;
  unit: string;
  expirationDate?: string;
  category: string;
}

export interface MonetaryDonation {
  pantryId: number;
  amount: number;
  contactInfo: {
    name: string;
    email: string;
    phone: string;
  };
  paymentMethod: string;
  isRecurring: boolean;
  notes: string;
}

@Injectable({
  providedIn: 'root'
})
export class DonationService {
  private readonly apiUrl = `${environment.apiBaseUrl}/donations`;

  constructor(private http: HttpClient) {}

  submitFoodDonation(donation: FoodDonation): Observable<any> {
    return this.http.post(`${this.apiUrl}/food`, donation);
  }

  submitMonetaryDonation(donation: MonetaryDonation): Observable<any> {
    return this.http.post(`${this.apiUrl}/monetary`, donation);
  }

  getDonationHistory(userId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/history/${userId}`);
  }

  getPantryDonations(pantryId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/pantry/${pantryId}`);
  }
}
