import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DonationService, MonetaryDonation } from '../../services/donation.service';
import { PantryService } from '../../services/pantry.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-monetary-donation',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './monetary-donation.html',
  styleUrls: ['./monetary-donation.scss']
})
export class MonetaryDonationComponent implements OnInit {
  isSubmitting = false;
  
  donation: MonetaryDonation = {
    pantryId: 0,
    amount: 0,
    contactInfo: {
      name: '',
      email: '',
      phone: ''
    },
    paymentMethod: 'credit-card',
    isRecurring: false,
    notes: ''
  };

  pantries: any[] = [];
  paymentMethods = [
    { value: 'credit-card', label: 'Credit Card' },
    { value: 'debit-card', label: 'Debit Card' },
    { value: 'paypal', label: 'PayPal' },
    { value: 'bank-transfer', label: 'Bank Transfer' }
  ];

  predefinedAmounts = [10, 25, 50, 100, 250, 500];

  constructor(
    private donationService: DonationService,
    private pantryService: PantryService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
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
    const currentUser = this.authService.getCurrentUser();
    if (currentUser) {
      this.donation.contactInfo.name = `${currentUser.firstName} ${currentUser.lastName}`.trim() || '';
      this.donation.contactInfo.email = currentUser.email || '';
    }
  }

  setAmount(amount: number) {
    this.donation.amount = amount;
  }

  submitDonation() {
    if (!this.isFormValid()) {
      return;
    }

    this.isSubmitting = true;

    this.donationService.submitMonetaryDonation(this.donation).subscribe({
      next: (response: any) => {
        console.log('Donation submitted successfully:', response);
        alert('Thank you for your generous donation! You will receive a confirmation email shortly.');
        this.router.navigate(['/']);
      },
      error: (error: any) => {
        console.error('Error submitting donation:', error);
        alert('There was an error processing your donation. Please try again.');
        this.isSubmitting = false;
      }
    });
  }

  isFormValid(): boolean {
    return this.donation.pantryId !== 0 &&
      this.donation.amount > 0 &&
      !!this.donation.contactInfo.name &&
      !!this.donation.contactInfo.email &&
      !!this.donation.contactInfo.phone &&
      !!this.donation.paymentMethod;
  }
}
