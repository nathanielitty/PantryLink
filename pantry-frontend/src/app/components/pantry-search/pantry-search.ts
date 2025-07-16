import { Component, OnInit, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';

interface Pantry {
  id: number;
  name: string;
  address: string;
  distance: number;
  phone: string;
  hours: string;
  availableItems: number;
  lastUpdated: Date;
  status: 'open' | 'closed' | 'limited';
  imageUrl?: string;
  tags?: string[];
}

@Component({
  selector: 'app-pantry-search',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './pantry-search.html',
  styleUrl: './pantry-search.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PantrySearch implements OnInit {
  zipCode: string = '';
  searchRadius: number = 10;
  pantries: Pantry[] = [];
  isLoading: boolean = false;
  hasSearched: boolean = false;

  // Mock data for demonstration
  mockPantries: Pantry[] = [
    {
      id: 1,
      name: 'Community Food Bank',
      address: '123 Main St, Anytown, ST 12345',
      distance: 2.3,
      phone: '(555) 123-4567',
      hours: 'Mon-Fri 9AM-5PM',
      availableItems: 120,
      lastUpdated: new Date(),
      status: 'open',
      imageUrl: 'assets/pantry-1.svg',
      tags: ['Wheelchair Accessible', 'Fresh Produce', 'Dairy']
    },
    {
      id: 2,
      name: 'Hope Kitchen',
      address: '456 Oak Ave, Anytown, ST 12345',
      distance: 3.7,
      phone: '(555) 987-6543',
      hours: 'Tue, Thu 10AM-3PM',
      availableItems: 85,
      lastUpdated: new Date(),
      status: 'limited',
      imageUrl: 'assets/pantry-2.svg',
      tags: ['Hot Meals', 'Canned Goods', 'Baby Items']
    },
    {
      id: 3,
      name: 'Neighborhood Pantry',
      address: '789 Pine Rd, Anytown, ST 12345',
      distance: 5.1,
      phone: '(555) 456-7890',
      hours: 'Wed, Sat 8AM-12PM',
      availableItems: 150,
      lastUpdated: new Date(),
      status: 'open',
      imageUrl: 'assets/pantry-3.svg',
      tags: ['Organic', 'Gluten-Free', 'Community Garden']
    },
    {
      id: 4,
      name: 'Helping Hands Food Bank',
      address: '321 Elm St, Anytown, ST 12345',
      distance: 6.8,
      phone: '(555) 789-0123',
      hours: 'Mon, Wed, Fri 10AM-4PM',
      availableItems: 95,
      lastUpdated: new Date(),
      status: 'open',
      imageUrl: 'assets/pantry-1.svg',
      tags: ['Pet Food', 'Hygiene Products', 'School Supplies']
    },
    {
      id: 5,
      name: 'Faith Community Pantry',
      address: '555 Church St, Anytown, ST 12345',
      distance: 7.2,
      phone: '(555) 234-5678',
      hours: 'Tue, Thu 1PM-6PM, Sat 9AM-12PM',
      availableItems: 65,
      lastUpdated: new Date(),
      status: 'limited',
      imageUrl: 'assets/pantry-2.svg',
      tags: ['Kosher', 'Halal', 'Cultural Foods']
    },
    {
      id: 6,
      name: 'University Food Pantry',
      address: '100 College Ave, Anytown, ST 12345',
      distance: 8.5,
      phone: '(555) 345-6789',
      hours: 'Mon-Fri 11AM-7PM',
      availableItems: 110,
      lastUpdated: new Date(),
      status: 'open',
      imageUrl: 'assets/pantry-3.svg',
      tags: ['Student ID Required', 'Ready-to-Eat', 'Vegan Options']
    }
  ];

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    // Check if ZIP code was passed from home page
    this.route.queryParams.subscribe(params => {
      if (params['zip']) {
        this.zipCode = params['zip'];
        this.searchPantries();
      }
    });
  }

  searchPantries() {
    if (this.zipCode && this.zipCode.length === 5) {
      this.isLoading = true;
      this.hasSearched = true;
      
      // Simulate API call
      setTimeout(() => {
        this.pantries = this.mockPantries.filter(p => p.distance <= this.searchRadius);
        this.isLoading = false;
      }, 1000);
    }
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'open': return 'success';
      case 'limited': return 'warning';
      case 'closed': return 'danger';
      default: return 'neutral';
    }
  }

  getStatusText(status: string): string {
    switch (status) {
      case 'open': return 'Open';
      case 'limited': return 'Limited Hours';
      case 'closed': return 'Closed';
      default: return 'Unknown';
    }
  }
}
