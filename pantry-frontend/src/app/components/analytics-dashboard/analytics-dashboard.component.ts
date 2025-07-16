import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AnalyticsService, AnalyticsDashboard, SystemAnalytics, PantryAnalytics } from '../../services/analytics.service';

@Component({
  selector: 'app-analytics-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  template: `
    <div class="analytics-container">
      <header class="analytics-header">
        <h1>PantryLink Analytics Dashboard</h1>
        <p class="header-subtitle">System performance and insights</p>
      </header>

      <div class="dashboard-grid" *ngIf="dashboardData">
        <!-- System Overview Cards -->
        <div class="stats-grid">
          <div class="stat-card primary">
            <div class="stat-icon">🏪</div>
            <div class="stat-content">
              <h3>{{ dashboardData.systemStats.totalActivePantries }}</h3>
              <p>Active Pantries</p>
            </div>
          </div>

          <div class="stat-card success">
            <div class="stat-icon">👥</div>
            <div class="stat-content">
              <h3>{{ dashboardData.systemStats.totalUsers }}</h3>
              <p>Registered Users</p>
            </div>
          </div>

          <div class="stat-card info">
            <div class="stat-icon">📦</div>
            <div class="stat-content">
              <h3>{{ dashboardData.systemStats.totalInventoryItems }}</h3>
              <p>Total Items</p>
            </div>
          </div>

          <div class="stat-card warning">
            <div class="stat-icon">🔍</div>
            <div class="stat-content">
              <h3>{{ dashboardData.systemStats.totalSearches }}</h3>
              <p>Total Searches</p>
            </div>
          </div>

          <div class="stat-card secondary">
            <div class="stat-icon">⭐</div>
            <div class="stat-content">
              <h3>{{ dashboardData.systemStats.averageSystemRating | number:'1.1-1' }}</h3>
              <p>Avg Rating</p>
            </div>
          </div>

          <div class="stat-card danger">
            <div class="stat-icon">📊</div>
            <div class="stat-content">
              <h3>{{ dashboardData.systemStats.totalItemsDistributed }}</h3>
              <p>Items Distributed</p>
            </div>
          </div>
        </div>

        <!-- Top Performing Pantries -->
        <div class="chart-section">
          <h2>Top Performing Pantries</h2>
          <div class="pantry-list">
            <div class="pantry-item" *ngFor="let pantry of dashboardData.topPerformingPantries">
              <div class="pantry-info">
                <h4>{{ pantry.pantryName }}</h4>
                <p>Rating: {{ pantry.averageRating | number:'1.1-1' }}/5.0</p>
                <p>Visitors: {{ pantry.visitorCount }} | Items: {{ pantry.totalItemsCount }}</p>
              </div>
              <div class="pantry-stats">
                <div class="progress-bar">
                  <div class="progress-fill" [style.width.%]="(pantry.averageRating / 5) * 100"></div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Popular Categories Chart -->
        <div class="chart-section">
          <h2>Popular Food Categories</h2>
          <div class="category-chart">
            <div class="category-item" *ngFor="let category of popularCategoriesArray">
              <div class="category-info">
                <span class="category-name">{{ category.name }}</span>
                <span class="category-count">{{ category.count }}</span>
              </div>
              <div class="category-bar">
                <div class="category-fill" [style.width.%]="(category.count / maxCategoryCount) * 100"></div>
              </div>
            </div>
          </div>
        </div>

        <!-- ZIP Code Search Analytics -->
        <div class="chart-section">
          <h2>Searches by ZIP Code</h2>
          <div class="zip-chart">
            <div class="zip-item" *ngFor="let zip of searchesByZipArray">
              <div class="zip-info">
                <span class="zip-code">{{ zip.zipCode }}</span>
                <span class="zip-searches">{{ zip.searches }} searches</span>
              </div>
              <div class="zip-bar">
                <div class="zip-fill" [style.width.%]="(zip.searches / maxZipSearches) * 100"></div>
              </div>
            </div>
          </div>
        </div>

        <!-- Recent Activity -->
        <div class="activity-section">
          <h2>Recent Pantry Activity</h2>
          <div class="activity-list">
            <div class="activity-item" *ngFor="let activity of dashboardData.recentActivity">
              <div class="activity-icon">📈</div>
              <div class="activity-content">
                <h4>{{ activity.pantryName }}</h4>
                <p>{{ activity.inventoryUpdatesCount }} updates | {{ activity.visitorCount }} visitors</p>
                <span class="activity-date">{{ activity.date | date:'shortDate' }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Loading State -->
      <div class="loading-container" *ngIf="isLoading">
        <div class="loading-spinner"></div>
        <p>Loading analytics data...</p>
      </div>

      <!-- Error State -->
      <div class="error-container" *ngIf="error">
        <h3>Unable to load analytics</h3>
        <p>{{ error }}</p>
        <button (click)="loadDashboard()" class="retry-button">Retry</button>
      </div>

      <!-- Refresh Controls -->
      <div class="controls-section">
        <button (click)="refreshData()" class="refresh-button" [disabled]="isLoading">
          {{ isLoading ? 'Updating...' : 'Refresh Data' }}
        </button>
        <button (click)="updateSystemAnalytics()" class="update-button" [disabled]="isLoading">
          Update System Analytics
        </button>
      </div>
    </div>
  `,
  styleUrls: ['./analytics-dashboard.component.scss']
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
