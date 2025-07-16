import { Component, OnInit, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';

interface InventoryItem {
  id: number;
  name: string;
  category: string;
  quantity: number;
  expirationDate: Date;
  nutritionalInfo: string;
  dietaryFlags: string[];
  dateAdded: Date;
  lastUpdated: Date;
  brand?: string;
  unit?: string;
}

@Component({
  selector: 'app-inventory-management',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule],
  templateUrl: './inventory-management.html',
  styleUrl: './inventory-management.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class InventoryManagement implements OnInit {
  pantryId: number = 0;
  pantryName: string = '';
  isLoading: boolean = true;
  isSaving: boolean = false;
  showAddItemModal: boolean = false;
  showEditItemModal: boolean = false;
  showDeleteConfirmation: boolean = false;
  
  // Search and filter
  searchTerm: string = '';
  selectedCategory: string = 'all';
  sortDirection: 'asc' | 'desc' = 'asc';
  sortColumn: string = 'name';
  
  // View mode
  isGridView: boolean = false;
  
  // Item form
  itemForm: FormGroup;
  editItemForm: FormGroup;
  
  // Selected item for edit/delete
  selectedItem: InventoryItem | null = null;
  itemToDelete: InventoryItem | null = null;
  
  // Inventory items
  inventoryItems: InventoryItem[] = [];
  
  // Categories
  categories: string[] = [
    'Canned Goods',
    'Produce',
    'Dairy',
    'Grains',
    'Proteins',
    'Beverages',
    'Snacks',
    'Baking',
    'Condiments',
    'Hygiene',
    'Baby',
    'Other'
  ];
  
  // Dietary flags
  dietaryFlags: string[] = [
    'Vegetarian',
    'Vegan',
    'Gluten-Free',
    'Dairy-Free',
    'Nut-Free',
    'Kosher',
    'Halal'
  ];
  
  // Selected dietary flags
  selectedDietaryFlags: string[] = [];
  
  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder
  ) {
    this.itemForm = this.fb.group({
      name: ['', Validators.required],
      category: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      expirationDate: ['', Validators.required],
      nutritionalInfo: [''],
      brand: [''],
      unit: ['items'],
      isVegan: [false],
      isVegetarian: [false],
      isGlutenFree: [false],
      isOrganic: [false]
    });
    
    this.editItemForm = this.fb.group({
      name: ['', Validators.required],
      category: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      expirationDate: ['', Validators.required],
      nutritionalInfo: [''],
      brand: [''],
      unit: ['items'],
      isVegan: [false],
      isVegetarian: [false],
      isGlutenFree: [false],
      isOrganic: [false]
    });
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.pantryId = +params['pantryId'];
      this.loadInventory();
    });
  }

  loadInventory() {
    this.isLoading = true;
    
    // Simulate API call
    setTimeout(() => {
      // Mock data
      this.pantryName = 'Community Food Bank';
      this.inventoryItems = [
        {
          id: 1,
          name: 'Canned Beans',
          category: 'Canned Goods',
          quantity: 45,
          expirationDate: new Date('2025-12-31'),
          nutritionalInfo: 'High in protein and fiber',
          dietaryFlags: ['Vegetarian', 'Vegan', 'Gluten-Free'],
          dateAdded: new Date('2025-06-01'),
          lastUpdated: new Date('2025-06-15'),
          brand: 'Green Valley',
          unit: 'cans'
        },
        {
          id: 2,
          name: 'Rice',
          category: 'Grains',
          quantity: 30,
          expirationDate: new Date('2026-06-30'),
          nutritionalInfo: 'Good source of carbohydrates',
          dietaryFlags: ['Vegetarian', 'Vegan', 'Gluten-Free'],
          dateAdded: new Date('2025-06-02'),
          lastUpdated: new Date('2025-06-02'),
          brand: 'Uncle Ben\'s',
          unit: 'lbs'
        },
        {
          id: 3,
          name: 'Pasta',
          category: 'Grains',
          quantity: 25,
          expirationDate: new Date('2026-03-15'),
          nutritionalInfo: 'Good source of carbohydrates',
          dietaryFlags: ['Vegetarian', 'Vegan'],
          dateAdded: new Date('2025-06-03'),
          lastUpdated: new Date('2025-06-10'),
          brand: 'Barilla',
          unit: 'boxes'
        },
        {
          id: 4,
          name: 'Canned Soup',
          category: 'Canned Goods',
          quantity: 20,
          expirationDate: new Date('2025-10-15'),
          nutritionalInfo: 'Varies by type',
          dietaryFlags: [],
          dateAdded: new Date('2025-06-04'),
          lastUpdated: new Date('2025-06-04'),
          brand: 'Campbell\'s',
          unit: 'cans'
        },
        {
          id: 5,
          name: 'Fresh Apples',
          category: 'Produce',
          quantity: 50,
          expirationDate: new Date('2025-08-10'),
          nutritionalInfo: 'High in fiber and vitamin C',
          dietaryFlags: ['Vegetarian', 'Vegan', 'Gluten-Free'],
          dateAdded: new Date('2025-06-05'),
          lastUpdated: new Date('2025-06-05'),
          brand: 'Local Farm',
          unit: 'lbs'
        },
        {
          id: 6,
          name: 'Milk',
          category: 'Dairy',
          quantity: 15,
          expirationDate: new Date('2025-08-05'),
          nutritionalInfo: 'Good source of calcium and vitamin D',
          dietaryFlags: ['Vegetarian'],
          dateAdded: new Date('2025-06-06'),
          lastUpdated: new Date('2025-06-06'),
          brand: 'Organic Valley',
          unit: 'gallons'
        }
      ];
      
      this.isLoading = false;
    }, 1000);
  }

  getFilteredItems(): InventoryItem[] {
    return this.inventoryItems.filter(item => {
      // Category filter
      const matchesCategory = this.selectedCategory === 'all' || item.category === this.selectedCategory;
      
      // Search term filter
      const matchesSearch = this.searchTerm === '' || 
                           item.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
                           item.category.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      return matchesCategory && matchesSearch;
    }).sort((a, b) => {
      // Sort by selected column
      let comparison = 0;
      
      switch (this.sortColumn) {
        case 'name':
          comparison = a.name.localeCompare(b.name);
          break;
        case 'category':
          comparison = a.category.localeCompare(b.category);
          break;
        case 'quantity':
          comparison = a.quantity - b.quantity;
          break;
        case 'expirationDate':
          comparison = a.expirationDate.getTime() - b.expirationDate.getTime();
          break;
        default:
          comparison = 0;
      }
      
      // Apply sort direction
      return this.sortDirection === 'asc' ? comparison : -comparison;
    });
  }

  openAddItemModal() {
    this.itemForm.reset();
    this.itemForm.patchValue({
      category: this.categories[0],
      quantity: 1
    });
    this.selectedDietaryFlags = [];
    this.showAddItemModal = true;
  }

  openEditItemModal(item: InventoryItem) {
    this.selectedItem = item;
    this.itemForm.patchValue({
      name: item.name,
      category: item.category,
      quantity: item.quantity,
      expirationDate: this.formatDateForInput(item.expirationDate),
      nutritionalInfo: item.nutritionalInfo
    });
    this.selectedDietaryFlags = [...item.dietaryFlags];
    this.showEditItemModal = true;
  }

  openDeleteConfirmation(item: InventoryItem) {
    this.selectedItem = item;
    this.showDeleteConfirmation = true;
  }

  closeModals() {
    this.showAddItemModal = false;
    this.showEditItemModal = false;
    this.showDeleteConfirmation = false;
    this.selectedItem = null;
  }

  addItem() {
    if (this.itemForm.valid) {
      this.isSaving = true;
      
      // Prepare new item data
      const newItem: InventoryItem = {
        id: this.getNextId(),
        name: this.itemForm.value.name,
        category: this.itemForm.value.category,
        quantity: this.itemForm.value.quantity,
        expirationDate: new Date(this.itemForm.value.expirationDate),
        nutritionalInfo: this.itemForm.value.nutritionalInfo,
        dietaryFlags: [...this.selectedDietaryFlags],
        dateAdded: new Date(),
        lastUpdated: new Date()
      };
      
      // Simulate API call
      setTimeout(() => {
        this.inventoryItems.push(newItem);
        this.isSaving = false;
        this.closeModals();
      }, 1000);
    }
  }

  updateItem() {
    if (this.itemForm.valid && this.selectedItem) {
      this.isSaving = true;
      
      // Prepare updated item data
      const updatedItem: InventoryItem = {
        ...this.selectedItem,
        name: this.itemForm.value.name,
        category: this.itemForm.value.category,
        quantity: this.itemForm.value.quantity,
        expirationDate: new Date(this.itemForm.value.expirationDate),
        nutritionalInfo: this.itemForm.value.nutritionalInfo,
        dietaryFlags: [...this.selectedDietaryFlags],
        lastUpdated: new Date()
      };
      
      // Simulate API call
      setTimeout(() => {
        const index = this.inventoryItems.findIndex(item => item.id === this.selectedItem?.id);
        if (index !== -1) {
          this.inventoryItems[index] = updatedItem;
        }
        this.isSaving = false;
        this.closeModals();
      }, 1000);
    }
  }

  sortBy(column: string) {
    if (this.sortColumn === column) {
      // Toggle direction if already sorting by this column
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      // Set new sort column and default to ascending
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }
    
    // Sort the inventory items
    this.inventoryItems.sort((a, b) => {
      let aValue = (a as any)[column];
      let bValue = (b as any)[column];
      
      // Handle date sorting
      if (column === 'expirationDate' || column === 'dateAdded') {
        aValue = new Date(aValue).getTime();
        bValue = new Date(bValue).getTime();
      }
      
      // Handle string sorting
      if (typeof aValue === 'string') {
        aValue = aValue.toLowerCase();
        bValue = bValue.toLowerCase();
      }
      
      if (this.sortDirection === 'asc') {
        return aValue > bValue ? 1 : -1;
      } else {
        return aValue < bValue ? 1 : -1;
      }
    });
  }

  toggleDietaryFlag(flag: string) {
    const index = this.selectedDietaryFlags.indexOf(flag);
    if (index === -1) {
      this.selectedDietaryFlags.push(flag);
    } else {
      this.selectedDietaryFlags.splice(index, 1);
    }
  }

  isDietaryFlagSelected(flag: string): boolean {
    return this.selectedDietaryFlags.includes(flag);
  }

  getExpirationClass(date: Date): string {
    const today = new Date();
    const daysUntilExpiration = Math.floor((date.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
    
    if (daysUntilExpiration < 0) return 'expired';
    if (daysUntilExpiration < 7) return 'expiring-soon';
    if (daysUntilExpiration < 30) return 'expiring-month';
    return '';
  }

  private getNextId(): number {
    return Math.max(...this.inventoryItems.map(item => item.id), 0) + 1;
  }

  private formatDateForInput(date: Date): string {
    return date.toISOString().split('T')[0];
  }
  
  // Methods referenced in the new HTML template
  getExpiringCount(): number {
    const today = new Date();
    const sevenDaysFromNow = new Date(today.getTime() + 7 * 24 * 60 * 60 * 1000);
    return this.inventoryItems.filter(item => 
      new Date(item.expirationDate) <= sevenDaysFromNow && new Date(item.expirationDate) >= today
    ).length;
  }
  
  getCategoryCount(): number {
    const uniqueCategories = new Set(this.inventoryItems.map(item => item.category));
    return uniqueCategories.size;
  }
  
  toggleView(): void {
    this.isGridView = !this.isGridView;
  }
  
  trackByFn(index: number, item: InventoryItem): number {
    return item.id;
  }
  
  isExpiringSoon(date: Date): boolean {
    const today = new Date();
    const sevenDaysFromNow = new Date(today.getTime() + 7 * 24 * 60 * 60 * 1000);
    return new Date(date) <= sevenDaysFromNow;
  }
  
  isLowStock(quantity: number): boolean {
    return quantity <= 5; // Consider items with 5 or fewer as low stock
  }
  
  filterExpiringSoon(): void {
    // Filter to show only items expiring soon
    this.selectedCategory = 'all';
    this.searchTerm = '';
    // Additional filtering logic can be added here
  }
  
  filterLowStock(): void {
    // Filter to show only low stock items
    this.selectedCategory = 'all';
    this.searchTerm = '';
    // Additional filtering logic can be added here
  }
  
  exportInventory(): void {
    console.log('Exporting inventory...');
    // Implementation for exporting inventory data
  }
  
  printInventory(): void {
    console.log('Printing inventory...');
    // Implementation for printing inventory
    window.print();
  }
  
  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString();
  }
  
  closeAddItemModal(): void {
    this.showAddItemModal = false;
    this.itemForm.reset();
  }
  
  closeEditItemModal(): void {
    this.showEditItemModal = false;
    this.editItemForm.reset();
    this.selectedItem = null;
  }
  
  closeDeleteConfirmation(): void {
    this.showDeleteConfirmation = false;
    this.itemToDelete = null;
  }
  
  editItem(item: InventoryItem): void {
    this.selectedItem = item;
    this.editItemForm.patchValue({
      name: item.name,
      category: item.category,
      quantity: item.quantity,
      expirationDate: this.formatDateForInput(new Date(item.expirationDate)),
      nutritionalInfo: item.nutritionalInfo
    });
    this.showEditItemModal = true;
  }
  
  deleteItem(id: number): void {
    const item = this.inventoryItems.find(i => i.id === id);
    if (item) {
      this.itemToDelete = item;
      this.showDeleteConfirmation = true;
    }
  }
  
  confirmDelete(): void {
    if (this.itemToDelete) {
      this.inventoryItems = this.inventoryItems.filter(item => item.id !== this.itemToDelete!.id);
      this.closeDeleteConfirmation();
    }
  }
}
