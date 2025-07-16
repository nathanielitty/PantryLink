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

  constructor(private analyticsService: AnalyticsService) {}

  ngOnInit(): void {
    this.loadDashboard();
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
        this.error = err.error?.message || 'Failed to load analytics data';
        this.isLoading = false;
        console.error('Analytics error:', err);
      }
    });
  }

  refreshData(): void {
    this.loadDashboard();
  }

  updateSystemAnalytics(): void {
    this.isLoading = true;
    this.analyticsService.updateSystemAnalytics().subscribe({
      next: () => {
        this.loadDashboard(); // Reload data after update
      },
      error: (err) => {
        this.error = 'Failed to update system analytics';
        this.isLoading = false;
        console.error('Update error:', err);
      }
    });
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
}
