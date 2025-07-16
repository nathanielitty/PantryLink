import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class HomeComponent {
  zipCode: string = '';

  onSearch() {
    if (this.zipCode && this.zipCode.length === 5) {
      // Navigate to pantry search with ZIP code
      console.log('Searching for pantries in:', this.zipCode);
    }
  }
}
