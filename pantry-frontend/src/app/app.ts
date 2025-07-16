import { Component, AfterViewInit, HostListener, signal, OnInit } from '@angular/core';
import { RouterOutlet, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth.service';
import { BreadcrumbService, Breadcrumb } from './services/breadcrumb.service';
import { HamburgerMenuComponent } from './components/hamburger-menu/hamburger-menu.component';
import { Observable } from 'rxjs';
import { User } from './models/pantry.models';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterModule, CommonModule, HamburgerMenuComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements AfterViewInit, OnInit {
  protected readonly title = signal('PantryLink');
  isScrolled = false;
  breadcrumbs: Breadcrumb[] = [];
  fabOpen = false;
  currentUser$: Observable<User | null>;

  constructor(
    public authService: AuthService,
    private breadcrumbService: BreadcrumbService
  ) {
    // Subscribe to breadcrumb changes
    this.breadcrumbService.breadcrumbs$.subscribe(breadcrumbs => {
      this.breadcrumbs = breadcrumbs;
    });
    
    // Subscribe to current user changes
    this.currentUser$ = this.authService.currentUser$;
  }

  ngOnInit() {
    // Initialize current user from localStorage
    this.authService.getCurrentUser();
  }
  
  ngAfterViewInit() {
    // Initialize animations
    this.animateOnScroll();
  }
  
  @HostListener('window:scroll', [])
  onWindowScroll() {
    // Add scrolled class to navbar when page is scrolled
    this.isScrolled = window.scrollY > 50;
    const navbar = document.querySelector('.navbar');
    if (navbar) {
      if (this.isScrolled) {
        navbar.classList.add('scrolled');
      } else {
        navbar.classList.remove('scrolled');
      }
    }
    
    // Trigger animations for elements in viewport
    this.animateOnScroll();
  }
  
  private animateOnScroll() {
    const animatedElements = document.querySelectorAll('.animate-slide-up, .animate-slide-left, .animate-slide-right, .animate-fade-in');
    
    animatedElements.forEach(element => {
      const elementTop = element.getBoundingClientRect().top;
      const elementBottom = element.getBoundingClientRect().bottom;
      
      // Check if element is in viewport
      if (elementTop < window.innerHeight && elementBottom > 0) {
        element.classList.add('animated');
      }
    });
  }

  showAbout() {
    alert(`PantryLink - Food Pantry Network

Version: 1.0.0
Developed: 2025

PantryLink is your ZIP-code-centric food pantry network that connects communities with local food resources. We provide:

• Real-time pantry locator with distance calculations
• Food and monetary donation capabilities  
• Inventory management for pantry administrators
• Analytics dashboard for tracking community impact
• AI-powered recommendations for optimal pantry matching

Built with Angular 20 and ASP.NET Core 8
© 2025 PantryLink. All rights reserved.`);
  }

  showHelp() {
    alert(`PantryLink Help & Support

Getting Started:
1. Enter your ZIP code on the home page to find nearby pantries
2. Click "Find Pantries" to view available food resources
3. Use the "Donate" menu to contribute food or money
4. Create an account to access personalized features

Features:
• Pantry Search: Find food pantries by ZIP code
• Donations: Contribute food items or monetary donations
• User Preferences: Set dietary restrictions and preferences
• Analytics: View pantry usage and community impact (logged in users)

For technical support or questions:
• Check the FAQ section
• Contact support at support@pantrylink.org
• Visit our documentation at docs.pantrylink.org

Need immediate help? Call our support line: 1-800-PANTRY-1`);
  }

  toggleFab() {
    this.fabOpen = !this.fabOpen;
  }

  logout() {
    this.authService.logout();
    window.location.reload();
  }
}
