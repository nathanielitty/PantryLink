import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { 
  InventoryItem, 
  CreateInventoryItemRequest, 
  UpdateStockRequest 
} from '../models/pantry.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  private readonly apiUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  getPantryInventory(pantryId: number): Observable<InventoryItem[]> {
    return this.http.get<InventoryItem[]>(`${this.apiUrl}/pantries/${pantryId}/inventory`);
  }

  getInventoryItem(pantryId: number, itemId: number): Observable<InventoryItem> {
    return this.http.get<InventoryItem>(`${this.apiUrl}/pantries/${pantryId}/inventory/${itemId}`);
  }

  addInventoryItem(item: CreateInventoryItemRequest): Observable<InventoryItem> {
    return this.http.post<InventoryItem>(`${this.apiUrl}/pantries/${item.pantryId}/inventory`, item);
  }

  updateInventoryItem(pantryId: number, itemId: number, item: Partial<CreateInventoryItemRequest>): Observable<InventoryItem> {
    return this.http.put<InventoryItem>(`${this.apiUrl}/pantries/${pantryId}/inventory/${itemId}`, item);
  }

  updateStockByBarcode(pantryId: number, stockUpdate: UpdateStockRequest): Observable<InventoryItem> {
    return this.http.post<InventoryItem>(`${this.apiUrl}/pantries/${pantryId}/inventory/update-stock`, stockUpdate);
  }

  deleteInventoryItem(pantryId: number, itemId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/pantries/${pantryId}/inventory/${itemId}`);
  }

  searchItems(query: string): Observable<InventoryItem[]> {
    return this.http.get<InventoryItem[]>(`${this.apiUrl}/search/items?query=${encodeURIComponent(query)}`);
  }

  importFromCsv(pantryId: number, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.apiUrl}/pantries/${pantryId}/inventory/import/csv`, formData);
  }

  importFromJson(pantryId: number, jsonData: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/pantries/${pantryId}/inventory/import/json`, jsonData, {
      headers: { 'Content-Type': 'application/json' }
    });
  }
}
