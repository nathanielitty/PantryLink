import { Component, OnInit, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

interface PantryItem {
  id: number;
  name: string;
  category: string;
  quantity: number;
  expirationDate: Date;
  nutritionalInfo: string;
  dietaryFlags: string[];
}

interface Pantry {
  id: number;
  name: string;
  address: string;
  phone: string;
  email: string;
  website: string;
  hours: string;
  description: string;
  services: string[];
  requirements: string[];
  imageUrl: string;
  latitude: number;
  longitude: number;
  status: 'open' | 'closed' | 'limited';
  items: PantryItem[];
}

@Component({
  selector: 'app-pantry-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './pantry-detail.html',
  styleUrl: './pantry-detail.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PantryDetail implements OnInit {
  pantryId: number = 0;
  pantry: Pantry | null = null;
  isLoading: boolean = true;
  selectedCategory: string = 'all';
  searchTerm: string = '';
  
  // Mock data for demonstration
  mockPantry: Pantry = {
    id: 1,
    name: 'Community Food Bank',
    address: '123 Main St, Anytown, ST 12345',
    phone: '(555) 123-4567',
    email: 'info@communityfoodbank.org',
    website: 'https://www.communityfoodbank.org',
    hours: 'Monday-Friday: 9AM-5PM, Saturday: 10AM-2PM, Sunday: Closed',
    description: 'Community Food Bank has been serving the local area for over 15 years, providing nutritious food to those in need. We work with local grocery stores, farmers, and community gardens to ensure a variety of fresh and shelf-stable options.',
    services: ['Food Distribution', 'Nutrition Education', 'SNAP Application Assistance', 'Home Delivery for Seniors'],
    requirements: ['Photo ID', 'Proof of Address', 'Income Verification (if applicable)'],
    imageUrl: '/assets/default-pantry.jpg',
    latitude: 40.7128,
    longitude: -74.0060,
    status: 'open',
    items: [
      {
        id: 1,
        name: 'Canned Beans',
        category: 'Canned Goods',
        quantity: 45,
        expirationDate: new Date('2025-12-31'),
        nutritionalInfo: 'High in protein and fiber',
        dietaryFlags: ['Vegetarian', 'Vegan', 'Gluten-Free']
      },
      {
        id: 2,
        name: 'Rice',
        category: 'Grains',
        quantity: 30,
        expirationDate: new Date('2026-06-30'),
        nutritionalInfo: 'Good source of carbohydrates',
        dietaryFlags: ['Vegetarian', 'Vegan', 'Gluten-Free']
      },
      {
        id: 3,
        name: 'Pasta',
        category: 'Grains',
        quantity: 25,
        expirationDate: new Date('2026-03-15'),
        nutritionalInfo: 'Good source of carbohydrates',
        dietaryFlags: ['Vegetarian', 'Vegan']
      },
      {
        id: 4,
        name: 'Canned Soup',
        category: 'Canned Goods',
        quantity: 20,
        expirationDate: new Date('2025-10-15'),
        nutritionalInfo: 'Varies by type',
        dietaryFlags: []
      },
      {
        id: 5,
        name: 'Fresh Apples',
        category: 'Produce',
        quantity: 50,
        expirationDate: new Date('2025-08-10'),
        nutritionalInfo: 'High in fiber and vitamin C',
        dietaryFlags: ['Vegetarian', 'Vegan', 'Gluten-Free']
      },
      {
        id: 6,
        name: 'Milk',
        category: 'Dairy',
        quantity: 15,
        expirationDate: new Date('2025-08-05'),
        nutritionalInfo: 'Good source of calcium and vitamin D',
        dietaryFlags: ['Vegetarian']
      }
    ]
  };

  categories: string[] = [];

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.pantryId = +params['id'];
      this.loadPantryDetails();
    });
  }

  loadPantryDetails() {
    // Simulate API call
    setTimeout(() => {
      this.pantry = this.mockPantry;
      this.extractCategories();
      this.isLoading = false;
    }, 1000);
  }

  extractCategories() {
    if (this.pantry) {
      const categorySet = new Set<string>();
      this.pantry.items.forEach(item => categorySet.add(item.category));
      this.categories = Array.from(categorySet);
    }
  }

  getFilteredItems() {
    if (!this.pantry) return [];
    
    return this.pantry.items.filter(item => {
      const matchesCategory = this.selectedCategory === 'all' || item.category === this.selectedCategory;
      const matchesSearch = this.searchTerm === '' || 
                           item.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
                           item.category.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      return matchesCategory && matchesSearch;
    });
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

  getExpirationClass(date: Date): string {
    const today = new Date();
    const daysUntilExpiration = Math.floor((date.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
    
    if (daysUntilExpiration < 0) return 'expired';
    if (daysUntilExpiration < 7) return 'expiring-soon';
    if (daysUntilExpiration < 30) return 'expiring-month';
    return '';
  }
}
