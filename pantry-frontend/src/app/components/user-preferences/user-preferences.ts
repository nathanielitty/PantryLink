import { Component, OnInit, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';

interface UserPreference {
  id: string;
  name: string;
  enabled: boolean;
}

interface DietaryPreference extends UserPreference {}
interface AllergiesPreference extends UserPreference {}
interface NotificationPreference extends UserPreference {}

@Component({
  selector: 'app-user-preferences',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule],
  templateUrl: './user-preferences.html',
  styleUrl: './user-preferences.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class UserPreferences implements OnInit {
  profileForm: FormGroup;
  isLoading = false;
  isSaving = false;
  saveSuccess = false;
  saveError = '';
  
  // Dietary preferences
  dietaryPreferences: DietaryPreference[] = [
    { id: 'vegetarian', name: 'Vegetarian', enabled: false },
    { id: 'vegan', name: 'Vegan', enabled: false },
    { id: 'gluten-free', name: 'Gluten-Free', enabled: false },
    { id: 'dairy-free', name: 'Dairy-Free', enabled: false },
    { id: 'nut-free', name: 'Nut-Free', enabled: false },
    { id: 'kosher', name: 'Kosher', enabled: false },
    { id: 'halal', name: 'Halal', enabled: false }
  ];
  
  // Allergies
  allergiesPreferences: AllergiesPreference[] = [
    { id: 'peanuts', name: 'Peanuts', enabled: false },
    { id: 'tree-nuts', name: 'Tree Nuts', enabled: false },
    { id: 'dairy', name: 'Dairy', enabled: false },
    { id: 'eggs', name: 'Eggs', enabled: false },
    { id: 'wheat', name: 'Wheat', enabled: false },
    { id: 'soy', name: 'Soy', enabled: false },
    { id: 'fish', name: 'Fish', enabled: false },
    { id: 'shellfish', name: 'Shellfish', enabled: false }
  ];
  
  // Notification preferences
  notificationPreferences: NotificationPreference[] = [
    { id: 'email', name: 'Email Notifications', enabled: true },
    { id: 'sms', name: 'SMS Notifications', enabled: false },
    { id: 'push', name: 'Push Notifications', enabled: true },
    { id: 'inventory-updates', name: 'Inventory Updates', enabled: true },
    { id: 'new-pantries', name: 'New Pantries in Area', enabled: true },
    { id: 'expiring-items', name: 'Expiring Items Alerts', enabled: false }
  ];
  
  // Search radius options
  radiusOptions = [
    { value: 5, label: '5 miles' },
    { value: 10, label: '10 miles' },
    { value: 15, label: '15 miles' },
    { value: 25, label: '25 miles' },
    { value: 50, label: '50 miles' }
  ];

  constructor(private fb: FormBuilder) {
    this.profileForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.pattern('^[0-9]{10}$')],
      zipCode: ['', [Validators.required, Validators.pattern('^[0-9]{5}$')]],
      searchRadius: [10],
      familySize: [1, [Validators.required, Validators.min(1), Validators.max(20)]],
      transportationMethod: ['car']
    });
  }

  ngOnInit() {
    this.loadUserPreferences();
  }

  loadUserPreferences() {
    this.isLoading = true;
    
    // Simulate API call
    setTimeout(() => {
      // Mock data
      this.profileForm.patchValue({
        firstName: 'John',
        lastName: 'Doe',
        email: 'john.doe@example.com',
        phone: '5551234567',
        zipCode: '12345',
        searchRadius: 15,
        familySize: 4,
        transportationMethod: 'public'
      });
      
      // Set dietary preferences
      this.dietaryPreferences[0].enabled = true; // Vegetarian
      this.dietaryPreferences[2].enabled = true; // Gluten-Free
      
      // Set allergies
      this.allergiesPreferences[0].enabled = true; // Peanuts
      this.allergiesPreferences[4].enabled = true; // Wheat
      
      // Set notification preferences
      this.notificationPreferences[1].enabled = true; // SMS
      
      this.isLoading = false;
    }, 1000);
  }

  savePreferences() {
    if (this.profileForm.valid) {
      this.isSaving = true;
      this.saveSuccess = false;
      this.saveError = '';
      
      // Prepare data for API
      const userData = {
        profile: this.profileForm.value,
        dietaryPreferences: this.dietaryPreferences.filter(p => p.enabled).map(p => p.id),
        allergies: this.allergiesPreferences.filter(p => p.enabled).map(p => p.id),
        notifications: this.notificationPreferences.filter(p => p.enabled).map(p => p.id)
      };
      
      // Simulate API call
      setTimeout(() => {
        console.log('Saving user preferences:', userData);
        this.isSaving = false;
        this.saveSuccess = true;
        
        // Hide success message after 3 seconds
        setTimeout(() => {
          this.saveSuccess = false;
        }, 3000);
      }, 1500);
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.profileForm.controls).forEach(key => {
        const control = this.profileForm.get(key);
        control?.markAsTouched();
      });
    }
  }

  togglePreference(preference: UserPreference) {
    preference.enabled = !preference.enabled;
  }
}
