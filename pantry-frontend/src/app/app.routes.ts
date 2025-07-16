import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home';
import { LoginComponent } from './components/login/login';
import { Register } from './components/register/register';
import { PantrySearch } from './components/pantry-search/pantry-search';
import { PantryDetail } from './components/pantry-detail/pantry-detail';
import { InventoryManagement } from './components/inventory-management/inventory-management';
import { BarcodeScanner } from './components/barcode-scanner/barcode-scanner';
import { UserPreferences } from './components/user-preferences/user-preferences';
import { AnalyticsDashboardComponent } from './components/analytics-dashboard/analytics-dashboard.component';
import { FoodDonationComponent } from './components/food-donation/food-donation';
import { MonetaryDonationComponent } from './components/monetary-donation/monetary-donation';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: Register },
  { path: 'search', component: PantrySearch },
  { path: 'pantry/:id', component: PantryDetail },
  { path: 'inventory/:pantryId', component: InventoryManagement },
  { path: 'scanner/:pantryId', component: BarcodeScanner },
  { path: 'preferences', component: UserPreferences },
  { path: 'analytics', component: AnalyticsDashboardComponent },
  { path: 'donate/food', component: FoodDonationComponent },
  { path: 'donate/money', component: MonetaryDonationComponent },
  { path: '**', redirectTo: '' }
];
