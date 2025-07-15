import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

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

  constructor(private http: HttpClient) {}

  getDashboard(): Observable<AnalyticsDashboard> {
    return this.http.get<AnalyticsDashboard>(`${this.apiUrl}/analytics/dashboard`);
  }

  getSystemAnalytics(date?: string): Observable<SystemAnalytics> {
    let url = `${this.apiUrl}/analytics/system`;
    if (date) {
      url += `?date=${encodeURIComponent(date)}`;
    }
    return this.http.get<SystemAnalytics>(url);
  }

  getPantryAnalytics(pantryId: number, date?: string): Observable<PantryAnalytics> {
    let url = `${this.apiUrl}/analytics/pantry/${pantryId}`;
    if (date) {
      url += `?date=${encodeURIComponent(date)}`;
    }
    return this.http.get<PantryAnalytics>(url);
  }

  getTopPantries(count: number = 10): Observable<PantryAnalytics[]> {
    return this.http.get<PantryAnalytics[]>(`${this.apiUrl}/analytics/top-pantries`, { 
      params: { count: count.toString() } 
    });
  }

  getPopularCategories(): Observable<{ [key: string]: number }> {
    return this.http.get<{ [key: string]: number }>(`${this.apiUrl}/analytics/popular-categories`);
  }

  getSearchesByZipCode(): Observable<{ [key: string]: number }> {
    return this.http.get<{ [key: string]: number }>(`${this.apiUrl}/analytics/searches-by-zip`);
  }

  updatePantryAnalytics(pantryId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/analytics/update-pantry/${pantryId}`, {});
  }

  updateSystemAnalytics(): Observable<any> {
    return this.http.post(`${this.apiUrl}/analytics/update-system`, {});
  }

  getDistance(fromZip: string, toZip: string): Observable<ZipDistance> {
    return this.http.get<ZipDistance>(`${this.apiUrl}/analytics/distance`, {
      params: { fromZip, toZip }
    });
  }

  getNearbyZipCodes(zipCode: string, maxDistance: number = 25): Observable<ZipDistance[]> {
    return this.http.get<ZipDistance[]>(`${this.apiUrl}/analytics/nearby-zips`, {
      params: { zipCode, maxDistance: maxDistance.toString() }
    });
  }
}
