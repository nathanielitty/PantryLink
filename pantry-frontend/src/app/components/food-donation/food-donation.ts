import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DonationService, FoodDonation, DonationItem } from '../../services/donation.service';
import { PantryService } from '../../services/pantry.service';
import { AuthService } from '../../services/auth.service';
import { BreadcrumbService } from '../../services/breadcrumb.service';

@Component({
  selector: 'app-food-donation',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './food-donation.html',
  styleUrl: './food-donation.scss'
})
export class FoodDonationComponent implements OnInit {
  donation: FoodDonation = {
    pantryId: 0,
    items: [this.createNewItem()],
    contactInfo: {
      name: '',
      email: '',
      phone: ''
    },
    preferredPickupTime: '',
    notes: ''
  };

  pantries: any[] = [];
  foodCategories = [
    'Canned Goods',
    'Fresh Produce',
    'Dairy',
    'Meat & Poultry',
    'Bread & Grains',
    'Beverages',
    'Snacks',
    'Baby Food',
    'Frozen Foods',
    'Personal Care',
    'Other'
  ];

  units = ['lbs', 'oz', 'items', 'cans', 'boxes', 'bottles', 'bags'];
  isSubmitting = false;

  constructor(
    private donationService: DonationService,
    private pantryService: PantryService,
    private authService: AuthService,
    private breadcrumbService: BreadcrumbService,
    private router: Router
  ) {}

  ngOnInit() {
    this.breadcrumbService.setBreadcrumbs([
      { label: 'Donate', url: '/donate/food', icon: 'fas fa-heart' },
      { label: 'Food Donation', url: '/donate/food', icon: 'fas fa-apple-alt' }
    ]);
    
    this.loadPantries();
    this.loadUserInfo();
  }

  loadPantries() {
    // Use a common zip code to get pantries or fallback to sample data
    this.pantryService.getPantriesByZip('00000').subscribe({
      next: (pantries: any[]) => {
        this.pantries = pantries.length > 0 ? pantries : [
          { id: 1, name: 'Downtown Community Pantry', address: '123 Main St' },
          { id: 2, name: 'Westside Food Bank', address: '456 West Ave' },
          { id: 3, name: 'East End Pantry', address: '789 East Rd' }
        ];
      },
      error: (error: any) => {
        console.error('Error loading pantries:', error);
        // Fallback to placeholder data
        this.pantries = [
          { id: 1, name: 'Downtown Community Pantry', address: '123 Main St' },
          { id: 2, name: 'Westside Food Bank', address: '456 West Ave' },
          { id: 3, name: 'East End Pantry', address: '789 East Rd' }
        ];
      }
    });
  }

  loadUserInfo() {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.donation.contactInfo.name = `${user.firstName} ${user.lastName}`;
      this.donation.contactInfo.email = user.email;
    }
  }

  createNewItem(): DonationItem {
    return {
      name: '',
      quantity: 1,
      unit: 'items',
      category: 'Other'
    };
  }

  addItem() {
    this.donation.items.push(this.createNewItem());
  }

  removeItem(index: number) {
    if (this.donation.items.length > 1) {
      this.donation.items.splice(index, 1);
    }
  }

  async submitDonation() {
    if (!this.isFormValid()) {
      return;
    }

    this.isSubmitting = true;
    try {
      await this.donationService.submitFoodDonation(this.donation);
      alert('Thank you! Your food donation has been submitted. The pantry will contact you to arrange pickup.');
      this.router.navigate(['/']);
    } catch (error) {
      alert('Error submitting donation. Please try again.');
      console.error('Donation error:', error);
    } finally {
      this.isSubmitting = false;
    }
  }

  isFormValid(): boolean {
    return this.donation.pantryId !== 0 &&
      !!this.donation.contactInfo.name &&
      !!this.donation.contactInfo.email &&
      !!this.donation.contactInfo.phone &&
      this.donation.items.length > 0 &&
      this.donation.items.every((item: DonationItem) => !!item.name && item.quantity > 0);
  }
}