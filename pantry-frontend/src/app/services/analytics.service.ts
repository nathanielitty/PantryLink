import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';

export interface SystemAnalytics {
  id: number;
  date: string;
  totalActivePantries: number;
  totalUsers: number;
  totalInventoryItems: number;
  totalSearches: number;
  totalRecommendations: number;
  mostSearchedZipCode?: string;
  totalItemsDistributed: number;
  averageSystemRating: number;
}

export interface PantryAnalytics {
  id: number;
  pantryId: number;
  pantryName: string;
  date: string;
  totalItemsCount: number;
  totalQuantity: number;
  expiringItemsCount: number;
  visitorCount: number;
  inventoryUpdatesCount: number;
  averageRating: number;
  mostPopularCategory?: string;
  itemsDistributedCount: number;
}

export interface AnalyticsDashboard {
  systemStats: SystemAnalytics;
  topPerformingPantries: PantryAnalytics[];
  popularCategories: { [key: string]: number };
  searchesByZipCode: { [key: string]: number };
  recentActivity: PantryAnalytics[];
}

export interface ZipDistance {
  id: number;
  fromZipCode: string;
  toZipCode: string;
  distanceMiles: number;
  estimatedTravelTimeMinutes: number;
  lastUpdated: string;
  isVerified: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AnalyticsService {
  private readonly apiUrl = environment.apiBaseUrl;

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  getDashboard(): Observable<AnalyticsDashboard> {
    return this.http.get<AnalyticsDashboard>(`${this.apiUrl}/analytics/dashboard`, {
      headers: this.getAuthHeaders()
    });
  }

  getSystemAnalytics(date?: string): Observable<SystemAnalytics> {
    let url = `${this.apiUrl}/analytics/system`;
    if (date) {
      url += `?date=${encodeURIComponent(date)}`;
    }
    return this.http.get<SystemAnalytics>(url, {
      headers: this.getAuthHeaders()
    });
  }

  getPantryAnalytics(pantryId: number, date?: string): Observable<PantryAnalytics> {
    let url = `${this.apiUrl}/analytics/pantry/${pantryId}`;
    if (date) {
      url += `?date=${encodeURIComponent(date)}`;
    }
    return this.http.get<PantryAnalytics>(url, {
      headers: this.getAuthHeaders()
    });
  }

  getTopPantries(count: number = 10): Observable<PantryAnalytics[]> {
    return this.http.get<PantryAnalytics[]>(`${this.apiUrl}/analytics/top-pantries`, { 
      params: { count: count.toString() },
      headers: this.getAuthHeaders()
    });
  }

  getPopularCategories(): Observable<{ [key: string]: number }> {
    return this.http.get<{ [key: string]: number }>(`${this.apiUrl}/analytics/popular-categories`, {
      headers: this.getAuthHeaders()
    });
  }

  getSearchesByZipCode(): Observable<{ [key: string]: number }> {
    return this.http.get<{ [key: string]: number }>(`${this.apiUrl}/analytics/searches-by-zip`, {
      headers: this.getAuthHeaders()
    });
  }

  updatePantryAnalytics(pantryId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/analytics/update-pantry/${pantryId}`, {}, {
      headers: this.getAuthHeaders()
    });
  }

  updateSystemAnalytics(): Observable<any> {
    return this.http.post(`${this.apiUrl}/analytics/update-system`, {}, {
      headers: this.getAuthHeaders()
    });
  }

  getDistance(fromZip: string, toZip: string): Observable<ZipDistance> {
    return this.http.get<ZipDistance>(`${this.apiUrl}/analytics/distance`, {
      params: { fromZip, toZip },
      headers: this.getAuthHeaders()
    });
  }

  getNearbyZipCodes(zipCode: string, maxDistance: number = 25): Observable<ZipDistance[]> {
    return this.http.get<ZipDistance[]>(`${this.apiUrl}/analytics/nearby-zips`, {
      params: { zipCode, maxDistance: maxDistance.toString() }
    });
  }
}
