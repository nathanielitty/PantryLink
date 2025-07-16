import { Component, OnInit, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AnalyticsService, AnalyticsDashboard, SystemAnalytics, PantryAnalytics } from '../../services/analytics.service';

@Component({
  selector: 'app-analytics-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './analytics-dashboard.component.html',
  styleUrls: ['./analytics-dashboard.component.scss'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AnalyticsDashboardComponent implements OnInit {
  dashboardData: AnalyticsDashboard | null = null;
  isLoading = true;
  error: string | null = null;

  popularCategoriesArray: { name: string; count: number }[] = [];
  searchesByZipArray: { zipCode: string; searches: number }[] = [];
  maxCategoryCount = 0;
  maxZipSearches = 0;

  // Make Object available in template
  Object = Object;

  constructor(private analyticsService: AnalyticsService) {}

  ngOnInit(): void {
    // Always start with sample data for demo purposes
    this.dashboardData = this.getSampleData();
    this.processChartData();
    this.isLoading = false;
    
    // Comment out API loading for now to use sample data
    // this.loadDashboard();
  }

  private getSampleData(): AnalyticsDashboard {
    return {
      systemStats: {
        id: 1,
        date: new Date().toISOString(),
        totalActivePantries: 47,
        totalUsers: 2634,
        totalInventoryItems: 18547,
        totalSearches: 5892,
        totalRecommendations: 1856,
        mostSearchedZipCode: '90210',
        totalItemsDistributed: 12847,
        averageSystemRating: 4.6
      },
      topPerformingPantries: [
        {
          id: 1,
          pantryId: 1,
          pantryName: 'Downtown Community Food Bank',
          date: new Date().toISOString(),
          totalItemsCount: 2850,
          totalQuantity: 4240,
          expiringItemsCount: 65,
          visitorCount: 485,
          inventoryUpdatesCount: 42,
          averageRating: 4.9,
          mostPopularCategory: 'Canned Goods',
          itemsDistributedCount: 1805
        },
        {
          id: 2,
          pantryId: 2,
          pantryName: 'Riverside Family Food Center',
          date: new Date().toISOString(),
          totalItemsCount: 2423,
          totalQuantity: 3610,
          expiringItemsCount: 48,
          visitorCount: 372,
          inventoryUpdatesCount: 38,
          averageRating: 4.7,
          mostPopularCategory: 'Fresh Produce',
          itemsDistributedCount: 1487
        },
        {
          id: 3,
          pantryId: 3,
          pantryName: 'Westside Neighborhood Kitchen',
          date: new Date().toISOString(),
          totalItemsCount: 2156,
          totalQuantity: 3200,
          expiringItemsCount: 34,
          visitorCount: 298,
          inventoryUpdatesCount: 35,
          averageRating: 4.5,
          mostPopularCategory: 'Dairy Products',
          itemsDistributedCount: 1223
        },
        {
          id: 4,
          pantryId: 4,
          pantryName: 'Eastside Community Pantry',
          date: new Date().toISOString(),
          totalItemsCount: 1934,
          totalQuantity: 2876,
          expiringItemsCount: 28,
          visitorCount: 256,
          inventoryUpdatesCount: 29,
          averageRating: 4.4,
          mostPopularCategory: 'Bakery Items',
          itemsDistributedCount: 1045
        },
        {
          id: 5,
          pantryId: 5,
          pantryName: 'Northside Food Hub',
          date: new Date().toISOString(),
          totalItemsCount: 1798,
          totalQuantity: 2654,
          expiringItemsCount: 31,
          visitorCount: 234,
          inventoryUpdatesCount: 26,
          averageRating: 4.2,
          mostPopularCategory: 'Meat & Protein',
          itemsDistributedCount: 978
        }
      ],
      popularCategories: {
        'Canned Goods': 3847,
        'Fresh Produce': 3256,
        'Dairy Products': 2923,
        'Bakery Items': 2634,
        'Meat & Protein': 2298,
        'Frozen Foods': 1987,
        'Pantry Staples': 1756,
        'Snacks & Beverages': 1587,
        'Baby Food & Formula': 1234,
        'Personal Care Items': 1087,
        'Household Essentials': 956,
        'Pet Food': 742,
        'Health & Wellness': 645,
        'School Supplies': 523,
        'Holiday Items': 387
      },
      searchesByZipCode: {
        '90210': 542,
        '10001': 498,
        '60601': 487,
        '77001': 456,
        '30309': 434,
        '94102': 398,
        '02101': 387,
        '20001': 365,
        '33101': 342,
        '85001': 298,
        '97201': 287,
        '78701': 256,
        '19101': 234,
        '80201': 198,
        '84101': 187
      },
      recentActivity: [
        {
          id: 6,
          pantryId: 6,
          pantryName: 'Central City Food Network',
          date: new Date(Date.now() - 1 * 24 * 60 * 60 * 1000).toISOString(),
          totalItemsCount: 1654,
          totalQuantity: 2376,
          expiringItemsCount: 22,
          visitorCount: 186,
          inventoryUpdatesCount: 18,
          averageRating: 4.3,
          mostPopularCategory: 'Fresh Produce',
          itemsDistributedCount: 845
        },
        {
          id: 7,
          pantryId: 7,
          pantryName: 'Southside Community Kitchen',
          date: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000).toISOString(),
          totalItemsCount: 1534,
          totalQuantity: 2154,
          expiringItemsCount: 19,
          visitorCount: 163,
          inventoryUpdatesCount: 15,
          averageRating: 4.1,
          mostPopularCategory: 'Canned Goods',
          itemsDistributedCount: 678
        },
        {
          id: 8,
          pantryId: 8,
          pantryName: 'Valley Food Assistance Center',
          date: new Date(Date.now() - 3 * 24 * 60 * 60 * 1000).toISOString(),
          totalItemsCount: 1423,
          totalQuantity: 1976,
          expiringItemsCount: 25,
          visitorCount: 152,
          inventoryUpdatesCount: 14,
          averageRating: 4.0,
          mostPopularCategory: 'Dairy Products',
          itemsDistributedCount: 612
        }
      ]
    };
  }

  loadDashboard(): void {
    this.isLoading = true;
    this.error = null;

    this.analyticsService.getDashboard().subscribe({
      next: (data) => {
        this.dashboardData = data;
        this.processChartData();
        this.isLoading = false;
      },
      error: (err) => {
        // Use sample data when API fails
        this.dashboardData = this.getDisplayData();
        this.processChartData();
        this.isLoading = false;
        console.log('Using sample data due to API error:', err);
      }
    });
  }

  refreshData(): void {
    this.isLoading = true;
    // Simulate loading delay for demo
    setTimeout(() => {
      this.dashboardData = this.getSampleData();
      this.processChartData();
      this.isLoading = false;
    }, 1000);
  }

  updateSystemAnalytics(): void {
    this.isLoading = true;
    // Simulate processing delay for demo
    setTimeout(() => {
      this.dashboardData = this.getSampleData();
      this.processChartData();
      this.isLoading = false;
    }, 1500);
  }

  private processChartData(): void {
    if (!this.dashboardData) return;

    // Process popular categories
    this.popularCategoriesArray = Object.entries(this.dashboardData.popularCategories)
      .map(([name, count]) => ({ name, count }))
      .sort((a, b) => b.count - a.count);
    
    this.maxCategoryCount = Math.max(...this.popularCategoriesArray.map(c => c.count));

    // Process searches by ZIP code
    this.searchesByZipArray = Object.entries(this.dashboardData.searchesByZipCode)
      .map(([zipCode, searches]) => ({ zipCode, searches }))
      .sort((a, b) => b.searches - a.searches);
    
    this.maxZipSearches = Math.max(...this.searchesByZipArray.map(z => z.searches));
  }

  // For development: provide sample data when backend is not available
  getDisplayData(): AnalyticsDashboard {
    return this.dashboardData || this.getSampleData();
  }
}
