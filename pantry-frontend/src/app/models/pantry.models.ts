// Core models for PantryLink application
export interface User {
  id: number;
  userName: string;
  email: string;
  firstName: string;
  lastName: string;
  createdAt: Date;
  lastLoginAt?: Date;
  preferences: DietaryPreference[];
}

export interface Pantry {
  id: number;
  name: string;
  zipCode: string;
  address: string;
  phoneNumber?: string;
  email?: string;
  description?: string;
  openTime?: string;
  closeTime?: string;
  averageRating: number;
  totalRatings: number;
  totalInventoryItems: number;
}

export interface InventoryItem {
  id: number;
  pantryId: number;
  name: string;
  description: string;
  barcode?: string;
  quantity: number;
  unit: string;
  expirationDate?: Date;
  category: string;
  isVegan: boolean;
  isGlutenFree: boolean;
  isHalal: boolean;
  isKosher: boolean;
  priority: number;
}

export interface PantryRecommendation {
  pantry: Pantry;
  recommendedItems: InventoryItem[];
  score: number;
  reasonForRecommendation: string;
}

export enum DietaryPreference {
  Vegan = 'Vegan',
  Vegetarian = 'Vegetarian',
  Halal = 'Halal',
  Kosher = 'Kosher',
  GlutenFree = 'GlutenFree',
  DairyFree = 'DairyFree',
  NutFree = 'NutFree'
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

export interface AuthResponse {
  token: string;
  expiration: Date;
  user: {
    id: number;
    userName: string;
    email: string;
    firstName: string;
    lastName: string;
    roles: string[];
  };
}

export interface CreateInventoryItemRequest {
  pantryId: number;
  name: string;
  description: string;
  barcode?: string;
  quantity: number;
  unit: string;
  expirationDate?: Date;
  category: string;
  isVegan: boolean;
  isGlutenFree: boolean;
  isHalal: boolean;
  isKosher: boolean;
}

export interface UpdateStockRequest {
  barcode: string;
  quantityChange: number;
}

export interface UserPreferencesRequest {
  preferences: DietaryPreference[];
}
