import { Component, signal, HostListener, AfterViewInit } from '@angular/core';
import { RouterOutlet, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterModule, CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements AfterViewInit {
  protected readonly title = signal('PantryLink');
  isScrolled = false;

  constructor(public authService: AuthService) {}
  
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

  logout() {
    this.authService.logout();
  }
}
