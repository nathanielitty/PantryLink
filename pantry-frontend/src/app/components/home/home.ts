import { Component, CUSTOM_ELEMENTS_SCHEMA, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './home.html',
  styleUrl: './home.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class HomeComponent implements AfterViewInit {
  zipCode: string = '';

  constructor(private router: Router) {}

  ngAfterViewInit() {
    // Initialize counter animations
    this.initCounters();
    
    // Initialize intersection observer for animations
    this.initAnimationObserver();
  }

  onSearch() {
    if (this.zipCode && this.zipCode.length === 5) {
      this.router.navigate(['/search'], { queryParams: { zip: this.zipCode } });
    }
  }
  
  private initCounters() {
    const counters = document.querySelectorAll('.counter');
    
    counters.forEach(counter => {
      const target = parseInt(counter.getAttribute('data-target') || '0', 10);
      const duration = 2000; // 2 seconds
      const step = Math.ceil(target / (duration / 20)); // Update every 20ms
      
      let current = 0;
      const updateCounter = () => {
        current += step;
        if (current > target) {
          current = target;
        }
        
        counter.textContent = current.toString();
        
        if (current < target) {
          setTimeout(updateCounter, 20);
        }
      };
      
      // Start the counter animation when element is in viewport
      const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
          if (entry.isIntersecting) {
            updateCounter();
            observer.unobserve(entry.target);
          }
        });
      }, { threshold: 0.5 });
      
      observer.observe(counter);
    });
  }
  
  private initAnimationObserver() {
    // Add animation classes when elements come into view
    const animatedElements = document.querySelectorAll('.animate-slide-up, .animate-slide-left, .animate-slide-right, .animate-fade-in');
    
    const observer = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          entry.target.classList.add('animated');
          observer.unobserve(entry.target);
        }
      });
    }, { threshold: 0.1 });
    
    animatedElements.forEach(el => {
      observer.observe(el);
    });
  }
}
