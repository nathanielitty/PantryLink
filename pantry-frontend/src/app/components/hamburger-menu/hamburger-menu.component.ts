import { Component, OnInit, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Observable } from 'rxjs';
import { User } from '../../models/pantry.models';

@Component({
  selector: 'app-hamburger-menu',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './hamburger-menu.component.html',
  styleUrls: ['./hamburger-menu.component.scss']
})
export class HamburgerMenuComponent implements OnInit {
  isOpen = false;
  currentUser$: Observable<User | null>;

  constructor(private authService: AuthService) {
    this.currentUser$ = this.authService.currentUser$;
  }

  ngOnInit() {
    this.authService.getCurrentUser();
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.key === 'Escape' && this.isOpen) {
      this.closeMenu();
    }
  }

  toggleMenu() {
    this.isOpen = !this.isOpen;
    // Prevent body scrolling when menu is open
    if (this.isOpen) {
      document.body.style.overflow = 'hidden';
    } else {
      document.body.style.overflow = 'auto';
    }
  }

  closeMenu() {
    this.isOpen = false;
    document.body.style.overflow = 'auto';
  }

  logout() {
    this.authService.logout();
    this.closeMenu();
  }
}
